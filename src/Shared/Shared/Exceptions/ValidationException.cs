namespace Shared.Exceptions
{
    public sealed class ValidationException : AppException
    {
        public const string ValidationErrorCode = "validation_error";

        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public ValidationException(IReadOnlyDictionary<string, string[]> errors)
            : base(ValidationErrorCode, "One or more validation errors occurred.", 422)
        {
            Errors = errors;
        }
    }
}
