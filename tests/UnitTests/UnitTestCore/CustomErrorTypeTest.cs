using Mahamudra.Core.Patterns;
using Mahamudra.Core.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestsCore
{
    /// <summary>
    /// Custom error class for domain-specific errors
    /// </summary>
    public class DomainError
    {
        public string Code { get; }
        public string Message { get; }
        public string Details { get; }

        public DomainError(string code, string message, string details = "")
        {
            Code = code;
            Message = message;
            Details = details;
        }

        public static DomainError InvalidAge =>
            new DomainError("INVALID_AGE", "Age must be between 0 and 150");

        public static DomainError InvalidEmail =>
            new DomainError("INVALID_EMAIL", "Email format is invalid");

        public static DomainError InvalidName =>
            new DomainError("INVALID_NAME", "Name cannot be empty");
    }

    [TestClass]
    public class CustomErrorTypeTest
    {
        #region Tests with Exception as error type

        [TestMethod]
        public void Result_WithException_SuccessCase_ShouldWork()
        {
            var person = new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            };

            var result = new Success<Person, Exception>(person);

            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual("John", result.Value.Name);
        }

        [TestMethod]
        public void Result_WithException_FailureCase_ShouldContainException()
        {
            var exception = new InvalidOperationException("Person validation failed");
            var result = new Failure<Person, Exception>(exception);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.IsFailure);
            Assert.HasCount(1, result.Messages);
            Assert.IsInstanceOfType(result.Messages[0], typeof(InvalidOperationException));
            Assert.AreEqual("Person validation failed", result.Messages[0].Message);
        }

        [TestMethod]
        public void Result_WithException_UsingFactoryMethod()
        {
            var exception = new ArgumentException("Invalid age parameter", "age");
            var result = Result<Person, Exception>.FailureResult(exception);

            var errorMessage = result.Match(
                onSuccess: p => "",
                onFailure: errors => errors[0].Message
            );

            Assert.AreEqual("Invalid age parameter (Parameter 'age')", errorMessage);
        }

        [TestMethod]
        public void Result_WithException_BindChaining()
        {
            var person = new Person { Name = "John", Email = "john@example.com", Age = 30 };

            Result<Person, Exception> ValidateAge(Person p)
            {
                if (p.Age < 18)
                    return Result<Person, Exception>.FailureResult(
                        new ValidationException("Must be 18 or older"));
                return Result<Person, Exception>.SuccessResult(p);
            }

            Result<Person, Exception> ValidateEmail(Person p)
            {
                if (string.IsNullOrEmpty(p.Email))
                    return Result<Person, Exception>.FailureResult(
                        new ValidationException("Email is required"));
                return Result<Person, Exception>.SuccessResult(p);
            }

            var result = Result<Person, Exception>.SuccessResult(person)
                .Bind(ValidateAge)
                .Bind(ValidateEmail);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void Result_WithException_BindChaining_ShouldFailOnFirstError()
        {
            var person = new Person { Name = "John", Email = "", Age = 15 };

            Result<Person, Exception> ValidateAge(Person p)
            {
                if (p.Age < 18)
                    return Result<Person, Exception>.FailureResult(
                        new ValidationException("Must be 18 or older"));
                return Result<Person, Exception>.SuccessResult(p);
            }

            Result<Person, Exception> ValidateEmail(Person p)
            {
                if (string.IsNullOrEmpty(p.Email))
                    return Result<Person, Exception>.FailureResult(
                        new ValidationException("Email is required"));
                return Result<Person, Exception>.SuccessResult(p);
            }

            var result = Result<Person, Exception>.SuccessResult(person)
                .Bind(ValidateAge)
                .Bind(ValidateEmail); // This won't execute due to age failure

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Must be 18 or older", result.Messages[0].Message);
        }

        #endregion

        #region Tests with DomainError as error type

        [TestMethod]
        public void Result_WithDomainError_SuccessCase_ShouldWork()
        {
            var person = new Person
            {
                Name = "Jane",
                Email = "jane@example.com",
                Age = 25
            };

            var result = new Success<Person, DomainError>(person);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Jane", result.Value.Name);
        }

        [TestMethod]
        public void Result_WithDomainError_FailureCase_ShouldContainError()
        {
            var result = new Failure<Person, DomainError>(DomainError.InvalidAge);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("INVALID_AGE", result.Messages[0].Code);
            Assert.AreEqual("Age must be between 0 and 150", result.Messages[0].Message);
        }

        [TestMethod]
        public void Result_WithDomainError_MultipleErrors()
        {
            var errors = new List<DomainError>
            {
                DomainError.InvalidName,
                DomainError.InvalidEmail,
                DomainError.InvalidAge
            };

            var result = new Failure<Person, DomainError>(errors);

            Assert.IsFalse(result.Success);
            Assert.HasCount(3, result.Messages);

            var errorCodes = result.Match(
                onSuccess: p => new List<string>(),
                onFailure: errs =>
                {
                    var codes = new List<string>();
                    foreach (var err in errs)
                        codes.Add(err.Code);
                    return codes;
                }
            );

            Assert.Contains("INVALID_NAME", errorCodes);
            Assert.Contains("INVALID_EMAIL", errorCodes);
            Assert.Contains("INVALID_AGE", errorCodes);
        }

        [TestMethod]
        public void Result_WithDomainError_BindChaining()
        {
            var person = new Person { Name = "John", Email = "john@example.com", Age = 30 };

            Result<Person, DomainError> ValidateName(Person p)
            {
                if (string.IsNullOrWhiteSpace(p.Name))
                    return Result<Person, DomainError>.FailureResult(DomainError.InvalidName);
                return Result<Person, DomainError>.SuccessResult(p);
            }

            Result<Person, DomainError> ValidateEmail(Person p)
            {
                if (!p.Email.Contains("@"))
                    return Result<Person, DomainError>.FailureResult(DomainError.InvalidEmail);
                return Result<Person, DomainError>.SuccessResult(p);
            }

            Result<Person, DomainError> ValidateAge(Person p)
            {
                if (p.Age < 0 || p.Age > 150)
                    return Result<Person, DomainError>.FailureResult(DomainError.InvalidAge);
                return Result<Person, DomainError>.SuccessResult(p);
            }

            var result = Result<Person, DomainError>.SuccessResult(person)
                .Bind(ValidateName)
                .Bind(ValidateEmail)
                .Bind(ValidateAge);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void Result_WithDomainError_MapToDto()
        {
            var person = new Person { Name = "John", Email = "john@example.com", Age = 30 };

            var result = Result<Person, DomainError>.SuccessResult(person)
                .Map(p => new { p.Name, p.Email })
                .Map(dto => $"{dto.Name} ({dto.Email})");

            var displayString = result.Match(
                onSuccess: str => str,
                onFailure: errors => $"Error: {errors[0].Code}"
            );

            Assert.AreEqual("John (john@example.com)", displayString);
        }

        [TestMethod]
        public void Result_WithDomainError_TapForLogging()
        {
            var person = new Person { Name = "John", Email = "john@example.com", Age = 30 };
            var loggedPerson = "";

            var result = Result<Person, DomainError>.SuccessResult(person)
                .Tap(p => loggedPerson = $"Processing person: {p.Name}")
                .Map(p => p.Age);

            Assert.AreEqual("Processing person: John", loggedPerson);
            Assert.AreEqual(30, result.Value);
        }

        #endregion

        #region Tests with Error record (from Core.Errors)

        [TestMethod]
        public void Result_WithErrorRecord_UsingValidation()
        {
            var person = new Person { Name = "", Email = "test@test.com", Age = 25 };

            Result<Person, Error> ValidatePerson(Person p)
            {
                if (string.IsNullOrWhiteSpace(p.Name))
                    return Result<Person, Error>.FailureResult(
                        Error.Validation("Name is required", "Name cannot be empty"));

                if (string.IsNullOrWhiteSpace(p.Email))
                    return Result<Person, Error>.FailureResult(
                        Error.Validation("Email is required", "Email cannot be empty"));

                if (p.Age < 18)
                    return Result<Person, Error>.FailureResult(
                        Error.BadRequest("Age requirement not met", "Must be 18 or older"));

                return Result<Person, Error>.SuccessResult(p);
            }

            var result = ValidatePerson(person);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(400, result.Messages[0].Code);
            Assert.AreEqual("Name is required", result.Messages[0].Description);
        }

        [TestMethod]
        public void Result_WithErrorRecord_DifferentErrorTypes()
        {
            Result<Person, Error> FindPerson(int id)
            {
                if (id <= 0)
                    return Result<Person, Error>.FailureResult(
                        Error.BadRequest("Invalid ID", "ID must be positive"));

                if (id == 999)
                    return Result<Person, Error>.FailureResult(
                        Error.NotFound("Person not found", $"No person with ID {id}"));

                return Result<Person, Error>.SuccessResult(new Person
                {
                    Name = "John",
                    Email = "john@example.com",
                    Age = 30
                });
            }

            var notFoundResult = FindPerson(999);
            var badRequestResult = FindPerson(-1);
            var successResult = FindPerson(1);

            Assert.AreEqual(404, notFoundResult.Messages[0].Code);
            Assert.AreEqual(400, badRequestResult.Messages[0].Code);
            Assert.IsTrue(successResult.Success);
        }

        [TestMethod]
        public void Result_WithValidationError_FieldLevelErrors()
        {
            var fieldErrors = new Dictionary<string, string[]>
            {
                ["Name"] = new[] { "Name is required", "Name must be at least 2 characters" },
                ["Email"] = new[] { "Email is required", "Email format is invalid" },
                ["Age"] = new[] { "Age must be 18 or older" }
            };

            var validationError = ValidationError.ForFields(fieldErrors, "Person validation failed");
            var result = new Failure<Person, ValidationError>(validationError);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Messages[0].HasFieldErrors);
            Assert.HasCount(3, result.Messages[0].FieldErrors);
            Assert.HasCount(2, result.Messages[0].FieldErrors["Name"]);
        }

        #endregion
    }

    /// <summary>
    /// Simple validation exception for testing
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
