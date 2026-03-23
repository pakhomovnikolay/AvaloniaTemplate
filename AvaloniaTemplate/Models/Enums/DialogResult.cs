using System;

namespace AvaloniaTemplate.Models.Enums
{
    public class DialogResult
    {
        public bool Success { get; }
        public string? Error { get; }
        public Exception? Exception { get; }

        private DialogResult(bool success, string? error = null, Exception? ex = null)
        {
            Success = success;
            Error = error;
            Exception = ex;
        }

        public static DialogResult Ok() => new(true);
        public static DialogResult Fail(string error, Exception? ex = null) => new(false, error, ex);
    }
}
