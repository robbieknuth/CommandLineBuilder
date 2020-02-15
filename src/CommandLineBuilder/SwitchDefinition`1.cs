using System;

namespace CommandLine
{
    public sealed class SwitchDefinition<TSettings>
    {
        private readonly OptionName longForm;
        private readonly OptionName? shortForm;

        internal Action<TSettings> Applicator { get; }

        private SwitchDefinition(
            OptionName longForm,
            OptionName? shortForm,
            Action<TSettings> applicator)
        {
            this.longForm = longForm;
            this.shortForm = shortForm;
            this.Applicator = applicator;
        }

        internal UntypedSwitchDefinition ToUntyped() => new UntypedSwitchDefinition(
            this.longForm,
            this.shortForm,
            o => this.Applicator((TSettings)o));

        public static SwitchDefinition<TSettings> Create(
            string longForm,
            Action<TSettings> applicator) => Create(longForm, null, applicator);

        public static SwitchDefinition<TSettings> Create(
            string longForm,
            string? shortForm,
            Action<TSettings> applicator)
        {
            if (applicator is null)
            {
                throw new ArgumentNullException(nameof(applicator));
            }

            var longFormOptionName = OptionName.FromLongForm(longForm);
            OptionName? shortFormOptionName = default;
            if (!(shortForm is null))
            {
                shortFormOptionName = OptionName.FromShortForm(shortForm);
            }

            return new SwitchDefinition<TSettings>(longFormOptionName, shortFormOptionName, applicator);
        }
    }
}
