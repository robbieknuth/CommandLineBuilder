using System;

namespace CommandLine
{
    public sealed class NonTerminalCommandBuilder<TSettings> :
        CommandWithSettingsBuilder<NonTerminalCommandBuilder<TSettings>, TSettings>,
        INonTerminalCommandWithSettingsBuilder<NonTerminalCommandBuilder<TSettings>, TSettings>, ICommandBuilder
        where TSettings : new()
    {
        internal override NonTerminalCommandBuilder<TSettings> Self => this;
        internal NonTerminalCommandBuilder(Command parent, string name)
            : base(Command.CreateChild(parent, name, null, typeof(TSettings)))
        { }
    
        public NonTerminalCommandBuilder<TSettings> AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(string name, Action<TerminalCommandBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
            where TEntrypoint : IEntrypoint<TDerivedSettings>, new()
            where TDerivedSettings : TSettings, new()
        => this.InternalAddTerminalCommandWithSettings<NonTerminalCommandBuilder<TSettings>, TEntrypoint, TSettings, TDerivedSettings>(name, commandBuilder);

        public NonTerminalCommandBuilder<TSettings> AddNonTerminalCommandWithSettings<TDerivedSettings>(string name, Action<NonTerminalCommandBuilder<TDerivedSettings>> commandBuilder)
            where TDerivedSettings : TSettings, new()
        => this.InternalAddNonTerminalCommandWithSettings<NonTerminalCommandBuilder<TSettings>, TSettings, TDerivedSettings>(name, commandBuilder);

        internal Command Build(Action<NonTerminalCommandBuilder<TSettings>> builder)
        {
            builder(this);
            return this.command;
        }
    }
}
