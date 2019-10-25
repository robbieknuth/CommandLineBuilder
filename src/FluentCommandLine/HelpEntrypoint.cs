using System.Threading;
using System.Threading.Tasks;

namespace FluentCommandLine
{
    internal sealed class HelpEntrypoint : IEntrypoint
    {
        private readonly string error;
        private readonly string? errorDetail;
        private readonly HelpOptions helpOptions;
        private readonly Command parsedCommand;

        public HelpEntrypoint(
            HelpOptions helpOptions,
            Command parsedCommand,
            string error,
            string? detail = null)
        {
            this.helpOptions = helpOptions;
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
                this.helpOptions.OutputHandler!(this.error);
                if (this.errorDetail != null)
                {
                    this.helpOptions.OutputHandler($" -> { this.errorDetail }");
                }

                this.helpOptions.OutputHandler("");

                this.helpOptions.OutputHandler(this.parsedCommand.ToString());
                return Task.FromResult(this.helpOptions.ExitCode);
            }
        }
    }
}
