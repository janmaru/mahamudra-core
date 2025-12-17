# Mahamudra.Core

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
