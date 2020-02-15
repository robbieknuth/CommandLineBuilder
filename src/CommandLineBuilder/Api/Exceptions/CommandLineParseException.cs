using System;

namespace CommandLine
{
    public sealed class CommandLineParseException : Exception
    {
        public CommandLineParseException(string message) : base(message) { }
    }
}
