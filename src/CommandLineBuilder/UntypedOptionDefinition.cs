using System;
using System.Linq.Expressions;

namespace CommandLine
{
    internal sealed class UntypedOptionDefinition
    {
        internal OptionName Name { get; }
        private Func<object, string, ApplicationResult> Applicator { get; }

        internal UntypedOptionDefinition(
            OptionName optionName,
            Func<object, string, ApplicationResult> applicator)
        {
            this.Name = optionName;
            this.Applicator = applicator;
        }

        internal Func<object, ApplicationResult> ApplicatorClosureAroundValue(string value)
        {
            return (o) => this.Applicator(o, value);
        }
    }
}
