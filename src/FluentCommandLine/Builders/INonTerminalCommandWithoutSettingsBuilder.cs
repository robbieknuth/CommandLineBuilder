using System;

namespace FluentCommandLine
{
    internal interface INonTerminalCommandWithoutSettingsBuilder<T> : ICommandBuilder
        where T : INonTerminalCommandWithoutSettingsBuilder<T>
    {
        T AddNonTerminalCommand(string name, Action<NonTerminalCommandBuilder> commandBuilder);

        T AddTerminalCommand<TEntrypoint>(string name, Action<TerminalCommandBuilder<TEntrypoint>> commandBuilder)
            where TEntrypoint : IEntrypoint, new();

        T AddTerminalCommand<TEntrypoint>(string name)
            where TEntrypoint : IEntrypoint, new();

        T AddNonTerminalCommandWithSettings<TSettings>(string name, Action<NonTerminalCommandWithSettingsBuilder<TSettings>> commandBuilder)
            where TSettings : new();

        T AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>> commandBuilder)
            where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
            where TSettings : new();

        T AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name)
            where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
            where TSettings : new();
    }
}
