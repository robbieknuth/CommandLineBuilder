using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace CommandLine
{
    internal sealed class Command
    {
        private readonly CommandName name;
        private readonly List<Command> children;
        private readonly Command? parent;
        private readonly HashSet<CommandName> existingInnerCommandNames;
        private readonly Type? entrypointType;
        private readonly Type? settingsType;
        private readonly Dictionary<OptionName, OptionApplicator>? optionApplicators;
        private readonly Dictionary<OptionName, Action<object>>? switchApplicators;
        private readonly List<PositionalApplicator>? positionalApplicators;

        private Command(
            Command? parent,
            CommandName commandName,
            Type? entrypointType,
            Type? settingsType)
        {
            this.parent = parent;
            this.name = commandName;
            this.entrypointType = entrypointType;
            this.settingsType = settingsType;

            Debug.Assert(
                this.parent == null ||
                this.parent.settingsType == null || (
                    this.settingsType != null &&
                    this.parent.settingsType.IsAssignableFrom(settingsType)));

            if (this.settingsType != null)
            {
                this.optionApplicators = new Dictionary<OptionName, OptionApplicator>();
                this.switchApplicators = new Dictionary<OptionName, Action<object>>();

                if (this.entrypointType != null)
                {
                    this.positionalApplicators = new List<PositionalApplicator>();
                }
            }

            this.children = new List<Command>();
            this.existingInnerCommandNames = new HashSet<CommandName>();
        }

        public static Command CreateRoot(string applicationName) => new Command(null, CommandName.FromApplicationName(applicationName), null, null);

        public static Command CreateRoot<T>(string applicationName) => new Command(null, CommandName.FromApplicationName(applicationName), null, typeof(T));

        public static Command CreateChild(
            Command parent,
            string name,
            Type? entrypointType,
            Type? settingsType)
        {
            if (!CommandName.TryConstruct(name, out var commandName))
            {
                throw new CommandStructureException($"Command name '{ name }' is invalid.");
            }

            if (parent.existingInnerCommandNames.TryGetValue(commandName, out var existingValue))
            {
                throw new CommandStructureException($"Command name '{ commandName}' conflicts with existing command '{ existingValue }' in command '{ parent.name }'.");
            }

            Debug.Assert(parent.entrypointType == null);

            return new Command(parent, commandName, entrypointType, settingsType);
        }

        public bool TryConsume(
            ParseContext parseContext,
            ReadOnlySpan<string> args,
            [NotNullWhen(true)]
            out IEntrypoint? entrypoint)
        {
            // the root command should always try and consume.
            if (!this.IsRootCommand())
            {
                if (args.Length == 0 ||
                    !CommandName.TryConstruct(args[0], out var name) ||
                    !this.name.Equals(name))
                {
                    entrypoint = null;
                    return false;
                }

                // consume the name of this command
                args = args.Slice(1);
            }

            while (
                this.TryConsumeSwitch(parseContext, ref args) ||
                this.TryConsumeOption(parseContext, ref args))
            {
                // this will eventually fail
            }

            // consume sub commands
            foreach (var tokenConsumer in this.children)
            {
                if (tokenConsumer.TryConsume(parseContext, args, out entrypoint))
                {
                    return true;
                }
            }

            var positionalIndex = 0;
            while (this.TryConsumeSwitch(parseContext, ref args) ||
                   this.TryConsumeOption(parseContext, ref args) || 
                   this.TryConsumePositional(parseContext, ref args, ref positionalIndex))
            {
                // this will fail eventually
            }

            if (args.Length > 0)
            {
                string error;
                if (OptionName.LooksLikeOptionOrSwitch(args[0]))
                {
                    error = $"Unrecognized option or switch { args[0] }";
                }
                else if (this.IsTerminalCommand())
                {
                    error = $"Unrecognized positional: { args[0] }";
                }
                else
                {
                    error = $"Unrecognized command: { args[0] }";
                }
                entrypoint = new ErrorEntrypoint(parseContext, error);
                return true;
            }

            if (this.IsTerminalCommand())
            {
                if (this.settingsType != null)
                {
                    var settings = Activator.CreateInstance(this.settingsType);
                    if (settings == null)
                    {
                        throw new Exception($"Unable to create settings type { this.settingsType.FullName }");
                    }

                    foreach (var applicator in parseContext.OptionApplicators)
                    {
                        var result = applicator(settings);
                        if (!result.IsSuccess)
                        {
                            entrypoint = new ErrorEntrypoint(parseContext, result.Error);
                            return true;
                        }
                    }

                    foreach (var applicator in parseContext.SwitchApplicators)
                    {
                        applicator(settings);
                    }

                    if (this.positionalApplicators != null)
                    {
                        foreach (var applicator in parseContext.PositionalApplicators)
                        {
                            var application = applicator(settings);
                            if (!application.IsSuccess)
                            {
                                entrypoint = new ErrorEntrypoint(parseContext, application.Error);
                                return true;
                            }
                        }
                    }

                    var actualEntrypoint = Activator.CreateInstance(this.entrypointType!)!;

                    entrypoint = (IEntrypoint?)Activator.CreateInstance(
                        typeof(EntrypointWithSettingThunk<>).MakeGenericType(this.settingsType!),
                        new object[]
                        {
                            actualEntrypoint,
                            settings
                        });

                    return true;
                }
                else
                {
                    entrypoint = (IEntrypoint?)Activator.CreateInstance(this.entrypointType!);
                    return true;
                }
            }
            else
            {
                string error;
                if (this.IsRootCommand())
                {
                    error = $"No subcommand given.";
                }
                else
                {
                    error = $"Command { this.name } requires a subcommand.";
                }
                
                entrypoint = new HelpEntrypoint(parseContext, this, error);
                return true;
            }
        }

        internal void AddSubCommand(Command innerCommand)
        {
            if (this.existingInnerCommandNames.TryGetValue(innerCommand.name, out var existingValue))
            {
                throw new CommandStructureException($"Incoming command name { innerCommand.name } conflicts with command name { existingValue }.");
            }

            Debug.Assert(this.entrypointType == null);
            this.existingInnerCommandNames.Add(innerCommand.name);
            this.children.Add(innerCommand);
        }

        internal void AddSettingFunction(OptionName optionName, Func<object, string, ApplicationResult> applicator)
        {
            Debug.Assert(this.settingsType != null);
            Debug.Assert(this.optionApplicators != null);

            EnsureOptionAndSwitchUniqueUpChain(optionName, this);
            this.optionApplicators![optionName] = new OptionApplicator(optionName, applicator);
        }

        internal void AddPositionalFunction(string name, Func<object, string, ApplicationResult> applicator)
        {
            Debug.Assert(this.settingsType != null);
            Debug.Assert(this.entrypointType != null);
            Debug.Assert(this.positionalApplicators != null);

            this.positionalApplicators!.Add(new PositionalApplicator(name, applicator));
        }

        internal void AddSwitchFunction(OptionName switchName, Action<object> applier)
        {
            Debug.Assert(this.settingsType != null);
            EnsureOptionAndSwitchUniqueUpChain(switchName, this);
            this.switchApplicators![switchName] = applier;
        }

        private static void EnsureOptionAndSwitchUniqueUpChain(OptionName optionName, Command? command)
        {
            if (command == null || command.optionApplicators == null || command.switchApplicators == null)
            {
                return;
            }

            if (command.optionApplicators.ContainsKey(optionName))
            {
                // find the actual existing option
                var existingOptionName = command.optionApplicators.Keys.First(x => x.Equals(optionName));
                throw new CommandStructureException($"Option or switch '{ optionName }' already has a setting function '{ existingOptionName }' in command '{ command.name }'.");
            }
            else if (command.switchApplicators.ContainsKey(optionName))
            {
                var existingSwitchName = command.switchApplicators.Keys.First(x => x.Equals(optionName));
                throw new CommandStructureException($"Option or switch '{ optionName }' already has a switch '{ existingSwitchName }' in command '{ command.name }'.");
            }

            EnsureOptionAndSwitchUniqueUpChain(optionName, command.parent);
        }

        private bool TryConsumeSwitch(ParseContext parseContext, ref ReadOnlySpan<string> args)
        {
            if (args.Length == 0)
            {
                return false;
            }

            if (!OptionName.FromUnknownValue(args[0], out var switchName))
            {
                return false;
            }

            return this.parent?.TryConsumeSwitchRec(switchName!, parseContext, ref args) ?? false;
        }

        private bool TryConsumeSwitchRec(OptionName switchName, ParseContext parseContext, ref ReadOnlySpan<string> args)
        {
            // settings can't be on the parent and not on the child. so if we encounter a null
            // value we're done.
            if (this.switchApplicators == null)
            {
                return false;
            }

            if (this.switchApplicators.TryGetValue(switchName, out var applicator))
            {
                // consume argument name only since this is a switch
                args = args.Slice(1);
                parseContext.SwitchApplicators.Add((o) => applicator(o));
                return true;
            }

            return this.parent?.TryConsumeSwitchRec(switchName, parseContext, ref args) ?? false;
        }

        private bool TryConsumeOption(ParseContext parseContext, ref ReadOnlySpan<string> args)
        {
            if (args.Length == 0)
            {
                return false;
            }

            if (!OptionName.FromUnknownValue(args[0], out var optionName))
            {
                return false;
            }

            return this.TryConsumeOptionRec(optionName!, parseContext, ref args);
        }

        private bool TryConsumeOptionRec(OptionName optionName, ParseContext parseContext, ref ReadOnlySpan<string> args)
        {
            // settings can't be on the parent and not on the child. so if we encounter a null
            // value we're done.
            if (this.optionApplicators == null)
            {
                return false;
            }

            if (this.optionApplicators.TryGetValue(optionName, out var applicator))
            {
                if (args.Length == 1)
                {
                    // jknuth TODO
                    throw new Exception("Option requires argument");
                }

                var value = args[1];

                // consume argument name and value
                args = args.Slice(2);
                parseContext.OptionApplicators.Add(applicator.Apply(value));
                return true;
            }

            return this.parent?.TryConsumeOptionRec(optionName, parseContext, ref args) ?? false;
        }

        private bool TryConsumePositional(ParseContext parseContext, ref ReadOnlySpan<string> args, ref int index)
        {
            if (this.positionalApplicators == null)
            {
                return false;
            }

            if (args.Length == 0)
            {
                return false;
            }
            else if (index >= this.positionalApplicators.Count)
            {
                return false;
            }

            // consume argument name only since this is a switch
            var value = args[0];
            args = args.Slice(1);
            var target = this.positionalApplicators[index];
            parseContext.PositionalApplicators.Add(target.Apply(value));
            index++;
            return true;
        }

        private bool IsRootCommand() => this.parent == null;

        private Command GetRootCommand()
        {
            if (this.IsRootCommand())
            {
                return this;
            }
            else
            {
                return this.parent!.GetRootCommand();
            }
        }

        private bool IsTerminalCommand() => this.entrypointType != null;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            var names = new List<string>();

            if (this.IsRootCommand())
            {
                names.Add(this.name.ToString());
            }
            
            this.CollectNames(names);
            stringBuilder.AppendLine("Usage:");
            if (this.positionalApplicators != null)
            {
                names.AddRange(this.positionalApplicators.Select(x => $"[{x.Name}]"));
            }

            if (this.children.Count > 0)
            {
                names.Add("[command]");
            }

            var hasOptions = false;
            if (this.switchApplicators?.Count > 0 || this.optionApplicators?.Count > 0)
            {
                hasOptions = true;
                names.Add("[options]");
            }

            stringBuilder.AppendLine(string.Join(" ", names));

            if (this.children.Count > 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Commands:");
                foreach (var command in this.children)
                {
                    stringBuilder.AppendLine($"    { command.name.ToString() }");
                }
            }

            if (hasOptions)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Options:");
            }

            if (this.switchApplicators != null)
            {
                foreach (var switchApplicator in this.switchApplicators)
                {
                    stringBuilder.AppendLine($"    { switchApplicator.Key.ToHelpString() }");
                }
            }

            if (this.optionApplicators != null)
            {
                foreach (var valueApplicator in this.optionApplicators)
                {
                    stringBuilder.AppendLine($"    { valueApplicator.Key.ToHelpString() }");
                }
            }

            return stringBuilder.ToString();
        }

        private void CollectNames(List<string> names)
        {
            if (!this.IsRootCommand())
            {
                this.parent!.CollectNames(names);
                names.Add(this.name.ToString());
            }
        }
    }
}
