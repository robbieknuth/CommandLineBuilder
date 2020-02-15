using System;

namespace CommandLine
{
    internal sealed class UntypedSwitchDefinition
    {
        public OptionName LongForm { get; }
        public OptionName? ShortForm { get; }
        public Action<object> Applicator { get; }

        public UntypedSwitchDefinition(
            OptionName longForm,
            OptionName? shortForm,
            Action<object> applicator)
        {
            this.LongForm = longForm;
            this.ShortForm = shortForm;
            this.Applicator = applicator;
        }
    }
}
