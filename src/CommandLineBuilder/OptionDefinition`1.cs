using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class OptionDefinition<TSettings>
    {
        private readonly OptionName longForm;
        private readonly OptionName? shortForm;
        private readonly Func<object, string, ApplicationResult> applicator;

        private OptionDefinition(
            OptionName longForm,
            OptionName? shortForm,
            Func<object, string, ApplicationResult> applicator)
        {
            this.longForm = longForm;
            this.shortForm = shortForm;
            this.applicator = applicator;
        }

        internal UntypedOptionDefinition ToUntyped() 
            => new UntypedOptionDefinition(this.longForm, this.shortForm, this.applicator);

        public static OptionDefinition<TSettings> Create<TPropertyValue>(
            string longForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion) 
            => Create(longForm, null, property, conversion);

        public static OptionDefinition<TSettings> Create<TPropertyValue>(
            string longForm,
            string? shortForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (conversion is null)
            {
                throw new ArgumentNullException(nameof(conversion));
            }

            var longFormOptionName = OptionName.FromLongForm(longForm);
            OptionName? shortFormOptionName = default;
            if (!(shortForm is null))
            {
                shortFormOptionName = OptionName.FromShortForm(shortForm);
            }
            
            var applicator = Converter<TPropertyValue>.CreateOptionConverter(longFormOptionName, shortFormOptionName, property, conversion);
            return new OptionDefinition<TSettings>(longFormOptionName, shortFormOptionName, applicator);
        }
    }
}
