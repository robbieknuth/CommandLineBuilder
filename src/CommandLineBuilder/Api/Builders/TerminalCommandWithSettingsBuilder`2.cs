using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class TerminalCommandBuilder<TEntrypoint, TSettings> :
        CommandWithSettingsBuilder<TerminalCommandBuilder<TEntrypoint, TSettings>, TSettings>,
        ICommandBuilder<TerminalCommandBuilder<TEntrypoint, TSettings>, TSettings>, ICommandBuilder
        where TEntrypoint : IEntrypoint<TSettings>, new()
        where TSettings : new()
    {
        internal TerminalCommandBuilder(Command parent, string name)
            : base (Command.CreateChild(parent, name, typeof(TEntrypoint), typeof(TSettings))) { }

        internal override TerminalCommandBuilder<TEntrypoint, TSettings> Self => this;

        public TerminalCommandBuilder<TEntrypoint, TSettings> AddPositional<T>(
            string name,
            bool required,
            Expression<Func<TSettings, T>> property,
            Conversion<T> converter)
            => this.AddPositional(PositionalDefinition<TSettings>.Create(name, required, property, converter));

        public TerminalCommandBuilder<TEntrypoint, TSettings> AddPositional(PositionalDefinition<TSettings> positionalDefinition)
        {
            this.command.AddPositionalFunction(positionalDefinition.ToUntyped());
            return this;
        }

        internal Command Build(Action<TerminalCommandBuilder<TEntrypoint, TSettings>> builderAction)
        {
            builderAction(this);
            return this.command;
        }
    }
}
