using System;

namespace CommandLine
{
    public sealed class CommandLineBuilder : 
        INonTerminalCommandWithoutSettingsBuilder<CommandLineBuilder>,
        ICommandBuilder
    {
        Command ICommandBuilder.Command => this.command;

        private readonly Command command;
        private readonly HelpOptions helpOptions;
        private readonly ParserOptions parserOptions;

        public CommandLineBuilder(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new ArgumentNullException(nameof(applicationName));
            }

            this.command = Command.CreateRoot(applicationName);
            this.helpOptions = new HelpOptions();
            this.parserOptions = new ParserOptions();
        }

        public CommandLineBuilder AddNonTerminalCommand(string name)
        => this.AddNonTerminalCommand(name, _ => {});

        public CommandLineBuilder AddNonTerminalCommand(string name, Action<NonTerminalCommandBuilder> commandBuilder)
        => this.InternalAddNonTerminalCommand(name, commandBuilder);

        public CommandLineBuilder AddNonTerminalCommandWithSettings<TSettings>(string name)
            where TSettings : new()
        => this.AddNonTerminalCommandWithSettings<TSettings>(name, _ => {});

        public CommandLineBuilder AddNonTerminalCommandWithSettings<TSettings>(string name, Action<NonTerminalCommandBuilder<TSettings>> commandBuilder)
            where TSettings : new()
        => this.InternalAddNonTerminalCommandWithSettings(name, commandBuilder);
        
        public CommandLineBuilder AddTerminalCommand<TEntrypoint>(string name)
            where TEntrypoint : IEntrypoint, new()
        => this.AddTerminalCommand<TEntrypoint>(name, _ => {});

        public CommandLineBuilder AddTerminalCommand<TEntrypoint>(string name, Action<TerminalCommandBuilder<TEntrypoint>> commandBuilder)
            where TEntrypoint : IEntrypoint, new()
        => this.InternalAddTerminalCommand(name, commandBuilder);
        
        public CommandLineBuilder AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name)
            where TEntrypoint : IEntrypoint<TSettings>, new()
            where TSettings : new()
        => this.AddTerminalCommandWithSettings<TEntrypoint, TSettings>(name, _ => {});

        public CommandLineBuilder AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name, Action<TerminalCommandBuilder<TEntrypoint, TSettings>> commandBuilder) 
            where TEntrypoint : IEntrypoint<TSettings>, new()
            where TSettings : new()
        => this.InternalAddTerminalCommandWithSettings(name, commandBuilder);

        public CommandLineBuilder WithDefaultEntrypoint<TEntrypoint>()
            where TEntrypoint : IEntrypoint
        {
            this.command.UpdateEntrypointType(typeof(TEntrypoint));
            return this;
        }

        public CommandLineBuilder Configure(Action<ParserOptions> optionsConfigure)
        {
            if (optionsConfigure is null)
            {
                throw new ArgumentNullException(nameof(optionsConfigure));
            }

            optionsConfigure(this.parserOptions);
            return this;
        }

        public CommandLineBuilder ConfigureHelp(Action<HelpOptions> helpConfigure)
        {
            if (helpConfigure is null)
            {
                throw new ArgumentNullException(nameof(helpConfigure));
            }

            helpConfigure(this.helpOptions);
            return this;
        }

        public IParser Build()
        => new Parser(this.parserOptions, this.helpOptions, this.command);
    }
}
