namespace Shared.Exceptions
{
    public sealed class ValidationException(IReadOnlyDictionary<string, string[]> errors) : Exception("Validation failed")
    {
        public const string ValidationErrorCode = "validation_error";

        public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
    }
}
