using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Result.Core;
using Mahamudra.Result.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsGuard
{
    [TestClass]
    public class GuardCheckFubar
    {
        [TestMethod]
        public void IsFubar_ShouldFailValidation_True()
        {
            var check = Guard.Check.IsFubar("fubar", "string");
            Assert.IsTrue(check is Failure<Option<string>, Error>);
        }

        [TestMethod]
        public void IsFubar_ShouldSuccedNull_True()
        {
            var check = Guard.Check.IsFubar(null, "string");
            Assert.IsTrue(check is Success<Option<string>, Error>);
        }

        [TestMethod]
        public void IsFubar_ShouldSuccedAnything_True()
        {
            var check = Guard.Check.IsFubar("anything", "string");
            Assert.IsTrue(check is Success<Option<string>, Error>);
        }
    }  
}
