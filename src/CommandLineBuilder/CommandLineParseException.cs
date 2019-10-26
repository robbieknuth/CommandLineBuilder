using System;

namespace CommandLineBuilder
{
    public sealed class CommandLineParseException : Exception
    {
        public CommandLineParseException(string message) : base(message) { }
    }
}
