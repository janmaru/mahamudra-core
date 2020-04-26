using System;

namespace Mahamudra.Result.Core.Patterns
{
    public static partial class FunctionalExtensions
    {
        public static Result<TSuccess, TFailure> Acc<TSuccess, TFailure>
            (this Result<TSuccess, TFailure> input,
            Func<Result<TSuccess, TFailure>, Result<TSuccess, TFailure>> function)
        {
            var _ = function(input);
            if (input.Success)
            {
                return _;
            }
            input.Messages.AddRange(_.Messages);
            return new Failure<TSuccess, TFailure>(input.Messages);
        }

        public static Result<TSuccess, TFailure> Acc<TSuccess, TFailure>
            (this TSuccess input,
            Func<TSuccess, Result<TSuccess, TFailure>> function)
        {
            return function(input);
        }
    }
}
