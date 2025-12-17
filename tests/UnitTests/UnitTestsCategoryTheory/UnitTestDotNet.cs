using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTestsCategoryTheory
{
    [TestClass]
    public class UnitTestDotNet
    {
        DotNetCategory dotnetCategory = null;
        Func<Type, Type> M1 = (Type int32) => int32.ToString().GetType();
        Func<Type, Type> M2 = (Type str) => str.ToString().Count().GetType();
        Func<Type, Type> M3 = (Type int32) => int32.ToString().ToUpper().GetType();

        [TestInitialize]
        public void Init()
        {
            dotnetCategory = new DotNetCategory();
        }


        [TestMethod]
        [DataRow("String")]
        [DataRow("Int32")]
        [DataRow("Int64")]
        public void Objects_ShouldCheckTypeGiven_True<T>(T nameType)
        {
            Type type = Type.GetType($"System.{nameType}");
            var types = dotnetCategory.Objects;
            Assert.IsTrue(types.Where(x => x == type).Select(x => true).DefaultIfEmpty(false).FirstOrDefault());
        }


        [TestMethod]
        [DataRow("Int32", "String")]
        [DataRow("Int64", "bool")]
        public void Morphism_ShouldCheckTypeGiven_True(string stypeIn, string stypeOut)
        {
            Type typeIn = Type.GetType($"System.{stypeIn}");
            Type typeOut = Type.GetType($"System.{stypeOut}");
            var morphism = dotnetCategory.Morphism(typeIn, typeOut);
            Assert.IsNotNull(morphism);
        }

        [TestMethod]
        [DataRow("Int32")]
        [DataRow("Int64")]
        [DataRow("bool")]
        [DataRow("String")]
        public void Identity_ShouldBeEqualToItself_AreEqual(string stypeIn)
        {
            Type typeIn = Type.GetType($"System.{stypeIn}");
            var identity = dotnetCategory.Identity(typeIn);
            Assert.IsNotNull(identity);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(-5)]
        [DataRow(0)]
        public void Compose_ShouldCompose_AreEqual(int a)
        {
            var M3 = dotnetCategory.Compose(M1, M2);
            var v1 = M1(a.GetType());
            var v2 = M1(v1);
            Assert.AreEqual(M3(a.GetType()), v2);
        }

        [TestMethod]
        [DataRow("System.String")]
        [DataRow("System.Single")]
        [DataRow("System.Double")]
        [DataRow("System.Decimal")]
        public void Associative_ShouldFulfillLaw_AreEqual(string type)
        {
            Type myType = Type.GetType(type);
     
            var M12 = dotnetCategory.Compose(M1, M2); 
            var M312 = dotnetCategory.Compose(M3, M12);
            var vM312 = M312(myType);

            var M32 = dotnetCategory.Compose(M3, M2);
            var M132 = dotnetCategory.Compose(M1, M32); 
            
            Assert.AreEqual(M132(myType), M312(myType));
        } 
    }
}
