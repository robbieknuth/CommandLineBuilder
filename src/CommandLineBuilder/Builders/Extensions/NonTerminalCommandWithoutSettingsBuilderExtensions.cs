using System;

namespace CommandLineBuilder
{
    internal static class NonTerminalCommandWithoutSettingsBuilderExtensions
    {
        public static T InternalAddTerminalCommand<T, TEntrypoint>(this T item, string name, Action<TerminalCommandBuilder<TEntrypoint>> commandBuilder)
            where T : INonTerminalCommandWithoutSettingsBuilder<T>
            where TEntrypoint : IEntrypoint, new()
        {
            var builder = new TerminalCommandBuilder<TEntrypoint>(item.Command, name);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }

        public static T InternalAddNonTerminalCommand<T>(this T item, string name, Action<NonTerminalCommandBuilder> commandBuilder)
            where T : INonTerminalCommandWithoutSettingsBuilder<T>
        {
            var builder = new NonTerminalCommandBuilder(item.Command, name);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }

        public static T InternalAddTerminalCommandWithSettings<T, TEntrypoint, TSettings>(this T item, string name, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>> commandBuilder)
            where T : INonTerminalCommandWithoutSettingsBuilder<T>
            where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
            where TSettings : new()
        {
            var builder = new TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>(
                item.Command,
                name);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }

        public static T InternalAddNonTerminalCommandWithSettings<T, TSettings>(this T item, string name, Action<NonTerminalCommandWithSettingsBuilder<TSettings>> commandBuilder)
            where T : INonTerminalCommandWithoutSettingsBuilder<T>
            where TSettings : new()
        {
            var builder = new NonTerminalCommandWithSettingsBuilder<TSettings>(
                item.Command,
                name);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }
    }
}
