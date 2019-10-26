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

        public CommandLineBuilder<TSettings> AddOption<TPropertyValue>(string longForm, Expression<Func<TSettings, TPropertyValue>> property, Conversion<TPropertyValue> converter)
            => this.InternalAddOption(longForm, property, converter);

        public CommandLineBuilder<TSettings> AddSwitch(string longForm, Action<TSettings> applier)
            => this.InternalAddSwitch(longForm, applier);

        public CommandLineBuilder<TSettings> AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(string name, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
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
