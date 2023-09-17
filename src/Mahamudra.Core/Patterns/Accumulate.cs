using System;
using System.Collections;
using System.Collections.Generic;

namespace Mahamudra.Result.Core.Patterns
{
    public static partial class FunctionalExtensions
    {
        public static Result<TSuccess, TFailure> Acc<TSuccess, TFailure>
            (this Result<TSuccess, TFailure> input,
            Func<Result<TSuccess, TFailure>, Result<TSuccess, TFailure>> function)
        {
            List<TFailure> messages = new List<TFailure>();
            var result = function(input);
            if (input.Success) 
                return result;
            messages.AddRange(input.Messages); 
            if(!result.Success)
                messages.AddRange(result.Messages);
            return new Failure<TSuccess, TFailure>(messages);
        }

        public static Result<TSuccess, TFailure> Acc<TSuccess, TFailure>
            (this TSuccess input,
            Func<TSuccess, Result<TSuccess, TFailure>> function)
        {
            return function(input);
        }
    }
}
