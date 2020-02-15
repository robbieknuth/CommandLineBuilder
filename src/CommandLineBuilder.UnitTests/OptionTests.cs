using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CommandLine.UnitTests
{
    public sealed class OptionTests
    {
        [Theory]
        [InlineData("one", "--option", "2")]
        [InlineData("--option", "2", "one")]
        public async Task OptionOnHigherCommand(string argOne, string argTwo, string argThree)
        {
            var entrypoint = new CommandLineBuilder<Settings>("test")
                .AddOption("--option", x => x.Value, Converters.IntConverter)
                .AddTerminalCommandWithSettings<Entrypoint, Settings>("one", commandBuilder => {})
                .Build()
                .Parse(new string[] { argOne, argTwo, argThree });
            
            Assert.Equal(typeof(EntrypointWithSettingThunk<Settings>), entrypoint.GetType());
            var value = await entrypoint.RunAsync(default);
            Assert.Equal(2, value);
        }

        [Theory]
        [InlineData("one", "-o", "2")]
        [InlineData("-o", "2", "one")]
        public async Task OptionSimpleShortForm(string argOne, string argTwo, string argThree)
        {
            var entrypoint = new CommandLineBuilder<Settings>("test")
                .AddOption("--option", "-o", x => x.Value, Converters.IntConverter)
                .AddTerminalCommandWithSettings<Entrypoint, Settings>("one", commandBuilder => {})
                .Build()
                .Parse(new string[] { argOne, argTwo, argThree });
            
            Assert.Equal(typeof(EntrypointWithSettingThunk<Settings>), entrypoint.GetType());
            var value = await entrypoint.RunAsync(default);
            Assert.Equal(2, value);
        }

        [Fact]
        public void NullOptionLongName()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CommandLineBuilder<Settings>("test")
                    .AddOption(null, x => x.Value, Converters.IntConverter));
        }

        [Theory]
        [InlineData("")]
        [InlineData("-")]
        [InlineData("--")]
        [InlineData("---")]
        [InlineData(" ")]
        [InlineData("-abc ")]
        [InlineData("-ab bc")]
        public void InvalidOptionLongNames(string optionName)
        {
            Assert.Throws<ArgumentException>(() =>
                new CommandLineBuilder<Settings>("test")
                    .AddOption(optionName, x => x.Value, Converters.IntConverter));
        }

        [Theory]
        [InlineData("")]
        [InlineData("-")]
        [InlineData("--")]
        [InlineData("---")]
        [InlineData(" ")]
        [InlineData("-abc ")]
        [InlineData("-ab bc")]
        public void InvalidOptionShortNames(string optionName)
        {
            Assert.Throws<ArgumentException>(() =>
                new CommandLineBuilder<Settings>("test")
                    .AddOption("--optionName", optionName, x => x.Value, Converters.IntConverter));
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