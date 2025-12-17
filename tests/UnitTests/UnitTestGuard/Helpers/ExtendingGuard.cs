using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Result.Core;
using Mahamudra.Result.Core.Patterns;

namespace UnitTestsGuard
{
    public static class FubarGuard
    {
        private static Result<Option<string>, Error> IsFubar(string input, string parameter)
        {
            if (input.ToLower() == "fubar")
                return new Failure<Option<string>, Error>(new ValidationError(422, $"Parameter {parameter} should not be f***ed up beyond all repair!"));
            else
                return new Success<Option<string>, Error>(Option<string>.Some(input));
        }

        public static Result<Option<string>, Error> IsFubar(this IGuard guard, string input, string parameter)
        {
            var result = Guard.Check.IsNullOrEmpty(input, parameter);
            if (result is Failure<string, Error>)
                return new Success<Option<string>, Error>(Option<string>.None);
            else
                return IsFubar(input, parameter); 
        }
    }
} 