using System;

namespace FluentCommandLine
{
    public sealed class CommandStructureException : Exception
    {
        public CommandStructureException(string message) : base(message) { }
    }
}
