using System;

namespace FluentCommandLine
{
    public sealed class CommandLineParseException : Exception
    {
        public CommandLineParseException(string message) : base(message) { }
    }
}
