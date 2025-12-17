namespace Mahamudra.Core.Errors
{
    public abstract class  Error
    {
        public int Code { get; private set; }

        public string  Description { get; private set; }

        public string Message { get; private set; }

        public Error(int statusCode, string statusDescription)
        {
            this.Code = statusCode;
            this.Description = statusDescription;
        }

        public Error(int code, string description, string message)
            : this(code, description)
        {
            this.Message = message;
        }
    }
}
