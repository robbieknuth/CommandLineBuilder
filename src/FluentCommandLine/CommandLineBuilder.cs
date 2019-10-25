using System;

namespace FluentCommandLine
{
    public sealed class CommandLineBuilder : INonTerminalCommandWithoutSettingsBuilder<CommandLineBuilder>
    {
        Command ICommandBuilder.Command => this.command;

        private readonly Command command;
        private readonly HelpOptions helpOptions;

        public CommandLineBuilder(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new ArgumentNullException(nameof(applicationName));
            }

            this.command = Command.CreateRoot(applicationName);
            this.helpOptions = new HelpOptions();
        }

        public CommandLineBuilder AddNonTerminalCommand(string name, Action<NonTerminalCommandBuilder> commandBuilder)
            => this.InternalAddNonTerminalCommand(name, commandBuilder);

        public CommandLineBuilder AddNonTerminalCommandWithSettings<TSettings>(string name, Action<NonTerminalCommandWithSettingsBuilder<TSettings>> commandBuilder)
            where TSettings : new()
            => this.InternalAddNonTerminalCommandWithSettings(name, commandBuilder);

        public CommandLineBuilder AddTerminalCommand<TEntrypoint>(string name, Action<TerminalCommandBuilder<TEntrypoint>> commandBuilder)
            where TEntrypoint : IEntrypoint, new()
            => this.InternalAddTerminalCommand(name, commandBuilder);

        public CommandLineBuilder AddTerminalCommand<TEntrypoint>(string name)
            where TEntrypoint : IEntrypoint, new()
            => this.InternalAddTerminalCommand(name, (TerminalCommandBuilder<TEntrypoint> x) => { });

        public CommandLineBuilder AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>> commandBuilder) 
            where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
            where TSettings : new()
            => this.InternalAddTerminalCommandWithSettings(name, commandBuilder);

        public CommandLineBuilder AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name)
            where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
            where TSettings : new()
            => this.InternalAddTerminalCommandWithSettings(name, (TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> x) => { });

        public CommandLineBuilder ConfigureHelp(Action<HelpOptions> helpConfigure)
        {
            helpConfigure(this.helpOptions);
            return this;
        }

        public IParser Build()
        {
            return new Parser(this.helpOptions, this.command);
        }
    }
}
