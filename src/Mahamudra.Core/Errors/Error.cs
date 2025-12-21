namespace Mahamudra.Core.Errors
{
    /// <summary>
    /// Represents an error in the application.
    /// Immutable record type ensuring thread safety and value-based equality.
    /// </summary>
    public record Error
    {
        public int Code { get; init; }
        public string Description { get; init; }
        public string Message { get; init; }

        public Error(int code, string description, string message = "")
        {
            this.Code = code;
            this.Description = description ?? string.Empty;
            this.Message = message ?? string.Empty;
        }

        /// <summary>Creates a validation error (400).</summary>
        public static Error Validation(string description, string message = "")
            => new(400, description, message);

        /// <summary>Creates a not found error (404).</summary>
        public static Error NotFound(string description, string message = "")
            => new(404, description, message);

        /// <summary>Creates a conflict error (409).</summary>
        public static Error Conflict(string description, string message = "")
            => new(409, description, message);

        /// <summary>Creates an unauthorized error (401).</summary>
        public static Error Unauthorized(string description, string message = "")
            => new(401, description, message);

        /// <summary>Creates a forbidden error (403).</summary>
        public static Error Forbidden(string description, string message = "")
            => new(403, description, message);

        /// <summary>Creates an internal server error (500).</summary>
        public static Error Internal(string description, string message = "")
            => new(500, description, message);

        /// <summary>Creates a bad request error (400).</summary>
        public static Error BadRequest(string description, string message = "")
            => new(400, description, message);
    }
}
