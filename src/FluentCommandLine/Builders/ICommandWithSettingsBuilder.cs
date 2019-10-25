using System;
using System.Linq.Expressions;

namespace FluentCommandLine
{
    internal interface ICommandWithSettingsBuilder<T, TSettings> : ICommandBuilder
        where T : ICommandWithSettingsBuilder<T, TSettings>
        where TSettings : new()
    {
        T AddOption<TPropertyValue>(string longForm, Expression<Func<TSettings, TPropertyValue>> property, Conversion<TPropertyValue> converter);

        T AddSwitch(string longForm, Action<TSettings> applier);
    }
}
