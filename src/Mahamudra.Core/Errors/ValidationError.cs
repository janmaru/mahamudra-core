using System.Collections.Generic;
using System.Linq;

namespace Mahamudra.Core.Errors
{
    /// <summary>
    /// Represents a validation error with optional field-level error details.
    /// </summary>
    public sealed record ValidationError : Error
    {
        public IReadOnlyDictionary<string, string[]> FieldErrors { get; init; }

        public ValidationError(
            string description,
            string message = "",
            IReadOnlyDictionary<string, string[]> fieldErrors = null)
            : base(400, description, message)
        {
            FieldErrors = fieldErrors ?? new Dictionary<string, string[]>();
        }

        public ValidationError(int code, string description, string message = "")
            : base(code, description, message)
        {
            FieldErrors = new Dictionary<string, string[]>();
        }

        /// <summary>Creates a validation error for a single field.</summary>
        public static ValidationError ForField(string field, params string[] errors)
        {
            var fieldErrors = new Dictionary<string, string[]>
            {
                [field] = errors ?? new[] { "Validation failed" }
            };
            return new ValidationError(
                $"Validation failed for {field}",
                string.Empty,
                fieldErrors);
        }

        /// <summary>Creates a validation error with multiple field errors.</summary>
        public static ValidationError ForFields(
            IReadOnlyDictionary<string, string[]> fieldErrors,
            string description = "Validation failed")
        {
            return new ValidationError(description, string.Empty, fieldErrors);
        }

        /// <summary>Returns true if there are field-level errors.</summary>
        public bool HasFieldErrors => FieldErrors.Any();
    }
}
