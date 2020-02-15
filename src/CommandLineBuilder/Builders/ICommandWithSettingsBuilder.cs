namespace CommandLine
{
    public interface ICommandWithSettingsBuilder<T, TSettings>
        where T : ICommandWithSettingsBuilder<T, TSettings>
        where TSettings : new()
    {
        T AddOption(OptionDefinition<TSettings> optionDefinition);

        T AddSwitch(SwitchDefinition<TSettings> switchDefinition);

        T AddSettingDefault(SettingDefaultDefinition<TSettings> settingDefaultDefinition);
    }
}
