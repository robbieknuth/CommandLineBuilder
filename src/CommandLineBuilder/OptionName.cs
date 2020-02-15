using System;
using System.Diagnostics.CodeAnalysis;

namespace CommandLine
{
    internal sealed class OptionName : IEquatable<OptionName>
    {
        private string Value { get; }

        private OptionName(string value)
        {
            this.Value = value;
        }

        public static OptionName FromLongForm(string? longForm)
        {
            if (longForm is null)
            {
                throw new ArgumentNullException(nameof(longForm));
            }

            if (!longForm.StartsWith("--"))
            {
                throw new ArgumentException("Long form must start with '--'.", nameof(longForm));
            }

            if (longForm.Length < 3)
            {
                throw new ArgumentException("Long form requires a value following '--'.", nameof(longForm));
            }

            AssertAlphaNumeric(longForm.AsSpan(2), "Long form", nameof(longForm));

            return new OptionName(longForm);
        }

        public static OptionName FromShortForm(string? shortForm)
        {
            if (shortForm is null)
            {
                throw new ArgumentNullException(nameof(shortForm));
            }

            if (!shortForm.StartsWith("-"))
            {
                throw new ArgumentException("Short form must start with '-'.", nameof(shortForm));
            }

            if (shortForm.Length < 2)
            {
                throw new ArgumentException("Short form requires a value following '-'.", nameof(shortForm));
            }

            AssertAlphaNumeric(shortForm.AsSpan(1), "Short form", nameof(shortForm));

            return new OptionName(shortForm);
        }

        internal static bool FromUnknownValue(string value, [NotNullWhen(true)] out OptionName? result)
        {
            var span = value.AsSpan();
            if (value.StartsWith("--"))
            {
                span = span.Slice(2);
                if (span.Length < 1)
                {
                    result = default;
                    return false;
                }

                foreach (var c in span)
                {
                    if (!char.IsLetterOrDigit(c))
                    {
                        result = default;
                        return false;
                    }
                }

                result = new OptionName(value);
                return true;
            }

            if (value.StartsWith("-"))
            {
                span = span.Slice(1);
                if (span.Length < 1)
                {
                    result = default;
                    return false;
                }

                foreach (var c in span)
                {
                    if (!char.IsLetterOrDigit(c))
                    {
                        result = default;
                        return false;
                    }
                }

                result = new OptionName(value);
                return true;
            }

            result = default ;
            return false;
        }

        public static bool LooksLikeOptionOrSwitch(string value)
        {
            return value.StartsWith("-");
        }

        public override string ToString() => this.Value;

        public override int GetHashCode() => this.Value.GetHashCode();

        public override bool Equals(object? obj) => obj is OptionName opt && this.Equals(opt);

        public bool Equals(OptionName other) => this.Value == other.Value;

        internal static string Combine(OptionName longForm, OptionName? shortForm)
        {
            var result = longForm.Value;
            if (!(shortForm is null))
            {
                result += $", {shortForm.Value}";
            }
            return result;
        }

        private static void AssertAlphaNumeric(
            ReadOnlySpan<char> value,
            string optionNameType,
            string argName)
        {
            foreach (var c in value)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    throw new ArgumentException($"{optionNameType} cannot contain '{c}'.", argName);
                }
            }
        }
    }
}
