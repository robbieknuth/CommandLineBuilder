using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> 
        : ICommandWithSettingsBuilder<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>, TSettings>
        where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
        where TSettings : new()
    {
        Command ICommandBuilder.Command => this.command;

        private readonly Command command;

        internal TerminalCommandWithSettingsBuilder(
            Command parent,
            string name)
        {
            this.command = Command.CreateChild(parent, name, typeof(TEntrypoint), typeof(TSettings));
        }

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddOption<T>(
            string longForm,
            Expression<Func<TSettings, T>> property,
            Conversion<T> converter) 
            => this.AddOption(OptionDefinition<TSettings>.Create(longForm, property, converter));

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddOption<T>(
            string longForm,
            string shortForm,
            Expression<Func<TSettings, T>> property,
            Conversion<T> converter)
            => this.AddOption(OptionDefinition<TSettings>.Create(longForm, shortForm, property, converter));

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddOption(OptionDefinition<TSettings> optionDefinition)
            => this.InternalAddOption<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>, TSettings>(optionDefinition);

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddSwitch(
            string longForm,
            Action<TSettings> applier)
            => this.AddSwitch(SwitchDefinition<TSettings>.Create(longForm, applier));

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddSwitch(
            string longForm,
            string shortForm,
            Action<TSettings> applier)
            => this.AddSwitch(SwitchDefinition<TSettings>.Create(longForm, shortForm, applier));

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddSwitch(SwitchDefinition<TSettings> switchDefinition)
            => this.InternalAddSwitch<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>, TSettings>(switchDefinition);

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddPositional<T>(
            string name,
            bool required,
            Expression<Func<TSettings, T>> property,
            Conversion<T> converter)
            => this.AddPositional(PositionalDefinition<TSettings>.Create(name, required, property, converter));

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddPositional(PositionalDefinition<TSettings> positionalDefinition)
        {
            this.command.AddPositionalFunction(positionalDefinition.ToUntyped());
            return this;
        }

        internal Command Build(Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>> builderAction)
        {
            builderAction(this);
            return this.command;
        }
    }
}
