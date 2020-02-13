using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class PositionalDefinition<TSettings>
    {
        internal string Name { get; }
        internal bool Required { get; }
        private Func<object, string, ApplicationResult> Applicator { get; }

        private PositionalDefinition(
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

        internal UntypedPositionalDefinition ToUntyped() => new UntypedPositionalDefinition(this.Name, this.Required, this.Applicator);

        public static PositionalDefinition<TSettings> Create<TPropertyValue>(
            string name,
            bool required,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> converter)
        {
            var applicator = Converter<TPropertyValue>.CreatePositionalConverter(name, property, converter);
            return new PositionalDefinition<TSettings>(name, required, applicator);
        }
    }
}
