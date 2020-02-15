using System;

namespace CommandLine
{
    public sealed class SettingDefaultDefinition<TSettings>
    {
        private readonly Action<TSettings> applicator;

        private SettingDefaultDefinition(Action<TSettings> applicator)
        => this.applicator = applicator;

        public static SettingDefaultDefinition<TSettings> Create(Action<TSettings> applicator)
        {
            if (applicator is null)
            {
                throw new ArgumentNullException(nameof(applicator));
            }

            return new SettingDefaultDefinition<TSettings>(applicator);
        }

        internal UntypedSettingDefaultDefinition ToUntyped()
        => new UntypedSettingDefaultDefinition(o => this.applicator((TSettings)o));
    }
}