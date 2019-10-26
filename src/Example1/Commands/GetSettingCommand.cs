using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Example1.CommandSettings;

namespace Example1.Commands
{
    class GetSettingCommand : IEntrypointWithSettings<GetSettingCommandSettings>
    {
        public async Task<int> RunAsync(GetSettingCommandSettings commandSettings, CancellationToken cancellationToken)
        {
            var settingsLoader = new SettingsLoader();
            var settings = await settingsLoader.Load(cancellationToken);
            if (string.Equals(commandSettings.SettingName, nameof(Settings.NumberValue), StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(settings.NumberValue);
            }
            else if (string.Equals(commandSettings.SettingName, nameof(Settings.StringValue), StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(settings.StringValue ?? "<null>");
            }
            else
            {
                Console.WriteLine($"Unknown setting name: '{commandSettings.SettingName}'.");
                return 1;
            }

            return 0;
        }
    }
}
