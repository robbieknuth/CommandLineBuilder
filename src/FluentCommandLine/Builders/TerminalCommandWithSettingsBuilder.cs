using System;
using System.Linq.Expressions;

namespace FluentCommandLine
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
            string longForm, Expression<Func<TSettings, T>> property, Conversion<T> converter)
            => this.InternalAddOption(longForm, property, converter);

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddSwitch(string longForm, Action<TSettings> applier)
            => this.InternalAddSwitch(longForm, applier);

        public TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> AddPositional<T>(string name, Expression<Func<TSettings, T>> property, Conversion<T> converter)
        {
            this.command.AddPositionalFunction(name, Converter<T>.CreatePositionalConverter(name, property, converter));
            return this;
        }

        internal Command Build(Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>> builderAction)
        {
            builderAction(this);
            return this.command;
        }
    }
}
