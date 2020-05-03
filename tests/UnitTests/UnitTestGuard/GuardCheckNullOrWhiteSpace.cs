using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Result.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsGuard
{
    [TestClass]
    public class GuardCheckNullOrWhiteSpace
    { 
        [DataTestMethod]
        [DataRow("k")]
        [DataRow("any text")]
        [DataRow(" leading whitespaced")]
        [DataRow("trailing whitespaced ")]
        public void IsNullOrWhiteSpace_ShouldSucced_True(string value)
        {
            var check = Guard.Check.IsNullOrWhiteSpace(value, "string");
            Assert.IsTrue(check is Success<string, Error>);
        }

        [DataTestMethod]
        [DataRow("7")]
        public void IsNullOrWhiteSpace_ShouldSucced2_True(string value)
        {
            var check = Guard.Check.IsNullOrWhiteSpace(value, "aNumericString");
            Assert.IsTrue(check is Success<string, Error>);
        }

        [DataTestMethod]
        [DataRow("7")]
        public void IsNullOrWhiteSpace_Failure_True(string value)
        {
            var check = Guard.Check.IsNullOrWhiteSpace(null, "null");
            Assert.IsTrue(check is Failure<string, Error>);
        }

    }
}
