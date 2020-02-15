namespace CommandLine
{
    public abstract class CommandWithSettingsBuilder<T, TSettings> : ICommandWithSettingsBuilder<T, TSettings>, ICommandBuilder
        where T : ICommandWithSettingsBuilder<T, TSettings>
        where TSettings : new()
    {
        Command ICommandBuilder.Command => this.command;

        internal readonly Command command;

        internal abstract T Self{ get; }

        internal CommandWithSettingsBuilder(Command command)
        {
            this.command = command;
        }

        public T AddOption(OptionDefinition<TSettings> optionDefinition)
        {
            var untypedOptionSettings = optionDefinition.ToUntyped();
            this.command.AddSettingFunction(untypedOptionSettings);
            return this.Self;
        }

        public T AddSwitch(SwitchDefinition<TSettings> switchDefinition)
        {
            var untypedSwitchDefinition = switchDefinition.ToUntyped();
            this.command.AddSwitchFunction(untypedSwitchDefinition);
            return this.Self;
        }

        public T AddSettingDefault(SettingDefaultDefinition<TSettings> settingDefaultDefinition)
        {
            var untypedSwitchDefinition = settingDefaultDefinition.ToUntyped();
            this.command.AddSettingDefault(untypedSwitchDefinition);
            return this.Self;
        }
    }
}