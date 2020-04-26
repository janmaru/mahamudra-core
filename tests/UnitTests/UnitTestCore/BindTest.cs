using Mahamudra.Result.Core.Patterns;
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
            Assert.IsTrue(result.Messages.Contains("Email should not be blank."));
        }

        [TestMethod]
        public void Bind_ShouldFailValidateAge_True()
        {
            var result = person2
             .Bind(Validation.CheckName)
             .Bind(Validation.CheckEmail)
             .Bind(Validation.CheckAge);
            Assert.IsTrue(result.Messages.Contains("The age should be not inferior than 18."));
        }

        [TestMethod]
        public void SimpleBind_ShouldFailValidateName_True()
        {
            var result = person3
             .Bind(Validation.CheckName)
             .Bind(Validation.CheckEmail)
             .Bind(Validation.CheckAge);
            Assert.IsTrue(result.Messages.Contains("Name should not be blank."));
        }
    }
}
