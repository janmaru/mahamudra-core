using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsGuard
{
    [TestClass]
    public class GuardCheckZero
    {
        [TestMethod]
        public void IsZero_ShouldSuccedMinusOne_True()
        { 
            var check = Guard.Check.IsZero(-1, "minusOne");
            Assert.IsTrue(check is Success<int, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedIntMinValue_True()
        {
            var check = Guard.Check.IsZero(int.MinValue, "int.MinValue");
            Assert.IsTrue(check is Success<int, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedIntMaxValue_True()
        {
            var check = Guard.Check.IsZero(int.MaxValue, "int.MaxValue");
            Assert.IsTrue(check is Success<int, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedLongMinValue_True()
        {
            var check = Guard.Check.IsZero(long.MinValue, "long.MinValue");
            Assert.IsTrue(check is Success<long, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedLongMaxValue_True()
        {
            var check = Guard.Check.IsZero(long.MaxValue, "long.MaxValue");
            Assert.IsTrue(check is Success<long, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedDecimalMinValue_True()
        {
            var check = Guard.Check.IsZero(decimal.MinValue, "long.MinValue");
            Assert.IsTrue(check is Success<decimal, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedDecimalMaxValue_True()
        {
            var check = Guard.Check.IsZero(decimal.MinValue, "decimal.MaxValue");
            Assert.IsTrue(check is Success<decimal, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedFloatMinValue_True()
        {
            var check = Guard.Check.IsZero(float.MinValue, "float.MinValue");
            Assert.IsTrue(check is Success<float, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedFloatMaxValue_True()
        {
            var check = Guard.Check.IsZero(float.MaxValue, "float.MaxValue");
            Assert.IsTrue(check is Success<float, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedDoubleMinValue_True()
        {
            var check = Guard.Check.IsZero(double.MaxValue, "double.MinValue");
            Assert.IsTrue(check is Success<double, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldSuccedDoubleMaxValue_True()
        {
            var check = Guard.Check.IsZero(double.MaxValue, "double.MaxValue");
            Assert.IsTrue(check is Success<double, Error>);
        }

        [TestMethod]
        public void IsZero_ShouldFailIntZero_True()
        {
            var check = Guard.Check.IsZero(0, "zero");
            Assert.IsTrue(check is Failure<int, Error>);
        }

        [TestMethod]
        public void IsZero_ShouldFailLongZero_True()
        {
            var check = Guard.Check.IsZero(0L, "zero");
            Assert.IsTrue(check is Failure<long, Error>);
        }

        [TestMethod]
        public void IsZero_ShouldFailDecimalZero_True()
        {
            var check = Guard.Check.IsZero(0M, "zero");
            Assert.IsTrue(check is Failure<decimal, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldFailFloatZero_True()
        {
            var check = Guard.Check.IsZero(0.0f, "zero");
            Assert.IsTrue(check is Failure<float, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldFailDoubleZero_True()
        {
            var check = Guard.Check.IsZero(0.0, "zero");
            Assert.IsTrue(check is Failure<double, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldFailDefaultInt_True()
        {
            var check = Guard.Check.IsZero(default(int), "zero");
            Assert.IsTrue(check is Failure<int, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldFailDefaultLong_True()
        {
            var check = Guard.Check.IsZero(default(long), "zero");
            Assert.IsTrue(check is Failure<long, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldFailDefaultDecimal_True()
        {
            var check = Guard.Check.IsZero(default(decimal), "zero");
            Assert.IsTrue(check is Failure<decimal, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldFailDefaultFloat_True()
        {
            var check = Guard.Check.IsZero(default(float), "zero");
            Assert.IsTrue(check is Failure<float, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldFailDefaultDouble_True()
        {
            var check = Guard.Check.IsZero(default(double), "zero");
            Assert.IsTrue(check is Failure<double, Error>);
        }
        [TestMethod]
        public void IsZero_ShouldFailDecimalZero2_True()
        {
            var check = Guard.Check.IsZero(decimal.Zero, "zero");
            Assert.IsTrue(check is Failure<decimal, Error>);
        } 
    }
}
      
        