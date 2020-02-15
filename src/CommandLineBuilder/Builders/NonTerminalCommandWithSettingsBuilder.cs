using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class NonTerminalCommandWithSettingsBuilder<TSettings> 
        : INonTerminalCommandWithSettingsBuilder<NonTerminalCommandWithSettingsBuilder<TSettings>, TSettings>, ICommandBuilder
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
            => this.AddOption<TPropertyValue>(OptionDefinition<TSettings>.Create(longForm, property, converter));

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddOption<TPropertyValue>(string longForm, string shortForm, Expression<Func<TSettings, TPropertyValue>> property, Conversion<TPropertyValue> converter)
            => this.AddOption<TPropertyValue>(OptionDefinition<TSettings>.Create(longForm, shortForm, property, converter));

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddOption<TPropertyValue>(OptionDefinition<TSettings> optionDefinition)
            => this.InternalAddOption(optionDefinition);

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddSwitch(string longForm, Action<TSettings> applicator)
            => this.AddSwitch(SwitchDefinition<TSettings>.Create(longForm, applicator));

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddSwitch(string longForm, string shortForm, Action<TSettings> applicator)
            => this.AddSwitch(SwitchDefinition<TSettings>.Create(longForm, shortForm, applicator));

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddSwitch(SwitchDefinition<TSettings> switchDefinition)
            => this.InternalAddSwitch(switchDefinition);

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddSettingDefault(Action<TSettings> applicator)
            => this.AddSettingDefault(SettingDefaultDefinition<TSettings>.Create(applicator));

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddSettingDefault(SettingDefaultDefinition<TSettings> settingDefaultDefinition)
            => this.InternalAddSettingDefault(settingDefaultDefinition);

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
