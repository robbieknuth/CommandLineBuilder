using System;
using System.Linq.Expressions;

namespace CommandLine
{
    public sealed class Converter<T> : IConverter<T>
    {
        private readonly Func<string, ConversionResult<T>> converter;

        public Converter(Func<string, ConversionResult<T>> converter)
        {
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public ConversionResult<T> Convert(string input) => this.converter(input);

        public static implicit operator Converter<T>(Func<string, ConversionResult<T>> converter)
        {
            return new Converter<T>(converter);
        }

        internal static Func<object, string, ApplicationResult> CreatePositionalConverter<TSettings>(string name, Expression<Func<TSettings, T>> property, Conversion<T> converter)
            => CreateConverter<TSettings>(property, converter, s => $"Value '{ s }' was not convertable for positional [{ name }].");

        internal static Func<object, string, ApplicationResult> CreateOptionConverter<TSettings>(OptionName name, Expression<Func<TSettings, T>> property, Conversion<T> converter)
            => CreateConverter<TSettings>(property, converter, s => $"Value '{ s }' was not convertable for option [{ name }].");

        private static Func<object, string, ApplicationResult> CreateConverter<TSettings>(
            Expression<Func<TSettings, T>> property,
            Conversion<T> converter,
            Func<string, string> error)
        {
            if (!(property.Body is MemberExpression))
            {
                throw new ArgumentException("Expected a member expression as the property selector.");
            }

            return (o, s) =>
            {
                var result = converter(s);
                if (result.IsSuccess)
                {
                    var member = (MemberExpression)property.Body;
                    var setterValue = Expression.Parameter(typeof(T), "value");
                    var setExpression = Expression.Lambda<Action<TSettings, T>>(
                        Expression.Assign(member, setterValue), property.Parameters[0], setterValue);
                    var setter = setExpression.Compile(preferInterpretation: true);
                    setter((TSettings)o, result.Value);
                    return ApplicationResult.FromSuccess();
                }
                else
                {
                    return ApplicationResult.FromError(error(s), result.ConversionError);
                }
            };
        }
    }
}
