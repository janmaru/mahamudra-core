using System.Collections.Generic;

namespace Mahamudra.Result.Core.Patterns
{
    public class Failure<TSuccess, TMessage> : Result<TSuccess, TMessage>
    {
        public Failure(IList<TMessage> messages) : base(messages)
        {

        }

        public Failure(TMessage message) : base(message)
        {

        }
    }
} 