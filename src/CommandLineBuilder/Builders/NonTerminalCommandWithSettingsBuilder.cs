using System;
using System.Linq.Expressions;

namespace CommandLineBuilder
{
    public sealed class NonTerminalCommandWithSettingsBuilder<TSettings> : INonTerminalCommandWithSettingsBuilder<NonTerminalCommandWithSettingsBuilder<TSettings>, TSettings>
        where TSettings : new()
    {
        Command ICommandBuilder.Command => this.command;

        private readonly Command command;

        internal NonTerminalCommandWithSettingsBuilder(
            Command parent,
            string name)
        {
            this.command = Command.CreateChild(parent, name, null, typeof(TSettings));
        }

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddOption<TPropertyValue>(string longForm, Expression<Func<TSettings, TPropertyValue>> property, Conversion<TPropertyValue> converter)
            => this.InternalAddOption(longForm, property, converter);

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddSwitch(string longForm, Action<TSettings> applicator)
            => this.InternalAddSwitch(longForm, applicator);

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(string name, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
            where TEntrypoint : IEntrypointWithSettings<TDerivedSettings>, new()
            where TDerivedSettings : TSettings, new()
            => this.InternalAddTerminalCommandWithSettings<NonTerminalCommandWithSettingsBuilder<TSettings>, TEntrypoint, TSettings, TDerivedSettings>(name, commandBuilder);

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddNonTerminalCommandWithSettings<TDerivedSettings>(string name, Action<NonTerminalCommandWithSettingsBuilder<TDerivedSettings>> commandBuilder)
            where TDerivedSettings : TSettings, new()
            => this.InternalAddNonTerminalCommandWithSettings<NonTerminalCommandWithSettingsBuilder<TSettings>, TSettings, TDerivedSettings>(name, commandBuilder);

        internal Command Build(Action<NonTerminalCommandWithSettingsBuilder<TSettings>> builder)
        {
            builder(this);
            return this.command;
        }
    }
}
