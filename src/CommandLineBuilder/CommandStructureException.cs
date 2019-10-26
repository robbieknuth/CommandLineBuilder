using System;

namespace CommandLine
{
    public sealed class CommandStructureException : Exception
    {
        public CommandStructureException(string message) : base(message) { }
    }
}
