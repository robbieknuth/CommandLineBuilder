using System.Threading;
using System.Threading.Tasks;

namespace CommandLine
{
    internal sealed class EntrypointWithSettingThunk<TSettings> : IEntrypoint
    {
        private readonly IEntrypoint<TSettings> actualEntrypoint;
        private readonly TSettings settings;

        public EntrypointWithSettingThunk(
            IEntrypoint<TSettings> actualEntrypoint,
            TSettings settings)
        {
            this.actualEntrypoint = actualEntrypoint;
            this.settings = settings;
        }

        public Task<int> RunAsync(CancellationToken cancellationToken)
        => this.actualEntrypoint.RunAsync(this.settings, cancellationToken);
    }
}
