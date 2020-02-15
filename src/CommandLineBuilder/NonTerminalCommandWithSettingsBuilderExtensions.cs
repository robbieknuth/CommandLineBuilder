using System;

namespace CommandLine
{
    internal static class NonTerminalCommandWithSettingsBuilderExtensions
    {
        public static T InternalAddTerminalCommandWithSettings<T, TEntrypoint, TBaseSettings, TDerivedSettings>(this T item, string name, Action<TerminalCommandBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
            where T : INonTerminalCommandWithSettingsBuilder<T, TBaseSettings>, ICommandBuilder
            where TBaseSettings : new()
            where TEntrypoint : IEntrypoint<TDerivedSettings>, new()
            where TDerivedSettings : TBaseSettings, new()
        {
            var builder = new TerminalCommandBuilder<TEntrypoint, TDerivedSettings>(
                item.Command,
                name);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }

        public static T InternalAddNonTerminalCommandWithSettings<T, TBaseSettings, TDerivedSettings>(this T item, string name, Action<NonTerminalCommandBuilder<TDerivedSettings>> commandBuilder)
            where T : INonTerminalCommandWithSettingsBuilder<T, TBaseSettings>, ICommandBuilder
            where TBaseSettings : new()
            where TDerivedSettings : TBaseSettings, new()
        {
            var builder = new NonTerminalCommandBuilder<TDerivedSettings>(
                item.Command,
                name);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }
    }
}
