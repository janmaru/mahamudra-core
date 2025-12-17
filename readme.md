# Core
![alt text](pawel-czerwinski.jpg "Mahamudra Core")
[Photo by Paweł Czerwiński on Unsplash]

A simple package that implements the Railway Oriented Programming, a powerful **Functional Programming** pattern.
## Usage

```c#
    public static Result<Person, string> CheckName(Result<Person, string> person)
    {
        if (string.IsNullOrWhiteSpace(person.Value.Name))
            return new Failure<Person, string>("Name should not be blank.");
        else
            return new Success<Person, string>(person.Value);
    }

    [TestMethod]
    public void Bind_ShouldFailValidateAge_True()
    {
        var result = person2
            .Bind(Validation.CheckName)
            .Bind(Validation.CheckEmail)
            .Bind(Validation.CheckAge);
        Assert.IsTrue(result.Messages.Contains("The age should be not inferior than 18."));
    }
```
## Test 

```c#
    public class Success
    {
        private Result<Person, string> person; 
        [TestInitialize]
        public void Init()
        {
            person = new Success<Person, string>(new Person
            {
                Email = "",
                Name = "Pippo",
                Age = 120
            });

        } 
        [TestMethod]
        public void Success_ShouldReturnSuccessFromResult_True()
        { 
            Assert.IsTrue(person is Success<Person, string>);
        } 
    }
```
```c#
    public class Failure
    {
        private Result<Person, string> error;

        [TestInitialize]
        public void Init()
        { 
            error = new Failure<Person, string>("This is an error!");
        }

 
        [TestMethod]
        public void Failure_ShouldReturnFailureFromResult_True()
        {
            Assert.IsTrue(error is Failure<Person, string>);
        }
    }
```
# Guard
![alt text](mitchell-ng-liang.jpg "Guard")
[Photo by Mitchell Ng Liang an on Unsplash]

A simple package with guard extensions that uses the the Railway Oriented Programming, a powerful **Functional Programming** pattern.

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