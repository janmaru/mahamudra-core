namespace Mahamudra.Core.Patterns
{
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    public sealed class Success<TSuccess, TMessage> : Result<TSuccess, TMessage>
    {
        public Success(TSuccess input) : base(input)
        {
        }
    }
} 