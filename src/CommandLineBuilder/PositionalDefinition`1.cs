using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class PositionalDefinition<TSettings>
    {
        private readonly string name;
        private readonly bool required;
        private readonly Func<object, string, ApplicationResult> applicator;

        private PositionalDefinition(
            string name,
            bool required,
            Func<object, string, ApplicationResult> applicator)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.name = name;
            this.required = required;
            this.applicator = applicator;
        }

        public static PositionalDefinition<TSettings> Create<TPropertyValue>(
            string name,
            bool required,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> converter)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Positional name cannot be null or whitespace", nameof(name));
            }

            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            var applicator = Converter<TPropertyValue>.CreatePositionalConverter(name, property, converter);
            return new PositionalDefinition<TSettings>(name, required, applicator);
        }

        internal UntypedPositionalDefinition ToUntyped()
        => new UntypedPositionalDefinition(this.name, this.required, this.applicator);
    }
}
