using System;

namespace AvaloniaTemplate.Models.Enums
{
    public class CryptResult
    {
        public bool Success { get; }
        public string? Error { get; }
        public Exception? Exception { get; }

        private CryptResult(bool success, string? error = null, Exception? ex = null)
        {
            Success = success;
            Error = error;
            Exception = ex;
        }

        public static CryptResult Ok() => new(true);
        public static CryptResult Fail(string error, Exception? ex = null) => new(false, error, ex);
    }
}
