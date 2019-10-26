using System;
using System.Linq.Expressions;

namespace CommandLine
{
    internal static class CommandWithSettingsBuilderExtensions
    {
        public static T InternalAddOption<T, TSettings, TValue>(this T item, string longForm, Expression<Func<TSettings, TValue>> property, Conversion<TValue> converter)
            where T : ICommandWithSettingsBuilder<T, TSettings>
            where TSettings : new()
        {
            var optionName = OptionName.FromLongForm(longForm);
            var applicator = Converter<TValue>.CreateOptionConverter(optionName, property, converter);
            item.Command.AddSettingFunction(optionName, applicator);
            return item;
        }

        public static T InternalAddSwitch<T, TSettings>(this T item, string longForm, Action<TSettings> applier)
            where T : ICommandWithSettingsBuilder<T, TSettings>
            where TSettings : new()
        {
            item.Command.AddSwitchFunction(OptionName.FromLongForm(longForm), (o) => applier((TSettings)o));
            return item;
        }
    }
}
