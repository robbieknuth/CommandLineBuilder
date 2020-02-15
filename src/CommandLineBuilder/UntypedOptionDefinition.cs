using System;

namespace CommandLine
{
    internal sealed class UntypedOptionDefinition
    {
        public OptionName LongForm { get; }
        public OptionName? ShortForm { get; }
        private Func<object, string, ApplicationResult> Applicator { get; }

        internal UntypedOptionDefinition(
            OptionName longForm,
            OptionName? shortForm,
            Func<object, string, ApplicationResult> applicator)
        {
            this.LongForm = longForm;
            this.ShortForm = shortForm;
            this.Applicator = applicator;
        }

        internal Func<object, ApplicationResult> ApplicatorClosureAroundValue(string value)
        => (o) => this.Applicator(o, value);
    }
}
