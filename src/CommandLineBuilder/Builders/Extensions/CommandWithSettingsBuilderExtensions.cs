namespace CommandLine
{
    internal static class CommandWithSettingsBuilderExtensions
    {
        public static T InternalAddOption<T, TSettings>(this T item, OptionDefinition<TSettings> optionDefinition)
            where T : ICommandWithSettingsBuilder<T, TSettings>, ICommandBuilder
            where TSettings : new()
        {
            var untypedOptionSettings = optionDefinition.ToUntyped();
            item.Command.AddSettingFunction(untypedOptionSettings);
            return item;
        }

        public static T InternalAddSwitch<T, TSettings>(this T item, SwitchDefinition<TSettings> switchDefinition)
            where T : ICommandWithSettingsBuilder<T, TSettings>, ICommandBuilder
            where TSettings : new()
        {
            var untypedSwitchDefinition = switchDefinition.ToUntyped();
            item.Command.AddSwitchFunction(untypedSwitchDefinition);
            return item;
        }

        public static T InternalAddSettingDefault<T, TSettings>(this T item, SettingDefaultDefinition<TSettings> settingDefaultDefinition)
            where T : ICommandWithSettingsBuilder<T, TSettings>, ICommandBuilder
            where TSettings : new()
        {
            var untypedSwitchDefinition = settingDefaultDefinition.ToUntyped();
            item.Command.AddSettingDefault(untypedSwitchDefinition);
            return item;
        }
    }
}
