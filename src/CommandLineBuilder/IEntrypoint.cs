using System.Threading;
using System.Threading.Tasks;

namespace CommandLineBuilder
{
    public interface IEntrypoint
    {
        Task<int> RunAsync(CancellationToken cancellationToken);
    }
}
