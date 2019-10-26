using System;
using System.Diagnostics.CodeAnalysis;

namespace CommandLineBuilder
{
    internal sealed class OptionName : IEquatable<OptionName>
    {
        public string? LongForm { get; }
        public string? ShortForm { get; }

        private OptionName(string? longForm, string? shortForm)
        {
            if (longForm != null && !longForm.StartsWith("--"))
            {
                throw new CommandStructureException("Long form must start with '-'.");
            }

            if (shortForm != null && !shortForm.StartsWith("-"))
            {
                throw new CommandStructureException("Short form must start with '-'.");
            }

            this.LongForm = longForm?.Substring(2);
            this.ShortForm = shortForm?.Substring(1);
        }

        public static OptionName FromLongForm(string longForm)
        {
            return new OptionName(longForm, null);
        }
        
        public static OptionName FromShortForm(string shortForm)
        {
            return new OptionName(null, shortForm);
        }

        public static OptionName FromLongAndShortForm(string longForm, string shortForm)
        {
            return new OptionName(longForm, shortForm);
        }

        public static bool FromUnknownValue(string value, [NotNullWhen(true)] out OptionName? result)
        {
            if (value.StartsWith("--"))
            {
                result = FromLongForm(value);
                return true;
            }

            if (value.StartsWith("-"))
            {
                result = FromShortForm(value);
                return true;
            }

            result = null;
            return false;
        }

        public static bool LooksLikeOptionOrSwitch(string value)
        {
            return value.StartsWith("-");
        }

        public bool Matches(string arg)
        {
            if (arg.StartsWith("--"))
            {
                var shortFormArg = arg.AsSpan(2);
                return this.LongForm.AsSpan().Equals(shortFormArg, StringComparison.Ordinal);
            }
            else if (arg.StartsWith("-"))
            {
                if (this.ShortForm != null)
                {
                    var shortFormArg = arg.AsSpan(1);
                    return this.ShortForm.AsSpan().Equals(shortFormArg, StringComparison.Ordinal);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override string ToString() => this.ShortForm == null ? $"{ this.LongForm }" : $"{ this.LongForm } ( {this.ShortForm} )";

        public string ToHelpString()
        {
            if (this.ShortForm != null)
            {
                if (this.LongForm != null)
                {
                    return $"-{this.ShortForm}, --{this.LongForm}";
                }
                else
                {
                    return $"{this.ShortForm}";
                }
            }
            else
            {
                return $"--{this.LongForm}";
            }
        }

        public override int GetHashCode() => this.LongForm?.GetHashCode() ?? this.ShortForm?.GetHashCode() ?? 0;

        public override bool Equals(object? obj) => this.Equals(obj as OptionName);

        public bool Equals(OptionName? other) => other != null && (this.LongForm == other.LongForm || this.ShortForm == other.ShortForm);
    }
}
