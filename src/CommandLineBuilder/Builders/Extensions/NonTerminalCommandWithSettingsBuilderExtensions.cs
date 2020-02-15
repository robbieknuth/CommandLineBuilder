using System;

namespace CommandLine
{
    internal static class NonTerminalCommandWithSettingsBuilderExtensions
    {
        public static T InternalAddTerminalCommandWithSettings<T, TEntrypoint, TBaseSettings, TDerivedSettings>(this T item, string name, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
            where T : INonTerminalCommandWithSettingsBuilder<T, TBaseSettings>, ICommandBuilder
            where TBaseSettings : new()
            where TEntrypoint : IEntrypointWithSettings<TDerivedSettings>, new()
            where TDerivedSettings : TBaseSettings, new()
        {
            var builder = new TerminalCommandWithSettingsBuilder<TEntrypoint, TDerivedSettings>(
                item.Command,
                name);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }

        public static T InternalAddNonTerminalCommandWithSettings<T, TBaseSettings, TDerivedSettings>(this T item, string name, Action<NonTerminalCommandWithSettingsBuilder<TDerivedSettings>> commandBuilder)
            where T : INonTerminalCommandWithSettingsBuilder<T, TBaseSettings>, ICommandBuilder
            where TBaseSettings : new()
            where TDerivedSettings : TBaseSettings, new()
        {
            var builder = new NonTerminalCommandWithSettingsBuilder<TDerivedSettings>(
                item.Command,
                name);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }
    }
}
