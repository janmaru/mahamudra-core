using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestsCore
{
    [TestClass]
    public class MatchTest
    {
        [TestMethod]
        public void Match_SuccessResult_ShouldExecuteOnSuccessPath()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var result = person.Match(
                onSuccess: p => $"Hello {p.Name}",
                onFailure: errors => "Error occurred"
            );

            Assert.AreEqual("Hello John", result);
        }

        [TestMethod]
        public void Match_FailureResult_ShouldExecuteOnFailurePath()
        {
            var error = new Failure<Person, string>("Validation failed");

            var result = error.Match(
                onSuccess: p => $"Hello {p.Name}",
                onFailure: errors => $"Error: {string.Join(", ", errors)}"
            );

            Assert.Contains("Validation failed", result);
        }

        [TestMethod]
        public void Match_WithMultipleErrors_ShouldReturnAllErrors()
        {
            var errors = new List<string> { "Error 1", "Error 2", "Error 3" };
            var result = new Failure<Person, string>(errors);

            var errorCount = result.Match(
                onSuccess: p => 0,
                onFailure: errs => errs.Count
            );

            Assert.AreEqual(3, errorCount);
        }

        [TestMethod]
        public void Match_CanReturnDifferentTypes()
        {
            var success = new Success<Person, string>(new Person
            {
                Name = "Jane",
                Email = "jane@example.com",
                Age = 25
            });

            var age = success.Match(
                onSuccess: p => p.Age,
                onFailure: _ => -1
            );

            Assert.AreEqual(25, age);
        }
    }
}
