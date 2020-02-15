namespace CommandLine
{
    public interface ICommandBuilder<T, TSettings>
        where T : ICommandBuilder<T, TSettings>
        where TSettings : new()
    {
        T AddOption(OptionDefinition<TSettings> optionDefinition);

        T AddSwitch(SwitchDefinition<TSettings> switchDefinition);

        T AddSettingDefault(SettingDefaultDefinition<TSettings> settingDefaultDefinition);
    }
}
