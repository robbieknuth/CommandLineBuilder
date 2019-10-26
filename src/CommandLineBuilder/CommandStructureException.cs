using System;

namespace CommandLineBuilder
{
    public sealed class CommandStructureException : Exception
    {
        public CommandStructureException(string message) : base(message) { }
    }
}
