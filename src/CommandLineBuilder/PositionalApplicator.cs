using System;

namespace CommandLineBuilder
{
    internal sealed class PositionalApplicator
    {
        private readonly Func<object, string, ApplicationResult> applicator;

        public string Name { get; }

        public PositionalApplicator(string name, Func<object, string, ApplicationResult> applicator)
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
