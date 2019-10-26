using System;

namespace CommandLine
{
    public sealed class HelpOptions
    {
        internal bool ShouldFailWithException { get; private set; }
        internal int ExitCode { get; private set; }

        internal HelpOptions() => this.FailWithOutput();

        public HelpOptions FailWithException()
        {
            this.ShouldFailWithException = true;
            return this;
        }

        public HelpOptions FailWithOutput(int exitCode = 1)
        {
            this.ExitCode = exitCode;
            this.ShouldFailWithException = false;
            return this;
        }
    }
}
