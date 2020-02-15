using System;

namespace CommandLine
{
    internal sealed class UntypedPositionalDefinition
    {
        public string Name { get; }
        public bool Required { get; }
        private Func<object, string, ApplicationResult> Applicator { get; }

        public UntypedPositionalDefinition(
            string name,
            bool required,
            Func<object, string, ApplicationResult> applicator)
        {
            this.Name = name;
            this.Required = required;
            this.Applicator = applicator ?? throw new ArgumentNullException(nameof(applicator));
        }

        internal Func<object, ApplicationResult> ApplicatorClosureAroundValue(string value)
        => (o) => this.Applicator(o, value);
    }
}
