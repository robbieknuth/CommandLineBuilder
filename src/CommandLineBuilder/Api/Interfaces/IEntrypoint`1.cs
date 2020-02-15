using System.Threading;
using System.Threading.Tasks;

namespace CommandLine
{
    public interface IEntrypoint<TSettings>
    {
        Task<int> RunAsync(TSettings settings, CancellationToken cancellationToken);
    }
}
