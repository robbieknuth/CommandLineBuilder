using System;

namespace CommandLine
{
    internal readonly struct ApplicationResult
    {
        private readonly bool isInitialized;
        private readonly bool isSuccess;
        private readonly string? error;
        private readonly string? detail;

        public string Error
        {
            get
            {
                if (!this.isInitialized)
                {
                    throw new InvalidOperationException("No default constructors, please.");
                }

                if (this.isSuccess)
                {
                    throw new InvalidOperationException("This was not a successful application.");
                }

                return this.error!;
            }
        }

        public string? ErrorDetail
        {
            get
            {
                if (!this.isInitialized)
                {
                    throw new InvalidOperationException("No default constructors, please.");
                }

                if (this.isSuccess)
                {
                    throw new InvalidOperationException("This was not a successful application.");
                }

                return this.detail;
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

        private ApplicationResult(bool isSuccess, string? error, string? detail)
        {
            this.isInitialized = true;
            this.isSuccess = isSuccess;
            this.error = error;
            this.detail = detail;
        }

        public static ApplicationResult FromError(string error, string? detail)
        => new ApplicationResult(false, error, detail);

        public static ApplicationResult FromSuccess()
        => new ApplicationResult(true, default, default);
    }
}
