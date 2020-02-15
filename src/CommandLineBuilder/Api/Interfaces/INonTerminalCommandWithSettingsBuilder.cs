using System;

namespace CommandLine
{
    public interface INonTerminalCommandWithSettingsBuilder<T, TSettings> : ICommandBuilder<T, TSettings>
        where T : ICommandBuilder<T, TSettings>
        where TSettings : new()
    {
        T AddNonTerminalCommandWithSettings<TDerivedSettings>(string name, Action<NonTerminalCommandBuilder<TDerivedSettings>> commandBuilder)
            where TDerivedSettings : TSettings, new();

        T AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(string name, Action<TerminalCommandBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
            where TEntrypoint : IEntrypoint<TDerivedSettings>, new()
            where TDerivedSettings : TSettings, new();
    }
}
