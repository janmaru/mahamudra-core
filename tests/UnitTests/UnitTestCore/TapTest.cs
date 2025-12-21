using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsCore
{
    [TestClass]
    public class TapTest
    {
        [TestMethod]
        public void Tap_OnSuccess_ShouldExecuteSideEffect()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var sideEffectExecuted = false;
            var capturedName = string.Empty;

            var result = person.Tap(p =>
            {
                sideEffectExecuted = true;
                capturedName = p.Name;
            });

            Assert.IsTrue(sideEffectExecuted);
            Assert.AreEqual("John", capturedName);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("John", result.Value.Name);
        }

        [TestMethod]
        public void Tap_OnFailure_ShouldNotExecuteSideEffect()
        {
            var error = new Failure<Person, string>("Validation failed");
            var sideEffectExecuted = false;

            var result = error.Tap(p =>
            {
                sideEffectExecuted = true;
            });

            Assert.IsFalse(sideEffectExecuted);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void Tap_ReturnsOriginalResult_ShouldAllowChaining()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var callOrder = string.Empty;

            var result = person
                .Tap(p => callOrder += "1")
                .Map(p => p.Name)
                .Tap(name => callOrder += "2")
                .Map(name => name.ToUpper());

            Assert.AreEqual("12", callOrder);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("JOHN", result.Value);
        }

        [TestMethod]
        public void Tap_InMiddleOfPipeline_ShouldExecuteAtCorrectTime()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            string loggedName = null;

            var result = person
                .Map(p => p.Name)
                .Tap(name => loggedName = name)
                .Map(name => name.ToUpper());

            Assert.AreEqual("John", loggedName);
            Assert.AreEqual("JOHN", result.Value);
        }
    }
}
