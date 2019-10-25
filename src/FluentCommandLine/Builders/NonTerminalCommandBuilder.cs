using System;

namespace FluentCommandLine
{
    public sealed class NonTerminalCommandBuilder : INonTerminalCommandWithoutSettingsBuilder<NonTerminalCommandBuilder>
    {
        Command ICommandBuilder.Command => this.command;

        private readonly Command command;

        internal NonTerminalCommandBuilder(Command parent, string name)
        {
            this.command = Command.CreateChild(parent, name, null, null);
        }

        public NonTerminalCommandBuilder AddNonTerminalCommand(string name, Action<NonTerminalCommandBuilder> commandBuilder)
            => this.InternalAddNonTerminalCommand(name, commandBuilder);

        public NonTerminalCommandBuilder AddNonTerminalCommandWithSettings<TSettings>(string name, Action<NonTerminalCommandWithSettingsBuilder<TSettings>> commandBuilder)
            where TSettings : new()
            => this.InternalAddNonTerminalCommandWithSettings(name, commandBuilder);

        public NonTerminalCommandBuilder AddTerminalCommand<TEntrypoint>(string name, Action<TerminalCommandBuilder<TEntrypoint>> commandBuilder)
            where TEntrypoint : IEntrypoint, new()
            => this.InternalAddTerminalCommand(name, commandBuilder);

        public NonTerminalCommandBuilder AddTerminalCommand<TEntrypoint>(string name)
            where TEntrypoint : IEntrypoint, new()
            => this.InternalAddTerminalCommand(name, (TerminalCommandBuilder<TEntrypoint> x) => { });

        public NonTerminalCommandBuilder AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name, Action<TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings>> commandBuilder)
            where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
            where TSettings : new()
            => this.InternalAddTerminalCommandWithSettings(name, commandBuilder);

        public NonTerminalCommandBuilder AddTerminalCommandWithSettings<TEntrypoint, TSettings>(string name)
            where TEntrypoint : IEntrypointWithSettings<TSettings>, new()
            where TSettings : new()
            => this.InternalAddTerminalCommandWithSettings(name, (TerminalCommandWithSettingsBuilder<TEntrypoint, TSettings> x) => { });

        internal Command Build(Action<NonTerminalCommandBuilder> builderAction)
        {
            builderAction(this);
            return this.command;
        }
    }
}
