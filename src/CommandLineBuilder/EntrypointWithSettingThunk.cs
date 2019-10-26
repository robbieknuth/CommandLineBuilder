using System.Threading;
using System.Threading.Tasks;

namespace CommandLineBuilder
{
    internal class EntrypointWithSettingThunk<TSettings> : IEntrypoint
    {
        private readonly IEntrypointWithSettings<TSettings> actualEntrypoint;
        private readonly TSettings settings;

        public EntrypointWithSettingThunk(
            IEntrypointWithSettings<TSettings> actualEntrypoint,
            TSettings settings)
        {
            this.actualEntrypoint = actualEntrypoint;
            this.settings = settings;
        }

        public Task<int> RunAsync(CancellationToken cancellationToken)
            => this.actualEntrypoint.RunAsync(this.settings, cancellationToken);
    }
}
