using System;
using System.Collections.Generic;

namespace Mahamudra.Result.Core.Patterns
{
    public abstract class Result<TSuccess, TMessage>
    {
        public Result(TSuccess input)
        {
            _ = input ?? throw new Exception("You must provide an input!");
            this.Value = input;
        }

        public Result(List<TMessage> messages)
        {
            _ = messages ?? throw new Exception("You must provide an input for the messages!"); 
            _ = Messages ?? (Messages = new List<TMessage>()); 
            this.Messages.AddRange(messages);
            Success = false;
        }

        public Result(TMessage message)
        {
            _ = message ?? throw new Exception("You must provide an input for the message!");
            _ = Messages ?? (Messages = new List<TMessage>());

            this.Messages.Add(message);
            Success = false;
        }

        public TSuccess Value { get; }
        public bool Success { get; } = true;
        public List<TMessage> Messages { get; }
    }
}
