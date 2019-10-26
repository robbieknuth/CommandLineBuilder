using System.Threading;
using System.Threading.Tasks;

namespace CommandLine
{
    internal sealed class ErrorEntrypoint : IEntrypoint
    {
        private readonly ParseContext parseContext;
        private readonly string error;

        public ErrorEntrypoint(ParseContext parseContext, string error)
        {
            this.parseContext = parseContext;
            this.error = error;
        }

        public Task<int> RunAsync(CancellationToken cancellationToken)
        {
            this.parseContext.parserOptions.OutputHandler?.Invoke(this.error);
            return Task.FromResult(this.parseContext.parserOptions.ErrorCode);
        }
    }
}
