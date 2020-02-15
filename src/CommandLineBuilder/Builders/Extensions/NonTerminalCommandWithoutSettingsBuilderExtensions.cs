using System;

namespace CommandLine
{
    internal static class NonTerminalCommandWithoutSettingsBuilderExtensions
    {
        public static T InternalAddTerminalCommand<T, TEntrypoint>(this T item, string commandName, Action<TerminalCommandBuilder<TEntrypoint>> commandBuilder)
            where T : INonTerminalCommandWithoutSettingsBuilder<T>, ICommandBuilder
            where TEntrypoint : IEntrypoint, new()
        {
            ValidateCommandName(commandName);
            if (commandBuilder is null)
            {
                throw new ArgumentNullException(nameof(commandBuilder));
            }
            
            var builder = new TerminalCommandBuilder<TEntrypoint>(item.Command, commandName);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }

        public static T InternalAddNonTerminalCommand<T>(this T item, string commandName, Action<NonTerminalCommandBuilder> commandBuilder)
            where T : INonTerminalCommandWithoutSettingsBuilder<T>, ICommandBuilder
        {
            ValidateCommandName(commandName);
            if (commandBuilder is null)
            {
                throw new ArgumentNullException(nameof(commandBuilder));
            }

            var builder = new NonTerminalCommandBuilder(item.Command, commandName);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }

        public static T InternalAddTerminalCommandWithSettings<T, TEntrypoint, TSettings>(this T item, string commandName, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>> commandBuilder)
            where T : INonTerminalCommandWithoutSettingsBuilder<T>, ICommandBuilder
            where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
            where TSettings : new()
        {
            ValidateCommandName(commandName);
            if (commandBuilder is null)
            {
                throw new ArgumentNullException(nameof(commandBuilder));
            }

            var builder = new TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>(item.Command, commandName);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }

        public static T InternalAddNonTerminalCommandWithSettings<T, TSettings>(this T item, string commandName, Action<NonTerminalCommandWithSettingsBuilder<TSettings>> commandBuilder)
            where T : INonTerminalCommandWithoutSettingsBuilder<T>, ICommandBuilder
            where TSettings : new()
        {
            ValidateCommandName(commandName);
            if (commandBuilder is null)
            {
                throw new ArgumentNullException(nameof(commandBuilder));
            }

            var builder = new NonTerminalCommandWithSettingsBuilder<TSettings>(item.Command, commandName);
            item.Command.AddSubCommand(builder.Build(commandBuilder));
            return item;
        }

        private static void ValidateCommandName(string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentException("Command name cannot be null or empty.", nameof(commandName));
            }

            foreach (var c in commandName)
            {
                if (!char.IsLetter(c))
                {
                    throw new ArgumentException("Command name must be alpha numeric.", nameof(commandName));
                }
            }
        }
    }
}
