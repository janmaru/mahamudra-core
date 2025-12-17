using Mahamudra.Core.Extensions;
using System;
using System.Collections.Generic;

namespace Mahamudra.Result.Core.Patterns
{
    public abstract class Result<TSuccess, TMessage>
    {
        public Result(TSuccess input)
        {
            this.Value = input ?? throw new Exception("You must provide an input!");
            this.Success = true;
        }

        public Result(IList<TMessage> messages)
        {
            this.Messages = messages.IsNullOrEmpty() ? throw new Exception("You must provide an input for the messages!") : messages;
            this.Success = false;
        }

        public Result(TMessage message)
        {
            this.Messages = new List<TMessage>() { message ?? throw new Exception("You must provide an input for the message!") };
            this.Success = false;
        }

        public TSuccess Value { get; }
        public bool Success { get; }
        public IList<TMessage> Messages { get; }
    }
} 