using System;

namespace CommandLine
{
    public interface INonTerminalCommandWithoutSettingsBuilder<T>
        where T : INonTerminalCommandWithoutSettingsBuilder<T>
    {
        T AddNonTerminalCommand(string name);
        T AddNonTerminalCommand(string name, Action<NonTerminalCommandBuilder> commandBuilder);
        T AddTerminalCommand<TEntrypoint>(string name)
            where TEntrypoint : IEntrypoint, new();

        T AddTerminalCommand<TEntrypoint>(string name, Action<TerminalCommandBuilder<TEntrypoint>> commandBuilder)
            where TEntrypoint : IEntrypoint, new();
        T AddNonTerminalCommandWithSettings<TSettings>(string name)
            where TSettings : new();
        T AddNonTerminalCommandWithSettings<TSettings>(string name, Action<NonTerminalCommandBuilder<TSettings>> commandBuilder)
            where TSettings : new();
        T AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name)
            where TEntrypoint : IEntrypoint<TSettings>, new()
            where TSettings : new();
        T AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name, Action<TerminalCommandBuilder<TEntrypoint, TSettings>> commandBuilder)
            where TEntrypoint : IEntrypoint<TSettings>, new()
            where TSettings : new();
    }
}
