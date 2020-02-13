using System;

namespace CommandLine
{
    public sealed class SwitchDefinition<TSettings>
    {
        internal OptionName Name { get; set; }

        internal Action<TSettings> Applicator { get; set; }

        private SwitchDefinition(
            OptionName optionName,
            Action<TSettings> applicator)
        {
            this.Name = optionName;
            this.Applicator = applicator;
        }

        internal UntypedSwitchDefinition ToUntyped() => new UntypedSwitchDefinition(this.Name, o => this.Applicator((TSettings)o));

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

            var optionName = OptionName.FromLongAndShortForm(longForm, shortForm);
            return new SwitchDefinition<TSettings>(optionName, o => applicator(o));
        }
    }
}
