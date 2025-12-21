using System.Collections.Generic;

namespace Mahamudra.Core.Patterns
{
    /// <summary>
    /// Represents a failed result with error messages.
    /// </summary>
    public sealed class Failure<TSuccess, TMessage> : Result<TSuccess, TMessage>
    {
        public Failure(IList<TMessage> messages) : base(messages)
        {
        }

        public Failure(TMessage message) : base(message)
        {
        }
    }
} 