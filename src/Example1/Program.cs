using System;
using System.Threading.Tasks;
using CommandLine;
using Example1.Commands;
using Example1.CommandSettings;

namespace Example1
{
    class Program
    {
        static Task<int> Main(string[] args)
        {
            return new CommandLineBuilder(nameof(Example1))
                .ConfigureHelp(helpOptions => helpOptions.FailWithOutput())
                .AddNonTerminalCommand("settings", settingsCommandBuilder => settingsCommandBuilder
                    .AddTerminalCommand<ListSettingsCommand>("list")
                    .AddTerminalCommandWithSettings<GetSettingCommand, GetSettingCommandSettings>("get", getSettingCommandBuilder => getSettingCommandBuilder
                        .AddPositional("name", x => x.SettingName, Converters.Identity))
                    .AddTerminalCommandWithSettings<SetSettingCommand, SetSettingCommandSettings>("set", setSettingsCommandBuilder => setSettingsCommandBuilder
                        .AddPositional("name", x => x.SettingName, Converters.Identity)
                        .AddPositional("value", x => x.SettingValue, Converters.Identity)))
                 .AddTerminalCommandWithSettings<PrintCommand, PrintCommandSettings>("print", printCommandBuilder => printCommandBuilder
                    .AddOption("--max", x => x.Max, Converters.IntConverter)
                    .AddOption("--color", x => x.Color, Converters.EnumConverter<ConsoleColor>()))
                .Build()
                .Parse(args)
                .RunAsync(default);
        }
    }
}
