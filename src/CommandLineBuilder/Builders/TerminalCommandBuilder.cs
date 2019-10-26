using System;

namespace CommandLineBuilder
{
    public sealed class TerminalCommandBuilder<TEntrypoint> : ICommandBuilder
    {
        Command ICommandBuilder.Command => this.command;

        private readonly Command command;

        internal TerminalCommandBuilder(
            Command parent,
            string name)
        {
            this.command = Command.CreateChild(parent, name, typeof(TEntrypoint), null);
        }

        internal Command Build(Action<TerminalCommandBuilder<TEntrypoint>> builderAction)
        {
            builderAction(this);
            return this.command;
        }
    }
}
