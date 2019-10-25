using System;

namespace FluentCommandLine
{
    public sealed class HelpOptions
    {
        internal bool ShouldFailWithException { get; private set; }
        internal int ExitCode { get; private set; }
        internal Action<string>? OutputHandler { get; private set; }

        internal HelpOptions() => this.FailWithOutput(Console.WriteLine);

        public HelpOptions FailWithException()
        {
            this.ShouldFailWithException = true;
            this.OutputHandler = null;
            return this;
        }

        public HelpOptions FailWithOutput(Action<string> lineHandler, int exitCode = 1)
        {
            this.ExitCode = exitCode;
            this.ShouldFailWithException = false;
            this.OutputHandler = lineHandler;
            return this;
        }
    }
}
