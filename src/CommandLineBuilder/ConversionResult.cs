#nullable disable

using System;

namespace CommandLine
{
    public readonly struct ConversionResult<T>
    { 
        private readonly bool isInitialized;
        private readonly T value;
        private readonly string error;
        private readonly bool isSuccess;

        public string ConversionError
        {
            get
            {
                if (!this.isInitialized)
                {
                    throw new InvalidOperationException("No default constructors, please.");
                }

                if (this.isSuccess)
                {
                    throw new InvalidOperationException($"Cannot access { nameof(ConversionResult<T>.ConversionError )}, conversion succeeded.");
                }

                return this.error;
            }
        }

        public T Value
        {
            get
            {
                if (!this.isInitialized)
                {
                    throw new InvalidOperationException("No default constructors, please.");
                }

                if (!this.isSuccess)
                {
                    throw new InvalidOperationException($"Cannot access { nameof(ConversionResult<T>.Value)}, conversion succeeded.");
                }

                return this.value;
            }
        }

        public bool IsSuccess
        {
            get
            {
                if (!this.isInitialized)
                {
                    throw new InvalidOperationException("No default constructors, please.");
                }

                return this.isSuccess;
            }
        }

        private ConversionResult(T value, string error, bool success)
        {
            this.isInitialized = true;
            this.value = value;
            this.error = error;
            this.isSuccess = success;
        }

        public static ConversionResult<T> Success(T value) => new ConversionResult<T>(value, default, true);

        public static ConversionResult<T> Failure(string error) => new ConversionResult<T>(default, error, false); 
    }
}
