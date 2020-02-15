using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CommandLine.UnitTests
{
    public sealed class SettingDefaultTests
    {
        [Fact]
        public async Task SettingDefaultSimple()
        {
            var entrypoint = new CommandLineBuilder<Settings>("test")
                .AddSettingDefault(settings => settings.Value = 1)
                .AddTerminalCommandWithSettings<Entrypoint, Settings>("one", commandBuilder => {})
                .Build()
                .Parse(new string[] { "one" });
            
            Assert.Equal(typeof(EntrypointWithSettingThunk<Settings>), entrypoint.GetType());
            var value = await entrypoint.RunAsync(default);
            Assert.Equal(1, value);
        }

        [Fact]
        public async Task SettingDefaultOverriden()
        {
            var entrypoint = new CommandLineBuilder<Settings>("test")
                .AddSettingDefault(settings => settings.Value = 1)
                .AddTerminalCommandWithSettings<Entrypoint, Settings>("one", commandBuilder => commandBuilder
                    .AddSettingDefault(Settings => Settings.Value = 2))
                .Build()
                .Parse(new string[] { "one" });
            
            Assert.Equal(typeof(EntrypointWithSettingThunk<Settings>), entrypoint.GetType());
            var value = await entrypoint.RunAsync(default);
            Assert.Equal(2, value);
        }

        [Fact]
        public async Task SettingDefaultOverriddenBySwitch()
        {
            var entrypoint = new CommandLineBuilder<Settings>("test")
                .AddSettingDefault(settings => settings.Value = 1)
                .AddTerminalCommandWithSettings<Entrypoint, Settings>("one", commandBuilder => commandBuilder
                    .AddSwitch("--switch", s => s.Value = 2))
                .Build()
                .Parse(new string[] { "one", "--switch" });
            
            Assert.Equal(typeof(EntrypointWithSettingThunk<Settings>), entrypoint.GetType());
            var value = await entrypoint.RunAsync(default);
            Assert.Equal(2, value);
        }

        [Fact]
        public async Task SettingDefaultOverriddenByOption()
        {
            var entrypoint = new CommandLineBuilder<Settings>("test")
                .AddSettingDefault(settings => settings.Value = 1)
                .AddTerminalCommandWithSettings<Entrypoint, Settings>("one", commandBuilder => commandBuilder
                    .AddOption("--option", x => x.Value, Converters.IntConverter))
                .Build()
                .Parse(new string[] { "one", "--option", "2" });
            
            Assert.Equal(typeof(EntrypointWithSettingThunk<Settings>), entrypoint.GetType());
            var value = await entrypoint.RunAsync(default);
            Assert.Equal(2, value);
        }

        private class Entrypoint : IEntrypoint<Settings>
        {
            public Task<int> RunAsync(Settings settings, CancellationToken cancellationToken)
            {
                return Task.FromResult(settings.Value);
            }
        }

        private class Settings
        {
            public int Value { get; set; }
        }
    }
}