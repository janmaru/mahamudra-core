using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Result.Core;
using Mahamudra.Result.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsGuard
{
    [TestClass]
    public class GuardCheckNegativeOrZero
    { 
        [TestMethod]
        public void IsNegativeOrZero_ShouldSuccedIntOne_True()
        {
            var check = Guard.Check.IsNegativeOrZero(1, "intPositive");
            Assert.IsTrue(check is Success<int, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldSuccedLongOne_True()
        {
            var check = Guard.Check.IsNegativeOrZero(1L, "longPositive");
            Assert.IsTrue(check is Success<long, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldSuccedDecimalOne_True()
        {
            var check = Guard.Check.IsNegativeOrZero(1.0M, "decimalPositive");
            Assert.IsTrue(check is Success<decimal, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldSuccedFloatOne_True()
        {
            var check = Guard.Check.IsNegativeOrZero(1.0f, "floatPositive");
            Assert.IsTrue(check is Success<float, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldSuccedDoubleOne_True()
        {
            var check = Guard.Check.IsNegativeOrZero(1.0, "doublePositive");
            Assert.IsTrue(check is Success<double, Error>);
        }
    }  
}
 
           
       