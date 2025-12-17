using Mahamudra.Core.Errors;
using Mahamudra.Result.Core.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Mahamudra.Guard
{
    public static class GuardExtensions
    {
        #region isnull

        private static Result<T, Error> IsNull<T>(T input, string parameter) where T : class
        {
            if (input == null)
                return new Failure<T, Error>(new ValidationError(422, $"Required input {parameter} was null."));
            else
                return new Success<T, Error>(input);
        }
        public static Result<object, Error> IsNull(this IGuard guard, object input, string parameter)
        {
            return IsNull(input, parameter);
        }
        #endregion

        #region isnullorempty
        private static Result<string, Error> IsEmpty(string input, string parameter)
        {
            if (input == String.Empty)
                return new Failure<string, Error>(new ValidationError(422, $"Required input {parameter} was empty."));
            else
                return new Success<string, Error>(input);
        }
        public static Result<string, Error> IsNullOrEmpty(this IGuard guard, string input, string parameter)
        {
            var _ = IsNull(input, parameter);
            if (_ is Failure<string, Error>)
                return _;
            return IsEmpty(input, parameter);
        }

        public static Result<string, Error> IsNullOrEmptyAcc(this IGuard guard, string input, string parameter)
        {
            return
                IsNull(input, parameter)
                .Acc(_ => IsEmpty(input, parameter));
        }

        private static Result<IEnumerable<T>, Error> IsEmpty<T>(IEnumerable<T> input, string parameter)
        {
            if (!input.Any())
                return new Failure<IEnumerable<T>, Error>(new ValidationError(422, $"Required input {parameter} was empty."));
            else
                return new Success<IEnumerable<T>, Error>(input);
        }
        public static Result<IEnumerable<T>, Error> IsNullOrEmpty<T>(this IGuard guard, IEnumerable<T> input, string parameter)
        {
            var _ = IsNull(input, parameter);
            if (_ is Failure<string, Error>)
                return _;
            return IsEmpty(input, parameter);
        }


        public static Result<IEnumerable<T>, Error> IsNullOrEmptyAcc<T>(this IGuard guard, IEnumerable<T> input, string parameter)
        {
            return
                IsNull(input, parameter)
                .Acc(_ => IsEmpty(input, parameter));
        }
        #endregion


        #region isnullorwhitespace 
        private static Result<string, Error> IsWhiteSpace(string input, string parameter)
        {
            if (String.IsNullOrWhiteSpace(input))
                return new Failure<string, Error>(new ValidationError(422, $"Required input {parameter} was empty."));
            else
                return new Success<string, Error>(input);
        }

        public static Result<string, Error> IsNullOrWhiteSpace(this IGuard guard, string input, string parameter)
        {
            var _ = IsNullOrEmpty(guard, input, parameter);
            if (_ is Failure<string, Error>)
                return _;
            return IsWhiteSpace(input, parameter);
        }

        public static Result<string, Error> NullOrWhiteSpaceAcc(this IGuard guard, string input, string parameter)
        {
            return
                IsNullOrEmptyAcc(guard, input, parameter)
                .Acc(_ => IsWhiteSpace(input, parameter));
        }
        #endregion

        #region isoutofrange

        private static Result<T, Error> IsOutOfRange<T>(T input, string parameter, T rangeFrom, T rangeTo)
        {
            Comparer<T> comparer = Comparer<T>.Default;

            if (comparer.Compare(rangeFrom, rangeTo) > 0)
                return new Failure<T, Error>(new ValidationError(422, $"{nameof(rangeFrom)} should be less or equal than {nameof(rangeTo)}"));

            if (comparer.Compare(input, rangeFrom) < 0 || comparer.Compare(input, rangeTo) > 0)
                return new Failure<T, Error>(new ValidationError(422, $"Input {parameter} was out of range."));

            return new Success<T, Error>(input);
        }

        private static Result<T, Error> IsOutOfRangeAcc<T>(T input, string parameter, T rangeFrom, T rangeTo)
        {
            List<Error> errors = null;
            Comparer<T> comparer = Comparer<T>.Default;

            if (comparer.Compare(rangeFrom, rangeTo) > 0)
            {
                errors = new List<Error>();
                errors.Add(new ValidationError(422, $"{nameof(rangeFrom)} should be less or equal than {nameof(rangeTo)}"));
            }


            if (comparer.Compare(input, rangeFrom) < 0 || comparer.Compare(input, rangeTo) > 0)
            {
                _ = errors ?? new List<Error>();
                errors.Add(new ValidationError(422, $"Input {parameter} was out of range."));
                return new Failure<T, Error>(errors);
            }

            return new Success<T, Error>(input);
        }

        public static Result<int, Error> IsOutOfRange(this IGuard guard, int input, string parameter, int rangeFrom, int rangeTo)
        {
            return IsOutOfRange<int>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<int, Error> IsOutOfRangeAcc(this IGuard guard, int input, string parameter, int rangeFrom, int rangeTo)
        {
            return IsOutOfRangeAcc<int>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<DateTime, Error> IsOutOfRange(this IGuard guard, DateTime input, string parameter, DateTime rangeFrom, DateTime rangeTo)
        {
            return IsOutOfRange<DateTime>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<DateTime, Error> IsOutOfRangeAcc(this IGuard guard, DateTime input, string parameter, DateTime rangeFrom, DateTime rangeTo)
        {
            return IsOutOfRangeAcc<DateTime>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<DateTime, Error> IsOutOfSQLDateRange(this IGuard guard, DateTime input, string parameter, DateTime rangeFrom, DateTime rangeTo)
        {
            const long sqlMinDateTicks = 552877920000000000;
            const long sqlMaxDateTicks = 3155378975999970000;
            return IsOutOfRange<DateTime>(input, parameter, new DateTime(sqlMinDateTicks), new DateTime(sqlMaxDateTicks));
        }

        public static Result<decimal, Error> IsOutOfRange(this IGuard guard, decimal input, string parameter, decimal rangeFrom, decimal rangeTo)
        {
            return IsOutOfRange<decimal>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<decimal, Error> IsOutOfRangeAcc(this IGuard guard, decimal input, string parameter, decimal rangeFrom, decimal rangeTo)
        {
            return IsOutOfRangeAcc<decimal>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<short, Error> IsOutOfRange(this IGuard guard, short input, string parameter, short rangeFrom, short rangeTo)
        {
            return IsOutOfRange<short>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<short, Error> IsOutOfRangeAcc(this IGuard guard, short input, string parameter, short rangeFrom, short rangeTo)
        {
            return IsOutOfRangeAcc<short>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<double, Error> IsOutOfRange(this IGuard guard, double input, string parameter, double rangeFrom, double rangeTo)
        {
            return IsOutOfRange<double>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<double, Error> IsOutOfRangeAcc(this IGuard guard, double input, string parameter, double rangeFrom, double rangeTo)
        {
            return IsOutOfRangeAcc<double>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<float, Error> IsOutOfRange(this IGuard guard, float input, string parameter, float rangeFrom, float rangeTo)
        {
            return IsOutOfRange<float>(input, parameter, rangeFrom, rangeTo);
        }

        public static Result<float, Error> IsOutOfRangeAcc(this IGuard guard, float input, string parameter, float rangeFrom, float rangeTo)
        {
            return IsOutOfRangeAcc<float>(input, parameter, rangeFrom, rangeTo);
        } 

        public static Result<int, Error> IsOutOfRange<T>(this IGuard guard, int input, string parameter) where T : struct, Enum
        {
            return IsOutOfRange<T>(guard, input, parameter);
        }

        public static Result<T, Error> IsOutOfRange<T>(this IGuard guard, T input, string parameter) where T : struct, Enum
        {
            if (Enum.IsDefined(typeof(T), input))
                return new Success<T, Error>(input);
            return new Failure<T, Error>(new ValidationError(422, $"Required input {parameter} was not a valid enum value for {typeof(T)}."));
        }

        #endregion

        #region iszero
        private static Result<T, Error> IsZero<T>(T input, string parameter) where T : struct
        {

            if (EqualityComparer<T>.Default.Equals(input, default(T)))
                return new Failure<T, Error>(new ValidationError(422, $"Required input {parameter} cannot be zero."));

            return new Success<T, Error>(input);
        }

        public static Result<int, Error> IsZero(this IGuard guard, int input, string parameter)
        {
            return IsZero<int>(input, parameter);
        }

        public static Result<long, Error> IsZero(this IGuard guard, long input, string parameter)
        {
            return IsZero<long>(input, parameter);
        }

        public static Result<decimal, Error> IsZero(this IGuard guard, decimal input, string parameter)
        {
            return IsZero<decimal>(input, parameter);
        }

        public static Result<float, Error> IsZero(this IGuard guard, float input, string parameter)
        {
            return IsZero<float>(input, parameter);
        }

        public static Result<double, Error> IsZero(this IGuard guard, double input, string parameter)
        {
            return IsZero<double>(input, parameter);
        }
        #endregion

        #region isnegative
        private static Result<T, Error> IsNegative<T>(T input, string parameter) where T : struct, IComparable
        {

            if (input.CompareTo(default(T)) < 0)
                return new Failure<T, Error>(new ValidationError(422, $"Required input {parameter} cannot be negative."));

            return new Success<T, Error>(input);
        }

        public static Result<int, Error> IsNegative(this IGuard guard, int input, string parameter)
        {
            return IsNegative<int>(input, parameter);
        }

        public static Result<long, Error> IsNegative(this IGuard guard, long input, string parameter)
        {
            return IsNegative<long>(input, parameter);
        }
        public static Result<decimal, Error> IsNegative(this IGuard guard, decimal input, string parameter)
        {
            return IsNegative<decimal>(input, parameter);
        }

        public static Result<float, Error> IsNegative(this IGuard guard, float input, string parameter)
        {
            return IsNegative<float>(input, parameter);
        }

        public static Result<double, Error> IsNegative(this IGuard guard, double input, string parameter)
        {
            return IsNegative<double>(input, parameter);
        }
        #endregion

        #region isnegativeorzero
        private static Result<T, Error> IsNegativeOrZero<T>(T input, string parameter) where T : struct, IComparable
        {
            if (input.CompareTo(default(T)) <= 0)
                return new Failure<T, Error>(new ValidationError(422, $"Required input {parameter} cannot be zero or negative."));

            return new Success<T, Error>(input);
        }

        public static Result<int, Error> IsNegativeOrZero(this IGuard guard, int input, string parameter)
        {
            return IsNegativeOrZero<int>(input, parameter);
        }

        public static Result<long, Error> IsNegativeOrZero(this IGuard guard, long input, string parameter)
        {
            return IsNegativeOrZero<long>(input, parameter);
        }

        public static Result<decimal, Error> IsNegativeOrZero(this IGuard guard, decimal input, string parameter)
        {
            return IsNegativeOrZero<decimal>(input, parameter);
        }

        public static Result<float, Error> IsNegativeOrZero(this IGuard guard, float input, string parameter)
        {
            return IsNegativeOrZero<float>(input, parameter);
        }

        public static Result<double, Error> IsNegativeOrZero(this IGuard guard, double input, string parameter)
        {
            return IsNegativeOrZero<double>(input, parameter);
        }
        #endregion

        public static Result<T, Error> IsDefault<T>(this IGuard guard, [AllowNull, NotNull] T input, string parameter)  
        {
            if (EqualityComparer<T>.Default.Equals(input, default(T)!))
                return new Failure<T, Error>(new ValidationError(422, $"Input [{parameter}] is default value for type {typeof(T).Name}"));

            return new Success<T, Error>(input);
        }
    }
}
