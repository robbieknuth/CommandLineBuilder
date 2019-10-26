using System.Threading;
using System.Threading.Tasks;

namespace CommandLine
{
    internal sealed class HelpEntrypoint : IEntrypoint
    {
        private readonly string error;
        private readonly string? errorDetail;
        private readonly HelpOptions helpOptions;
        private readonly ParserOptions parserOptions;
        private readonly Command parsedCommand;

        public HelpEntrypoint(
            ParseContext parseContext,
            Command parsedCommand,
            string error,
            string? detail = null)
        {
            this.helpOptions = parseContext.helpOptions;
            this.parserOptions = parseContext.parserOptions;
            this.parsedCommand = parsedCommand;
            this.error = error;
            this.errorDetail = detail;
        }

        public Task<int> RunAsync(CancellationToken cancellationToken)
        {
            if (this.helpOptions.ShouldFailWithException)
            {
                throw new CommandLineParseException(this.error);
            }
            else
            {
                this.parserOptions.OutputHandler?.Invoke(this.error);
                if (this.errorDetail != null)
                {
                    this.parserOptions.OutputHandler?.Invoke($" -> { this.errorDetail }");
                }

                this.parserOptions.OutputHandler?.Invoke("");

                this.parserOptions.OutputHandler?.Invoke(this.parsedCommand.ToString());
                return Task.FromResult(this.helpOptions.ExitCode);
            }
        }
    }
}
