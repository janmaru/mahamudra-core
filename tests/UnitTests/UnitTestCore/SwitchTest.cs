using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestsCore
{
    [TestClass]
    public class SwitchTest
    {
        [TestMethod]
        public void Switch_OnSuccess_ShouldExecuteOnSuccessAction()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var successExecuted = false;
            var failureExecuted = false;

            person.Switch(
                onSuccess: p => successExecuted = true,
                onFailure: errors => failureExecuted = true
            );

            Assert.IsTrue(successExecuted);
            Assert.IsFalse(failureExecuted);
        }

        [TestMethod]
        public void Switch_OnFailure_ShouldExecuteOnFailureAction()
        {
            var error = new Failure<Person, string>("Validation failed");

            var successExecuted = false;
            var failureExecuted = false;

            error.Switch(
                onSuccess: p => successExecuted = true,
                onFailure: errors => failureExecuted = true
            );

            Assert.IsFalse(successExecuted);
            Assert.IsTrue(failureExecuted);
        }

        [TestMethod]
        public void Switch_CanAccessValue_InSuccessAction()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var capturedName = string.Empty;

            person.Switch(
                onSuccess: p => capturedName = p.Name,
                onFailure: _ => { }
            );

            Assert.AreEqual("John", capturedName);
        }

        [TestMethod]
        public void Switch_CanAccessErrors_InFailureAction()
        {
            var errors = new List<string> { "Error 1", "Error 2" };
            var result = new Failure<Person, string>(errors);

            var capturedErrorCount = 0;

            result.Switch(
                onSuccess: _ => { },
                onFailure: errs => capturedErrorCount = errs.Count
            );

            Assert.AreEqual(2, capturedErrorCount);
        }
    }
}
