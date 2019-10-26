using System.Threading;
using System.Threading.Tasks;

namespace CommandLineBuilder
{
    public interface IEntrypointWithSettings<TSettings>
    {
        Task<int> RunAsync(TSettings settings, CancellationToken cancellationToken);
    }
}
