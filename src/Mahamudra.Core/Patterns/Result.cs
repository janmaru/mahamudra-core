using Mahamudra.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamudra.Core.Patterns
{
    /// <summary>
    /// Represents the result of an operation that can succeed with a value or fail with messages.
    /// Implements the railway-oriented programming pattern.
    /// </summary>
    public abstract class Result<TSuccess, TMessage>
    {
        protected Result(TSuccess input)
        {
            this.Value = input ?? throw new ArgumentNullException(nameof(input));
            this.Success = true;
            this.Messages = Array.Empty<TMessage>();
        }

        protected Result(IList<TMessage> messages)
        {
            this.Messages = messages.IsNullOrEmpty()
                ? throw new ArgumentNullException(nameof(messages))
                : messages;
            this.Success = false;
            this.Value = default!;
        }

        protected Result(TMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            this.Messages = new List<TMessage> { message };
            this.Success = false;
            this.Value = default!;
        }

        public TSuccess Value { get; }
        public bool Success { get; }
        public bool IsFailure => !Success;
        public IList<TMessage> Messages { get; }

        /// <summary>Creates a successful result.</summary>
        public static Result<TSuccess, TMessage> SuccessResult(TSuccess value)
            => new ConcreteResult<TSuccess, TMessage>(value);

        /// <summary>Creates a failed result with a single message.</summary>
        public static Result<TSuccess, TMessage> FailureResult(TMessage message)
            => new ConcreteResult<TSuccess, TMessage>(message);

        /// <summary>Creates a failed result with multiple messages.</summary>
        public static Result<TSuccess, TMessage> FailureResult(IList<TMessage> messages)
            => new ConcreteResult<TSuccess, TMessage>(messages);

        /// <summary>
        /// Pattern match on the result. Forces handling both success and failure paths.
        /// </summary>
        public TOut Match<TOut>(
            Func<TSuccess, TOut> onSuccess,
            Func<IList<TMessage>, TOut> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            return Success ? onSuccess(Value) : onFailure(Messages);
        }

        /// <summary>
        /// Transforms the success value. If result is failure, passes through the messages.
        /// </summary>
        public Result<TOut, TMessage> Map<TOut>(Func<TSuccess, TOut> mapper)
        {
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));

            return Success
                ? Result<TOut, TMessage>.SuccessResult(mapper(Value))
                : Result<TOut, TMessage>.FailureResult(Messages);
        }

        /// <summary>
        /// Chains an operation that returns a Result. Enables railway switching.
        /// </summary>
        public Result<TOut, TMessage> Bind<TOut>(
            Func<TSuccess, Result<TOut, TMessage>> binder)
        {
            if (binder == null) throw new ArgumentNullException(nameof(binder));

            return Success
                ? binder(Value)
                : Result<TOut, TMessage>.FailureResult(Messages);
        }

        /// <summary>
        /// Executes a side effect if successful, then returns the original result.
        /// </summary>
        public Result<TSuccess, TMessage> Tap(Action<TSuccess> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (Success)
                action(Value);

            return this;
        }

        /// <summary>Async version of Map.</summary>
        public async Task<Result<TOut, TMessage>> MapAsync<TOut>(
            Func<TSuccess, Task<TOut>> mapper)
        {
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));

            return Success
                ? Result<TOut, TMessage>.SuccessResult(await mapper(Value))
                : Result<TOut, TMessage>.FailureResult(Messages);
        }

        /// <summary>Async version of Bind.</summary>
        public async Task<Result<TOut, TMessage>> BindAsync<TOut>(
            Func<TSuccess, Task<Result<TOut, TMessage>>> binder)
        {
            if (binder == null) throw new ArgumentNullException(nameof(binder));

            return Success
                ? await binder(Value)
                : Result<TOut, TMessage>.FailureResult(Messages);
        }

        /// <summary>
        /// Executes different actions based on success or failure, without returning a value.
        /// </summary>
        public void Switch(
            Action<TSuccess> onSuccess,
            Action<IList<TMessage>> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            if (Success)
                onSuccess(Value);
            else
                onFailure(Messages);
        }
    }

    /// <summary>
    /// Concrete sealed implementation of Result.
    /// </summary>
    internal sealed class ConcreteResult<TSuccess, TMessage> : Result<TSuccess, TMessage>
    {
        internal ConcreteResult(TSuccess input) : base(input) { }
        internal ConcreteResult(TMessage message) : base(message) { }
        internal ConcreteResult(IList<TMessage> messages) : base(messages) { }
    }
} 