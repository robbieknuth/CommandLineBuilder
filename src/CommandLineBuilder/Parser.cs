using System;

namespace CommandLine
{
    internal sealed class Parser : IParser
    {
        private readonly Command root;
        private readonly ParserOptions parserOptions;
        private readonly HelpOptions helpOptions;

        public Parser(ParserOptions parserOptions, HelpOptions helpOptions, Command root)
        {
            this.parserOptions = parserOptions;
            this.helpOptions = helpOptions;
            this.root = root;
        }

        public IEntrypoint Parse(string[] args)
        {
            var parseContext = new ParseContext(this.parserOptions, this.helpOptions);
            if (this.root.TryConsume(parseContext, new ReadOnlySpan<string>(args), out var entrypoint) || entrypoint != null)
            {
                return entrypoint!;
            }

            throw new Exception("Unable to find entrypoint.");
        }
    }
}
