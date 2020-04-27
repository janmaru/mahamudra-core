using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Result.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsGuard
{
    [TestClass]
    public class GuardCheckOutOfRange
    {

        [DataTestMethod]
        [DataRow(1, 1, 1)]
        [DataRow(1, 1, 3)]
        [DataRow(2, 1, 3)]
        [DataRow(3, 1, 3)]
        public void IsOutOfRange_ShouldSuccedInRangeInt_True(int input, int rangeFrom, int rangeTo)
        {
            var check = Guard.Check.IsOutOfRange(input, "int", rangeFrom, rangeTo);
            Assert.IsTrue(check is Success<int, Error>);
        }

        [DataTestMethod]
        [DataRow(-1, 1, 3)]
        [DataRow(0, 1, 3)]
        [DataRow(-2, 1, 3)]
        [DataRow(4, 1, 3)]
        public void IsOutOfRange_ShouldFailOutRangeInt_True(int input, int rangeFrom, int rangeTo)
        {
            var check = Guard.Check.IsOutOfRange(input, "int", rangeFrom, rangeTo);
            Assert.IsTrue(check is Failure<int, Error>);
        }

        [DataTestMethod]
        [DataRow(-1, 5, 3)]
        [DataRow(0, 3, 1)]
        [DataRow(4, 3, 1)]
        [DataRow(4, 7, 3)]
        public void IsOutOfRange_ShouldFailInvalidRangeInt_True(int input, int rangeFrom, int rangeTo)
        {
            var check = Guard.Check.IsOutOfRange(input, "int", rangeFrom, rangeTo);
            Assert.IsTrue(check is Failure<int, Error>);
        }
    }
}
