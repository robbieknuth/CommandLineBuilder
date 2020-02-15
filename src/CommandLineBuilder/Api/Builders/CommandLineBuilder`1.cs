using System;

namespace CommandLine
{
    public sealed class CommandLineBuilder<TSettings> : 
        CommandWithSettingsBuilder<CommandLineBuilder<TSettings>, TSettings>,
        INonTerminalCommandWithSettingsBuilder<CommandLineBuilder<TSettings>, TSettings>, ICommandBuilder
        where TSettings : new()
    {
        private readonly HelpOptions helpOptions;
        private readonly ParserOptions parserOptions;
        internal override CommandLineBuilder<TSettings> Self => this;

        public CommandLineBuilder(string applicationName)
            : base(Command.CreateRoot<TSettings>(applicationName))
        {
            this.helpOptions = new HelpOptions();
            this.parserOptions = new ParserOptions();
        }

        public CommandLineBuilder<TSettings> AddNonTerminalCommandWithSettings<TDerivedSettings>(string name)
            where TDerivedSettings : TSettings, new()
        => this.AddNonTerminalCommandWithSettings<TDerivedSettings>(name, x => {});

        public CommandLineBuilder<TSettings> AddNonTerminalCommandWithSettings<TDerivedSettings>(string name, Action<NonTerminalCommandBuilder<TDerivedSettings>> commandBuilder)
            where TDerivedSettings : TSettings, new()
        => this.InternalAddNonTerminalCommandWithSettings<CommandLineBuilder<TSettings>, TSettings, TDerivedSettings>(name, commandBuilder);

        public CommandLineBuilder<TSettings> AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(
            string name,
            Action<TerminalCommandBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
            where TEntrypoint : IEntrypoint<TDerivedSettings>, new()
            where TDerivedSettings : TSettings, new()
        => this.InternalAddTerminalCommandWithSettings<CommandLineBuilder<TSettings>, TEntrypoint, TSettings, TDerivedSettings>(name, commandBuilder);

        public CommandLineBuilder<TSettings> AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(string name)
            where TEntrypoint : IEntrypoint<TDerivedSettings>, new()
            where TDerivedSettings : TSettings, new()
        => this.AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(name, x => {});

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

        public CommandLineBuilder<TSettings> WithDefaultEntrypoint<TEntrypoint>()
            where TEntrypoint : IEntrypoint<TSettings>
        {
            this.command.UpdateEntrypointType(typeof(TEntrypoint));
            return this;
        }

        public IParser Build()
        => new Parser(this.parserOptions, this.helpOptions, this.command);
    }
}
