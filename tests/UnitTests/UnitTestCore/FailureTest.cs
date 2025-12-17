using Mahamudra.Result.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsCore
{
    [TestClass]
    public class FailureTest
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
} 