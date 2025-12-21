using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestsCore
{
    [TestClass]
    public class FactoryMethodsTest
    {
        [TestMethod]
        public void SuccessResult_ShouldCreateSuccessfulResult()
        {
            var person = new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            };

            var result = Result<Person, string>.SuccessResult(person);

            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(person, result.Value);
            Assert.IsEmpty(result.Messages);
        }

        [TestMethod]
        public void FailureResult_WithSingleMessage_ShouldCreateFailedResult()
        {
            var result = Result<Person, string>.FailureResult("Validation failed");

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.IsFailure);
            Assert.HasCount(1, result.Messages);
            Assert.Contains("Validation failed", result.Messages);
        }

        [TestMethod]
        public void FailureResult_WithMultipleMessages_ShouldCreateFailedResult()
        {
            var messages = new List<string> { "Error 1", "Error 2", "Error 3" };
            var result = Result<Person, string>.FailureResult(messages);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.IsFailure);
            Assert.HasCount(3, result.Messages);
            Assert.Contains("Error 1", result.Messages);
            Assert.Contains("Error 2", result.Messages);
            Assert.Contains("Error 3", result.Messages);
        }

        [TestMethod]
        public void SuccessResult_ViaConstructor_ShouldStillWork()
        {
            var person = new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            };

            var result = new Success<Person, string>(person);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(person, result.Value);
        }

        [TestMethod]
        public void FailureResult_ViaConstructor_ShouldStillWork()
        {
            var result = new Failure<Person, string>("Error message");

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.IsFailure);
            Assert.Contains("Error message", result.Messages);
        }

        [TestMethod]
        public void IsFailure_ShouldBeOppositeOfSuccess()
        {
            var success = Result<Person, string>.SuccessResult(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var failure = Result<Person, string>.FailureResult("Error");

            Assert.IsTrue(success.Success);
            Assert.IsFalse(success.IsFailure);

            Assert.IsFalse(failure.Success);
            Assert.IsTrue(failure.IsFailure);
        }
    }
}
