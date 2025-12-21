using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTestsCore
{
    [TestClass]
    public class AsyncTest
    {
        [TestMethod]
        public async Task MapAsync_OnSuccess_ShouldTransformValueAsync()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var result = await person.MapAsync(async p =>
            {
                await Task.Delay(10);
                return p.Name.ToUpper();
            });

            Assert.IsTrue(result.Success);
            Assert.AreEqual("JOHN", result.Value);
        }

        [TestMethod]
        public async Task MapAsync_OnFailure_ShouldPassThroughError()
        {
            var error = new Failure<Person, string>("Validation failed");

            var result = await error.MapAsync(async p =>
            {
                await Task.Delay(10);
                return p.Name;
            });

            Assert.IsFalse(result.Success);
            Assert.Contains("Validation failed", result.Messages);
        }

        [TestMethod]
        public async Task BindAsync_OnSuccess_ShouldChainAsyncOperation()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var result = await person.BindAsync(async p =>
            {
                await Task.Delay(10);
                if (p.Age >= 18)
                    return Result<Person, string>.SuccessResult(p);
                else
                    return Result<Person, string>.FailureResult("Must be 18 or older");
            });

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task BindAsync_OnFailure_ShouldPassThroughError()
        {
            var error = new Failure<Person, string>("Initial error");

            var result = await error.BindAsync(async p =>
            {
                await Task.Delay(10);
                return Result<Person, string>.SuccessResult(p);
            });

            Assert.IsFalse(result.Success);
            Assert.Contains("Initial error", result.Messages);
        }

        [TestMethod]
        public async Task BindAsync_CanReturnFailure_FromAsyncOperation()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 15
            });

            var result = await person.BindAsync(async p =>
            {
                await Task.Delay(10);
                if (p.Age >= 18)
                    return Result<Person, string>.SuccessResult(p);
                else
                    return Result<Person, string>.FailureResult("Must be 18 or older");
            });

            Assert.IsFalse(result.Success);
            Assert.Contains("Must be 18 or older", result.Messages);
        }

        [TestMethod]
        public async Task ChainAsyncOperations_ShouldWork()
        {
            var person = new Success<Person, string>(new Person
            {
                Name = "John",
                Email = "john@example.com",
                Age = 30
            });

            var result = await person
                .MapAsync(async p =>
                {
                    await Task.Delay(5);
                    return p.Name;
                })
                .ContinueWith(t => t.Result.MapAsync(async name =>
                {
                    await Task.Delay(5);
                    return name.ToUpper();
                }))
                .Unwrap();

            Assert.IsTrue(result.Success);
            Assert.AreEqual("JOHN", result.Value);
        }
    }
}
