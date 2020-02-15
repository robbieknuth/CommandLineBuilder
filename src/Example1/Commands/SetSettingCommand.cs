using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Example1.CommandSettings;

namespace Example1.Commands
{
    class SetSettingCommand : IEntrypoint<SetSettingCommandSettings>
    {
        public async Task<int> RunAsync(SetSettingCommandSettings commandSettings, CancellationToken cancellationToken)
        {
            var settingsLoader = new SettingsLoader();
            var settings = await settingsLoader.Load(cancellationToken);
            if (string.Equals(commandSettings.SettingName, nameof(Settings.NumberValue), StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(commandSettings.SettingValue, out var numberValue))
                {
                    settings.NumberValue = numberValue;
                }
                else
                {
                    Console.WriteLine($"Could not convert '{commandSettings.SettingValue}' to an int.");
                    return 1;
                }
            }
            else if (string.Equals(commandSettings.SettingName, nameof(Settings.StringValue), StringComparison.OrdinalIgnoreCase))
            {
                settings.StringValue = commandSettings.SettingValue;
            }
            else
            {
                Console.WriteLine($"Unknown setting name: '{commandSettings.SettingName}'.");
                return 1;
            }

            await settingsLoader.Write(settings, cancellationToken);
            return 0;
        }
    }
}
