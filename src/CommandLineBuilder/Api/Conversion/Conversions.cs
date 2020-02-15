using System;

namespace CommandLine
{
    public static class Converters
    {
        public static readonly Conversion<int> IntConverter = new Conversion<int>(s =>
        {
            if (int.TryParse(s, out var result))
            {
                return ConversionResult<int>.Success(result);
            }
            else
            {
                return ConversionResult<int>.Failure($"Could not convert '{ s }' to a 32 bit signed integer.");
            }
        });

        public static readonly Conversion<bool> BoolConverter = new Conversion<bool>(s =>
        {
            if (bool.TryParse(s, out var result))
            {
                return ConversionResult<bool>.Success(result);
            }
            else
            {
                return ConversionResult<bool>.Failure($"Could not convert '{ s }' to a boolean.");
            }
        });

        public static Conversion<T> EnumConverter<T>()
            where T : struct, Enum
        {
            return new Conversion<T>(s =>
            {
                if (Enum.TryParse<T>(s, true, out var result))
                {
                    return ConversionResult<T>.Success(result);
                }
                else
                {
                    return ConversionResult<T>.Failure($"Could not convert '{ s }' to { typeof(T).FullName }.");
                }
            });
        }

        public static readonly Conversion<string> Identity = new Conversion<string>(s => ConversionResult<string>.Success(s));
    }
}
