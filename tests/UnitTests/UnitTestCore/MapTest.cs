using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsCore
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void Map_SuccessResult_ShouldTransformValue()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var nameResult = person.Map(p => p.Name);

            Assert.IsTrue(nameResult.Success);
            Assert.AreEqual("John", nameResult.Value);
        }

        [TestMethod]
        public void Map_FailureResult_ShouldPassThroughError()
        {
            var error = new Failure<Person, string>("Validation failed");

            var nameResult = error.Map(p => p.Name);

            Assert.IsFalse(nameResult.Success);
            Assert.IsTrue(nameResult.IsFailure);
            Assert.Contains("Validation failed", nameResult.Messages);
        }

        [TestMethod]
        public void Map_ChainMultipleTransformations_ShouldWork()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John Doe",
                Email = "john@example.com",
                Age = 30
            });

            var result = person
                .Map(p => p.Name)
                .Map(name => name.ToUpper())
                .Map(name => name.Replace(" ", "_"));

            Assert.IsTrue(result.Success);
            Assert.AreEqual("JOHN_DOE", result.Value);
        }

        [TestMethod]
        public void Map_TransformToCompletelyDifferentType_ShouldWork()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var ageResult = person.Map(p => p.Age);

            Assert.IsTrue(ageResult.Success);
            Assert.AreEqual(30, ageResult.Value);
        }

        [TestMethod]
        public void Map_AfterFailure_ShouldNotExecuteMapper()
        {
            var error = new Failure<Person, string>("Error");
            var mapperWasCalled = false;

            var result = error.Map(p =>
            {
                mapperWasCalled = true;
                return p.Name;
            });

            Assert.IsFalse(mapperWasCalled);
            Assert.IsFalse(result.Success);
        }
    }
}
