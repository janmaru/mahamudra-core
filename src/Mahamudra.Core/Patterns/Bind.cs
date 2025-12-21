using System;

namespace Mahamudra.Core.Patterns
{
    public static partial class FunctionalExtensions
    {
        public static Result<TSuccess, TFailure> Bind<TSuccess, TFailure>
            (this Result<TSuccess, TFailure> input,
            Func<Result<TSuccess, TFailure>, Result<TSuccess, TFailure>> function)
        {
            if (input.Success) 
                return function(input); 
            return new Failure<TSuccess, TFailure>(input.Messages);
        }
 
        public static Result<TSuccess, TFailure> Bind<TSuccess, TFailure>
            (this TSuccess input, 
            Func<TSuccess, Result<TSuccess, TFailure>> function)
        {
            return function(input);
        }
    }
} 