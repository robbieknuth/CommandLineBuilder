using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class CommandLineBuilder<TSettings> : INonTerminalCommandWithSettingsBuilder<CommandLineBuilder<TSettings>, TSettings>
        where TSettings : new()
    {
        Command ICommandBuilder.Command => this.command;

        private readonly Command command;
        private readonly HelpOptions helpOptions;
        private readonly ParserOptions parserOptions;

        public CommandLineBuilder(string applicationName)
        {
            this.command = Command.CreateRoot<TSettings>(applicationName);
            this.helpOptions = new HelpOptions();
            this.parserOptions = new ParserOptions();
        }

        public CommandLineBuilder<TSettings> AddNonTerminalCommandWithSettings<TDerivedSettings>(string name, Action<NonTerminalCommandWithSettingsBuilder<TDerivedSettings>> commandBuilder)
         where TDerivedSettings : TSettings, new()
            => this.InternalAddNonTerminalCommandWithSettings<CommandLineBuilder<TSettings>, TSettings, TDerivedSettings>(name, commandBuilder);

        public CommandLineBuilder<TSettings> AddOption<TPropertyValue>(
            string longForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
            => this.AddOption(OptionDefinition<TSettings>.Create(longForm, property, conversion));

        public CommandLineBuilder<TSettings> AddOption<TPropertyValue>(
            string longForm,
            string shortForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
            => this.AddOption(OptionDefinition<TSettings>.Create(longForm, shortForm, property, conversion));

        public CommandLineBuilder<TSettings> AddOption(OptionDefinition<TSettings> optionDefinition)
            => this.InternalAddOption<CommandLineBuilder<TSettings>, TSettings>(optionDefinition);

        public CommandLineBuilder<TSettings> AddSwitch(
            string longForm,
            Action<TSettings> applicator)
            => this.AddSwitch(SwitchDefinition<TSettings>.Create(longForm, applicator));

        public CommandLineBuilder<TSettings> AddSwitch(
            string longForm,
            string shortForm,
            Action<TSettings> applicator)
            => this.AddSwitch(SwitchDefinition<TSettings>.Create(longForm, shortForm, applicator));

        public CommandLineBuilder<TSettings> AddSwitch(SwitchDefinition<TSettings> switchDefinition)
            => this.InternalAddSwitch<CommandLineBuilder<TSettings>, TSettings>(switchDefinition);

        public CommandLineBuilder<TSettings> AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(
            string name,
            Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
            where TEntrypoint : IEntrypointWithSettings<TDerivedSettings>, new()
            where TDerivedSettings : TSettings, new()
            => this.InternalAddTerminalCommandWithSettings<CommandLineBuilder<TSettings>, TEntrypoint, TSettings, TDerivedSettings>(name, commandBuilder);

        public CommandLineBuilder<TSettings> ConfigureHelp(Action<HelpOptions> helpConfigure)
        {
            helpConfigure(this.helpOptions);
            return this;
        }

        public CommandLineBuilder<TSettings> Configure(Action<ParserOptions> optionsConfigure)
        {
            optionsConfigure(this.parserOptions);
            return this;
        }

        public IParser Build()
        {
            return new Parser(this.parserOptions, this.helpOptions, this.command);
        }
    }
}
