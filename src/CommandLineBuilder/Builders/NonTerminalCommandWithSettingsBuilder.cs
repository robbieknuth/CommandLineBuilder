using System;

namespace CommandLine
{
    public sealed class NonTerminalCommandWithSettingsBuilder<TSettings> :
        CommandWithSettingsBuilder<NonTerminalCommandWithSettingsBuilder<TSettings>, TSettings>,
        INonTerminalCommandWithSettingsBuilder<NonTerminalCommandWithSettingsBuilder<TSettings>, TSettings>, ICommandBuilder
        where TSettings : new()
    {
        internal override NonTerminalCommandWithSettingsBuilder<TSettings> Self => this;
        internal NonTerminalCommandWithSettingsBuilder(Command parent, string name)
            : base(Command.CreateChild(parent, name, null, typeof(TSettings)))
        { }
    
        public NonTerminalCommandWithSettingsBuilder<TSettings> AddTerminalCommandWithSettings<TEntrypoint, TDerivedSettings>(string name, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TDerivedSettings>> commandBuilder)
            where TEntrypoint : IEntrypointWithSettings<TDerivedSettings>, new()
            where TDerivedSettings : TSettings, new()
        => this.InternalAddTerminalCommandWithSettings<NonTerminalCommandWithSettingsBuilder<TSettings>, TEntrypoint, TSettings, TDerivedSettings>(name, commandBuilder);

        public NonTerminalCommandWithSettingsBuilder<TSettings> AddNonTerminalCommandWithSettings<TDerivedSettings>(string name, Action<NonTerminalCommandWithSettingsBuilder<TDerivedSettings>> commandBuilder)
            where TDerivedSettings : TSettings, new()
        => this.InternalAddNonTerminalCommandWithSettings<NonTerminalCommandWithSettingsBuilder<TSettings>, TSettings, TDerivedSettings>(name, commandBuilder);

        internal Command Build(Action<NonTerminalCommandWithSettingsBuilder<TSettings>> builder)
        {
            builder(this);
            return this.command;
        }
    }
}
