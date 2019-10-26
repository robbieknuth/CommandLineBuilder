using System;

namespace CommandLineBuilder
{
    internal sealed class Parser : IParser
    {
        private readonly Command root;
        private readonly HelpOptions helpOptions;

        public Parser(HelpOptions helpOptions, Command root)
        {
            this.helpOptions = helpOptions;
            this.root = root;
        }

        public IEntrypoint Parse(string[] args)
        {
            var parseContext = new ParseContext(this.helpOptions);
            if (this.root.TryConsume(parseContext, new ReadOnlySpan<string>(args), out var entrypoint) || entrypoint != null)
            {
                return entrypoint!;
            }

            throw new Exception("Unable to find entrypoint.");
        }
    }
}
