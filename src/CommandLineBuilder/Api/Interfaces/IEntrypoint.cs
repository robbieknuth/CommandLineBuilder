using System.Threading;
using System.Threading.Tasks;

namespace CommandLine
{
    public interface IEntrypoint
    {
        Task<int> RunAsync(CancellationToken cancellationToken);
    }
}
