using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> :
        CommandWithSettingsBuilder<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>, TSettings>,
        ICommandWithSettingsBuilder<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>, TSettings>, ICommandBuilder
        where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
        where TSettings : new()
    {
        internal TerminalCommandWithSettingsBuilder(Command parent, string name)
            : base (Command.CreateChild(parent, name, typeof(TEntrypoint), typeof(TSettings))) { }

        internal override TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> Self => this;

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
