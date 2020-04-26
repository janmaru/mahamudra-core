using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Result.Core;
using Mahamudra.Result.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestsGuard
{
    [TestClass]
    public class GuardCheckNull
    {
 

        [TestMethod]
        public void IsNull_ShouldSuccedStringEmpty_True()
        {
            var check = Guard.Check.IsNull("", "string");
            Assert.IsTrue(check is Success<object, Error>);
        }

        [TestMethod]
        public void IsNull_ShouldSuccedIntOne_True()
        {
            var check = Guard.Check.IsNull(1, "int");
            Assert.IsTrue(check is Success<object, Error>);
        }

        [TestMethod]
        public void IsNull_ShouldSuccedGuidEmpty_True()
        {
            var check = Guard.Check.IsNull(Guid.Empty, "guid");
            Assert.IsTrue(check is Success<object, Error>);
        }

        [TestMethod]
        public void IsNull_ShouldSuccedDateTimeNow_True()
        {
            var check = Guard.Check.IsNull(DateTime.UtcNow, "datetime");
            Assert.IsTrue(check is Success<object, Error>);
        }

        [TestMethod]
        public void IsNull_ShouldSuccedNewObject_True()
        {
            var check = Guard.Check.IsNull(new Object(), "object");
            Assert.IsTrue(check is Success<object, Error>);
        }

        [TestMethod]
        public void IsNull_ShouldFailNullObject_True()
        {
            var check = Guard.Check.IsNull(null, "null");
            Assert.IsTrue(check is Failure<object, Error>);
        }
    }  
}

 