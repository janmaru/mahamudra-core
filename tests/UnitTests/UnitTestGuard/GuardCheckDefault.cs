using Mahamudra.Core.Errors;
using Mahamudra.Guard;
using Mahamudra.Result.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestsGuard
{
    [TestClass]
    public class GuardCheckDefault
    {
        [TestMethod]
        public void IsDefault_ShouldSuccedStringDefault_True()
        {
            var check = Guard.Check.IsDefault<string>("", "string");
            Assert.IsTrue(check is Success<string, Error>);
        }
        [TestMethod]
        public void IsDefault_ShouldSuccedIntDefault_True()
        {
            var check = Guard.Check.IsDefault<int>(1, "int");
            Assert.IsTrue(check is Success<int, Error>);
        }
        [TestMethod]
        public void IsDefault_ShouldSuccedGuidDefault_True()
        {
            var check = Guard.Check.IsDefault<Guid>(Guid.NewGuid(), "guid");
            Assert.IsTrue(check is Success<Guid, Error>);
        }
        [TestMethod]
        public void IsDefault_ShouldSuccedDateTimeDefault_True()
        {
            var check = Guard.Check.IsDefault<DateTime>(DateTime.Now, "datetime");
            Assert.IsTrue(check is Success<DateTime, Error>);
        }
        [TestMethod]
        public void IsDefault_ShouldSuccedObjectDefault_True()
        {
            var check = Guard.Check.IsDefault<Object>(new Object(), "object");
            Assert.IsTrue(check is Success<Object, Error>);
        }

        [TestMethod]
        public void IsDefault_ShouldFailDefaultString_True()
        {
            var check = Guard.Check.IsDefault(default(string), "string");
            Assert.IsTrue(check is Failure<string, Error>);
        }

        [TestMethod]
        public void IsDefault_ShouldFailDefaultInt_True()
        {
            var check = Guard.Check.IsDefault(default(int), "int");
            Assert.IsTrue(check is Failure<int, Error>);
        }

        [TestMethod]
        public void IsDefault_ShouldFailDefaultGuid_True()
        {
            var check = Guard.Check.IsDefault(default(Guid), "guid");
            Assert.IsTrue(check is Failure<Guid, Error>);
        }

        [TestMethod]
        public void IsDefault_ShouldFailDefaultDateTime_True()
        {
            var check = Guard.Check.IsDefault(default(DateTime), "datetime");
            Assert.IsTrue(check is Failure<DateTime, Error>);
        }
        [TestMethod]
        public void IsDefault_ShouldFailDefaultObject_True()
        {
            var check = Guard.Check.IsDefault(default(object), "object");
            Assert.IsTrue(check is Failure<Object, Error>);
        }
    }
}
     
      