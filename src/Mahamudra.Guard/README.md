# Mahamudra.Guard

A simple package with guard extensions that uses the Railway Oriented Programming, a powerful **Functional Programming** pattern.

## Usage

```c#
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
```

Extend:

```c#
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
            var _ = Guard.Check.IsNullOrEmpty(input, parameter);
            if (_ is Failure<string, Error>)
                return new Success<Option<string>, Error>(Option<string>.None);
            else
                return IsFubar(input, parameter);
        }
    }
```

## Test

```c#
    [TestMethod]
    public void IsFubar_ShouldFailValidation_True()
    {
        var check = Guard.Check.IsFubar("fubar", "string");
        Assert.IsTrue(check is Failure<Option<string>, Error>);
    }

    [TestMethod]
    public void IsFubar_ShouldSuccedNull_True()
    {
        var check = Guard.Check.IsFubar(null, "string");
        Assert.IsTrue(check is Success<Option<string>, Error>);
    }
```
