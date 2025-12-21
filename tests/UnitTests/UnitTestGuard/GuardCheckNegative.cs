using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsGuard
{
    [TestClass]
    public class GuardCheckNegative
    {
        [TestMethod]
        public void IsNegative_ShouldSuccedInt_True()
        {
            var check = Guard.Check.IsNegative(0, "intZero");
            Assert.IsTrue(check is Success<int, Error>);
        }

        [TestMethod]
        public void IsNegative_ShouldSuccedLong_True()
        {
            var check = Guard.Check.IsNegative(0L, "longZero");
            Assert.IsTrue(check is Success<long, Error>);
        }

        [TestMethod]
        public void IsNegative_ShouldSuccedDecimal_True()
        {
            var check = Guard.Check.IsNegative(0.0M, "decimalZero");
            Assert.IsTrue(check is Success<decimal, Error>);
        }

        [TestMethod]
        public void IsNegative_ShouldSuccedFloat_True()
        {
            var check = Guard.Check.IsNegative(0.0f, "floatZero");
            Assert.IsTrue(check is Success<float, Error>);
        }
        [TestMethod]
        public void IsNegative_ShouldSuccedDouble_True()
        {
            var check = Guard.Check.IsNegative(0.0, "doubleZero");
            Assert.IsTrue(check is Success<double, Error>);
        }
        [TestMethod]
        public void IsNegative_ShouldSuccedOneInt_True()
        {
            var check = Guard.Check.IsNegative(1, "intZero");
            Assert.IsTrue(check is Success<int, Error>);
        }
        [TestMethod]
        public void IsNegative_ShouldSuccedOneDecimal_True()
        {
            var check = Guard.Check.IsNegative(1.0M, "decimalZero");
            Assert.IsTrue(check is Success<decimal, Error>);
        }
        [TestMethod]
        public void IsNegative_ShouldSuccedOneFloat_True()
        {
            var check = Guard.Check.IsNegative(1.0f, "floatZero");
            Assert.IsTrue(check is Success<float, Error>);
        }
        [TestMethod]
        public void IsNegative_ShouldSuccedOneDouble_True()
        {
            var check = Guard.Check.IsNegative(1.0, "doubleZero");
            Assert.IsTrue(check is Success<double, Error>);
        }

        [TestMethod]
        public void IsNegative_ShouldFailMinusOneInt_True()
        {
            var check = Guard.Check.IsNegative(-1, "negative");
            Assert.IsTrue(check is Failure<int, Error>);
        }

        [TestMethod]
        public void IsNegative_ShouldFailMinusOneLong_True()
        {
            var check = Guard.Check.IsNegative(-1L, "negative");
            Assert.IsTrue(check is Failure<long, Error>);
        }

        [TestMethod]
        public void IsNegative_ShouldFailMinusOneDecimal_True()
        {
            var check = Guard.Check.IsNegative(-1.0M, "negative");
            Assert.IsTrue(check is Failure<decimal, Error>);
        }

        [TestMethod]
        public void IsNegative_ShouldFailMinusOneFloat_True()
        {
            var check = Guard.Check.IsNegative(-1.0f, "negative");
            Assert.IsTrue(check is Failure<float, Error>);
        }

        [TestMethod]
        public void IsNegative_ShouldFailMinusOneDouble_True()
        {
            var check = Guard.Check.IsNegative(-1.0, "negative");
            Assert.IsTrue(check is Failure<double, Error>);
        }
    }
}
