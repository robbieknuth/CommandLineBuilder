using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Example1
{
    sealed class SettingsLoader
    {
        const string SettingsFileName = ".example1.settings";

        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public async Task<Settings> Load(CancellationToken cancellationToken)
        {
            var settingsFilePath = GetSettingsFilePath();

            Settings settings;

            if (!File.Exists(settingsFilePath))
            {
                settings = new Settings();
                await this.Write(settings, cancellationToken);
                return settings;
            }
            else
            {
                using (var fileStream = File.OpenRead(settingsFilePath))
                {
                    settings = await JsonSerializer.DeserializeAsync<Settings>(fileStream, SerializerOptions, cancellationToken);
                }
            }

            return settings;
        }

        public async Task Write(Settings settings, CancellationToken cancellationToken)
        {
            using (var fileStream = new FileStream(GetSettingsFilePath(), FileMode.Truncate))
            {
                await JsonSerializer.SerializeAsync(fileStream, settings, SerializerOptions, cancellationToken);
            }
        }

        private static string GetSettingsFilePath()
        {
            var settingsDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData, 
                Environment.SpecialFolderOption.Create);
            return Path.Combine(settingsDirectory, SettingsFileName);
        }
    }
}
