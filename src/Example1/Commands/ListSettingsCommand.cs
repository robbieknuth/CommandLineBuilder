using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace Example1.Commands
{
    sealed class ListSettingsCommand : IEntrypoint
    {
        public async Task<int> RunAsync(CancellationToken cancellationToken)
        {
            var settingsLoader = new SettingsLoader();
            var settings = await settingsLoader.Load(cancellationToken);
            Console.WriteLine($"StringValue => {settings.StringValue ?? "<null>"}");
            Console.WriteLine($"NumberValue => {settings.NumberValue}");
            return 0;
        }
    }
}
