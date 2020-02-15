using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CommandLine.UnitTests
{
    public sealed class SwitchTests
    {
        [Theory]
        [InlineData("one", "--switch")]
        [InlineData("--switch", "one")]
        public async Task SwitchOnHigherCommand(string argOne, string argTwo)
        {
            var entrypoint = new CommandLineBuilder<Settings>("test")
                .AddSwitch("--switch", x => x.Value = 2)
                .AddTerminalCommandWithSettings<Entrypoint, Settings>("one", commandBuilder => {})
                .Build()
                .Parse(new string[] { argOne, argTwo });
            
            Assert.Equal(typeof(EntrypointWithSettingThunk<Settings>), entrypoint.GetType());
            var value = await entrypoint.RunAsync(default);
            Assert.Equal(2, value);
        }

        [Theory]
        [InlineData("one", "-s")]
        [InlineData("-s", "one")]
        public async Task SwitchSimpleShortForm(string argOne, string argTwo)
        {
            var entrypoint = new CommandLineBuilder<Settings>("test")
                .AddSwitch("--switch", "-s", x => x.Value = 2)
                .AddTerminalCommandWithSettings<Entrypoint, Settings>("one", commandBuilder => {})
                .Build()
                .Parse(new string[] { argOne, argTwo });
            
            Assert.Equal(typeof(EntrypointWithSettingThunk<Settings>), entrypoint.GetType());
            var value = await entrypoint.RunAsync(default);
            Assert.Equal(2, value);
        }

        [Theory]
        [InlineData(null)]
        public void NullSwitchLongName(string switchName)
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CommandLineBuilder<Settings>("test")
                    .AddSwitch(switchName, x => x.Value = 2));
        }

        [Theory]
        [InlineData("")]
        [InlineData("-")]
        [InlineData("--")]
        [InlineData("---")]
        [InlineData(" ")]
        [InlineData("-abc ")]
        [InlineData("-ab bc")]
        public void InvalidSwitchLongNames(string switchName)
        {
            Assert.Throws<ArgumentException>(() =>
                new CommandLineBuilder<Settings>("test")
                    .AddSwitch(switchName, x => x.Value = 2));
        }

        [Theory]
        [InlineData("")]
        [InlineData("-")]
        [InlineData("--")]
        [InlineData("---")]
        [InlineData(" ")]
        [InlineData("-abc ")]
        [InlineData("-ab bc")]
        public void InvalidSwitchShortNames(string switchName)
        {
            Assert.Throws<ArgumentException>(() =>
                new CommandLineBuilder<Settings>("test")
                    .AddSwitch("--switchName", switchName, x => x.Value = 2));
        }

        private class Entrypoint : IEntrypointWithSettings<Settings>
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