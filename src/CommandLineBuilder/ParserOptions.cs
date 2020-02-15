using System;

namespace CommandLine
{
    public sealed class ParserOptions
    {
        public Action<string>? OutputHandler { get; set; }
        public int ErrorCode { get; set; }

        internal ParserOptions()
        => this.OutputHandler = Console.Write;
    }
}
