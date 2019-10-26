using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Example1.CommandSettings;

namespace Example1.Commands
{
    class PrintCommand : IEntrypointWithSettings<PrintCommandSettings>
    {
        public async Task<int> RunAsync(PrintCommandSettings commandSettings, CancellationToken cancellationToken)
        {
            var settings = await new SettingsLoader().Load(cancellationToken);
            if (settings.StringValue is null)
            {
                Console.WriteLine($"Requires setting value '{nameof(Settings.StringValue)}' to be set.");
                return 1;
            }
            else if (settings.NumberValue < 0)
            {
                Console.WriteLine($"Setting value '{nameof(Settings.NumberValue)}' is negative. Cannot print negative times.");
                return 1;
            }

            var oldColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = commandSettings.Color;
                var times = Math.Min(commandSettings.Max, settings.NumberValue);
                for (var i = 0; i < times; i++)
                {
                    Console.WriteLine(settings.StringValue);
                }
            }
            finally
            {
                Console.ForegroundColor = oldColor;
            }

            return 0;
        }
    }
}
