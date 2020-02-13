using System;
using System.Linq.Expressions;

namespace CommandLine
{
    internal sealed class UntypedPositionalDefinition
    {
        internal string Name { get; }
        internal bool Required { get; }
        private Func<object, string, ApplicationResult> Applicator { get; }

        internal UntypedPositionalDefinition(
            string name,
            bool required,
            Func<object, string, ApplicationResult> applicator)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.Required = required;
            this.Applicator = applicator ?? throw new ArgumentNullException(nameof(applicator));
        }

        internal Func<object, ApplicationResult> ApplicatorClosureAroundValue(string value)
        {
            return (o) => this.Applicator(o, value);
        }
    }
}
