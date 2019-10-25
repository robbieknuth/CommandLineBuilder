using System.Threading;
using System.Threading.Tasks;

namespace FluentCommandLine
{
    public interface IEntrypoint
    {
        Task<int> RunAsync(CancellationToken cancellationToken);
    }
}
