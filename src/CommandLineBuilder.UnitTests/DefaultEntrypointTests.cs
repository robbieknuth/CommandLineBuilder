using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CommandLine.UnitTests
{
    public sealed class DefaultEntrypointTests
    {
        [Fact]
        public void CannotAssignDefaultEntrypointTwiceWithSettings()
        {
            Assert.Throws<CommandStructureException>(() => 
                new CommandLineBuilder<Settings>("abc")
                    .WithDefaultEntrypoint<EntrypointWithSettings>()
                    .WithDefaultEntrypoint<EntrypointWithSettings>());
        }

        [Fact]
        public async Task CommandWithSettingsEvaluatesCorrectEntrypoint()
        {
            var entrypoint = new CommandLineBuilder<Settings>("app")
                .WithDefaultEntrypoint<EntrypointWithSettings>()
                .AddTerminalCommandWithSettings<SecondaryWithSettings, Settings>("one")
                .Build()
                .Parse(new string[0]);
            
            var value = await entrypoint.RunAsync(default);
            Assert.Equal(1, value);
        }

        [Fact]
        public void CannotAssignDefaultEntrypointTwice()
        {
            Assert.Throws<CommandStructureException>(() => 
                new CommandLineBuilder("abc")
                    .WithDefaultEntrypoint<Entrypoint>()
                    .WithDefaultEntrypoint<Entrypoint>());
        }

        [Fact]
        public void CommandEvaluatesCorrectEntrypoint()
        {
            var entrypoint = new CommandLineBuilder("app")
                .WithDefaultEntrypoint<Entrypoint>()
                .AddTerminalCommandWithSettings<SecondaryWithSettings, Settings>("one")
                .Build()
                .Parse(new string[0]);
            
            Assert.Equal(typeof(Entrypoint), entrypoint.GetType());
        }

        private class Entrypoint : IEntrypoint
        {
            public Task<int> RunAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(0);
            }
        }

        private class SecondaryWithSettings : IEntrypointWithSettings<Settings>
        {
            public Task<int> RunAsync(Settings settings, CancellationToken cancellationToken)
            {
                return Task.FromResult(2);
            }
        }
        
        private class EntrypointWithSettings : IEntrypointWithSettings<Settings>
        {
            public Task<int> RunAsync(Settings settings, CancellationToken cancellationToken)
            {
                return Task.FromResult(1);
            }
        }

        private class Settings
        {
            public int Value { get; set; }
        }
    }
}