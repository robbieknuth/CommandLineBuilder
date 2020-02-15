using System;
using System.Linq.Expressions;

namespace CommandLine
{
    internal interface ICommandWithSettingsBuilder<T, TSettings> : ICommandBuilder
        where T : ICommandWithSettingsBuilder<T, TSettings>
        where TSettings : new()
    {
        T AddOption<TPropertyValue>(string longForm, Expression<Func<TSettings, TPropertyValue>> property, Conversion<TPropertyValue> converter);

        T AddOption<TPropertyValue>(string longForm, string shortForm, Expression<Func<TSettings, TPropertyValue>> property, Conversion<TPropertyValue> converter);

        T AddOption<TPropertyValue>(OptionDefinition<TSettings> optionDefinition);

        T AddSwitch(string longForm, Action<TSettings> applier);

        T AddSwitch(string longForm, string shortForm, Action<TSettings> applier);

        T AddSwitch(SwitchDefinition<TSettings> switchDefinition);

        T AddSettingDefault(Action<TSettings> applicator);

        T AddSettingDefault(SettingDefaultDefinition<TSettings> settingDefaultDefinition);
    }
}
