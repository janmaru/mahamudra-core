using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Core.Patterns;
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

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailIntZero_True()
        {
            var check = Guard.Check.IsNegativeOrZero(0, "intZero");
            Assert.IsTrue(check is Failure<int, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailLongZero_True()
        {
            var check = Guard.Check.IsNegativeOrZero(0L, "longZero");
            Assert.IsTrue(check is Failure<long, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailDecimalZero_True()
        {
            var check = Guard.Check.IsNegativeOrZero(0M, "decimalZero");
            Assert.IsTrue(check is Failure<decimal, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailFloatZero_True()
        {
            var check = Guard.Check.IsNegativeOrZero(0f, "floatZero");
            Assert.IsTrue(check is Failure<float, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailDoubleZero_True()
        {
            var check = Guard.Check.IsNegativeOrZero(0.0, "doubleZero");
            Assert.IsTrue(check is Failure<double, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailIntNegative_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-1, "intNegative");
            Assert.IsTrue(check is Failure<int, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailIntNegative2_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-29, "intNegative");
            Assert.IsTrue(check is Failure<int, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailLongNegative_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-1L, "longNegative");
            Assert.IsTrue(check is Failure<long, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailLongNegative2_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-666L, "longNegative");
            Assert.IsTrue(check is Failure<long, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailDecimalNegative_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-1M, "decimalNegative");
            Assert.IsTrue(check is Failure<decimal, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailDecimalNegative2_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-616M, "decimalNegative");
            Assert.IsTrue(check is Failure<decimal, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailFloatNegative_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-1f, "floatNegative");
            Assert.IsTrue(check is Failure<float, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailFloatNegative2_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-666f, "floatNegative");
            Assert.IsTrue(check is Failure<float, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailDoubleNegative_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-1.0, "doubleNegative");
            Assert.IsTrue(check is Failure<double, Error>);
        }

        [TestMethod]
        public void IsNegativeOrZero_ShouldFailDoubleNegative2_True()
        {
            var check = Guard.Check.IsNegativeOrZero(-616.00, "doubleNegative");
            Assert.IsTrue(check is Failure<double, Error>);
        }
    }
}


