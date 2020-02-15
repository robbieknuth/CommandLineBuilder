using System;

namespace CommandLine
{
    internal sealed class UntypedSettingDefaultDefinition
    {
        public Action<object> Applicator { get; }

        public UntypedSettingDefaultDefinition(Action<object> applicator)
        => this.Applicator = applicator;
    }
}