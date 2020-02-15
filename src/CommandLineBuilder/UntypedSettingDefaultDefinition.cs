using System;

namespace CommandLine
{
    internal sealed class UntypedSettingDefaultDefinition
    {
        internal Action<object> Applicator { get; }

        internal UntypedSettingDefaultDefinition(Action<object> applicator)
        {
            this.Applicator = applicator;
        }
    }
}