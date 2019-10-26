using System;

namespace CommandLine
{
    internal sealed class OptionApplicator
    {
        private readonly Func<object, string, ApplicationResult> applicator;

        public OptionName Name { get; }

        public OptionApplicator(OptionName name, Func<object, string, ApplicationResult> applicator)
        {
            this.Name = name;
            this.applicator = applicator;
        }

        public Func<object, ApplicationResult> Apply(string value)
        {
            return (o) => this.applicator(o, value);
        }
    }
}
