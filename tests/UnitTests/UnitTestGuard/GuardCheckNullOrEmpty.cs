using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestsGuard
{
    [TestClass]
    public class GuardCheckNullOrEmpty
    { 
        [TestMethod]
        public void IsNullOrEmpty_ShouldSuccedString_True()
        {
            var check = Guard.Check.IsNullOrEmpty("empty", "string");
            Assert.IsTrue(check is Success<string, Error>);
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldSuccedString2_True()
        {
            var check = Guard.Check.IsNullOrEmpty("616", "aNumericString");
            Assert.IsTrue(check is Success<string, Error>);
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldSuccedStringArray_True()
        {
            var check = Guard.Check.IsNullOrEmpty(new[] { "bracco", "baldo" }, "stringArray");
            Assert.IsTrue(check is Success<IEnumerable<string>, Error>);
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldSuccedIntArray_True()
        {
            var check = Guard.Check.IsNullOrEmpty(new[] { 6, 6, 6 }, "intArray");
            Assert.IsTrue(check is Success<IEnumerable<int>, Error>);
        }
    }  
}
