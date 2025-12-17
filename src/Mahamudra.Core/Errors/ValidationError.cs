namespace Mahamudra.Core.Errors
{
    public class ValidationError : Error
    {
        public ValidationError(int statusCode, string statusDescription) : base(statusCode, statusDescription)
        {
        }

        public ValidationError(int code, string description, string message) : base(code, description, message)
        {
        }
    }
}
