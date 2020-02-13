using System;

namespace CommandLine
{
    internal sealed class UntypedSwitchDefinition
    {
        internal OptionName Name { get; set; }

        internal Action<object> Applicator { get; set; }

        internal UntypedSwitchDefinition(
            OptionName optionName,
            Action<object> applicator)
        {
            this.Name = optionName;
            this.Applicator = applicator;
        }
    }
}
