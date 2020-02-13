using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class OptionDefinition<TSettings>
    {
        internal OptionName Name { get; }
        private Func<object, string, ApplicationResult> Applicator { get; }

        private OptionDefinition(
            OptionName optionName,
            Func<object, string, ApplicationResult> applicator)
        {
            this.Name = optionName;
            this.Applicator = applicator;
        }

        internal UntypedOptionDefinition ToUntyped() => new UntypedOptionDefinition(this.Name, this.Applicator);

        public static OptionDefinition<TSettings> Create<TPropertyValue>(
            string longForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion) => Create(longForm, null, property, conversion);

        public static OptionDefinition<TSettings> Create<TPropertyValue>(
            string longForm,
            string? shortForm,
            Expression<Func<TSettings, TPropertyValue>> property,
            Conversion<TPropertyValue> conversion)
        {
            var optionName = OptionName.FromLongAndShortForm(longForm, shortForm);
            var applicator = Converter<TPropertyValue>.CreateOptionConverter(optionName, property, conversion);
            return new OptionDefinition<TSettings>(optionName, applicator);
        }
    }
}
