using Mahamudra.Core.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsCore
{
    [TestClass]
    public class BindTest
    {
        private Result<Person, string> person1;
        private Result<Person, string> person2;
        private Person person3;
        [TestInitialize]
        public void Init()
        {
            person1 = new Success<Person, string>(new Person
            {
                Email = "",
                Name = "Pippo",
                Age = 120
            });

            person2 = new Success<Person, string>(new Person
            {
                Email = "pippo@gmail.com",
                Name = "Pippo",
                Age = 14
            });

            person3 = new Person
            {
                Email = "pippo@gmail.com",
                Name = "",
                Age = 55
            }; 
        }

        [TestMethod]
        public void Bind_ShouldFailValidateEmail_True()
        {
            var result = person1
             .Bind(Validation.CheckName)
             .Bind(Validation.CheckEmail);
            Assert.Contains("Email should not be blank.", result.Messages);
        }

        [TestMethod]
        public void Bind_ShouldFailValidateAge_True()
        {
            var result = person2
             .Bind(Validation.CheckName)
             .Bind(Validation.CheckEmail)
             .Bind(Validation.CheckAge);
            Assert.Contains("The age should be not inferior than 18.", result.Messages);
        }

        [TestMethod]
        public void SimpleBind_ShouldFailValidateName_True()
        {
            var result = person3
             .Bind(Validation.CheckName)
             .Bind(Validation.CheckEmail)
             .Bind(Validation.CheckAge);
            Assert.Contains("Name should not be blank.", result.Messages);
        }
    }
}