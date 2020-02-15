using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public static class CommandWithSettingsBuilderExtensions
    {
        public static CommandLineBuilder<TSettings> AddSwitch<TSettings>(
            this CommandLineBuilder<TSettings> commandLineBuilder,
            string longForm,
            Action<TSettings> applicator)
            where TSettings : new()
        => commandLineBuilder.AddSwitch(longForm, null, applicator);

        public static CommandLineBuilder<TSettings> AddSwitch<TSettings>(
            this CommandLineBuilder<TSettings> commandLineBuilder,
            string longForm,
            string? shortForm,
            Action<TSettings> applicator)
            where TSettings : new()
        => commandLineBuilder.AddSwitch(SwitchDefinition<TSettings>.Create(longForm, shortForm, applicator));
        
        public static CommandLineBuilder<TSettings> AddSettingDefault<TSettings>(
            this CommandLineBuilder<TSettings> commandLineBuilder,
            Action<TSettings> applicator)
            where TSettings : new()
        => commandLineBuilder.AddSettingDefault(SettingDefaultDefinition<TSettings>.Create(applicator));

        public static CommandLineBuilder<TSettings> AddOption<TSettings, TPropertyValue>(
            this CommandLineBuilder<TSettings> commandLineBuilder,
            string longForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
            where TSettings : new()
        => commandLineBuilder.AddOption(longForm, null, property, conversion);

        public static CommandLineBuilder<TSettings> AddOption<TSettings, TPropertyValue>(
            this CommandLineBuilder<TSettings> commandLineBuilder,
            string longForm,
            string? shortForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
            where TSettings : new()
        => commandLineBuilder.AddOption(OptionDefinition<TSettings>.Create(longForm, shortForm, property, conversion));

        public static TerminalCommandBuilder<TEntrypoint, TSettings> AddSwitch<TEntrypoint, TSettings>(
            this TerminalCommandBuilder<TEntrypoint, TSettings> commandBuilder,
            string longForm,
            Action<TSettings> applicator)
            where TSettings : new()
            where TEntrypoint : IEntrypoint<TSettings>, new()
        => commandBuilder.AddSwitch(longForm, null, applicator);

        public static TerminalCommandBuilder<TEntrypoint, TSettings> AddSwitch<TEntrypoint, TSettings>(
            this TerminalCommandBuilder<TEntrypoint, TSettings> commandBuilder,
            string longForm,
            string? shortForm,
            Action<TSettings> applicator)
            where TSettings : new()
            where TEntrypoint : IEntrypoint<TSettings>, new()
        => commandBuilder.AddSwitch(SwitchDefinition<TSettings>.Create(longForm, shortForm, applicator));

        public static TerminalCommandBuilder<TEntrypoint, TSettings> AddOption<TEntrypoint, TSettings, TPropertyValue>(
            this TerminalCommandBuilder<TEntrypoint, TSettings> commandBuilder,
            string longForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
            where TEntrypoint : IEntrypoint<TSettings>, new()
            where TSettings : new()
        => commandBuilder.AddOption(longForm, null, property, conversion);

        public static TerminalCommandBuilder<TEntrypoint, TSettings> AddOption<TEntrypoint, TSettings, TPropertyValue>(
            this TerminalCommandBuilder<TEntrypoint, TSettings> commandBuilder,
            string longForm,
            string? shortForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
            where TEntrypoint : IEntrypoint<TSettings>, new()
            where TSettings : new()
        => commandBuilder.AddOption(OptionDefinition<TSettings>.Create(longForm, shortForm, property, conversion));

        public static TerminalCommandBuilder<TEntrypoint, TSettings> AddSettingDefault<TEntrypoint, TSettings>(
            this TerminalCommandBuilder<TEntrypoint, TSettings> commandBuilder,
            Action<TSettings> applicator)
            where TEntrypoint : IEntrypoint<TSettings>, new()
            where TSettings : new()
        => commandBuilder.AddSettingDefault(SettingDefaultDefinition<TSettings>.Create(applicator));

        public static NonTerminalCommandBuilder<TSettings> AddSwitch<TSettings>(
            this NonTerminalCommandBuilder<TSettings> commandBuilder,
            string longForm,
            Action<TSettings> applicator)
            where TSettings : new()
        => commandBuilder.AddSwitch(longForm, null, applicator);

        public static NonTerminalCommandBuilder<TSettings> AddSwitch<TSettings>(
            this NonTerminalCommandBuilder<TSettings> commandBuilder,
            string longForm,
            string? shortForm,
            Action<TSettings> applicator)
            where TSettings : new()
        => commandBuilder.AddSwitch(SwitchDefinition<TSettings>.Create(longForm, shortForm, applicator));

        public static NonTerminalCommandBuilder<TSettings> AddOption<TSettings, TPropertyValue>(
            this NonTerminalCommandBuilder<TSettings> commandBuilder,
            string longForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
            where TSettings : new()
        => commandBuilder.AddOption(longForm, null, property, conversion);

        public static NonTerminalCommandBuilder<TSettings> AddOption<TSettings, TPropertyValue>(
            this NonTerminalCommandBuilder<TSettings> commandBuilder,
            string longForm,
            string? shortForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
            where TSettings : new()
        => commandBuilder.AddOption(OptionDefinition<TSettings>.Create(longForm, shortForm, property, conversion));

        public static NonTerminalCommandBuilder<TSettings> AddSettingDefault<TSettings>(
            this NonTerminalCommandBuilder<TSettings> commandBuilder,
            Action<TSettings> applicator)
            where TSettings : new()
        => commandBuilder.AddSettingDefault(SettingDefaultDefinition<TSettings>.Create(applicator));
    }
}
