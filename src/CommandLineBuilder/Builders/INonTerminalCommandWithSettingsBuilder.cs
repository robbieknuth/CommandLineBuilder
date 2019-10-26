using System;

namespace CommandLine
{
    internal interface INonTerminalCommandWithSettingsBuilder<T, TSettings> : ICommandWithSettingsBuilder<T, TSettings>
        where T : ICommandWithSettingsBuilder<T, TSettings>
        where TSettings : new()
    {
        T AddNonTerminalCommandWithSettings<TDerivedSettings>(string name, Action<NonTerminalCommandWithSettingsBuilder<TDerivedSettings>> commandBuilder)
            where TDerivedSettings : TSettings, new();

        T AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(string name, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
            where TEntrypoint : IEntrypointWithSettings<TDerivedSettings>, new()
            where TDerivedSettings : TSettings, new();
    }
}
