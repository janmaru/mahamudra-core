using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsCore
{
    [TestClass]
    public class SuccessTest
    {
        private Result<Person, string> person; 
        [TestInitialize]
        public void Init()
        {
            person = new Success<Person, string>(new Person
            {
                Email = "",
                Name = "Pippo",
                Age = 120
            });

        } 
        [TestMethod]
        public void Success_ShouldReturnSuccessFromResult_True()
        { 
            Assert.IsTrue(person is Success<Person, string>);
        } 
    }
} 