using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestsCategoryTheory
{
    [TestClass]
    public class UnitTestDotNetv2
    {

        DotNetCategoryv2 dotnetCategory = null;

        [TestInitialize]
        public void Init()
        {
            dotnetCategory = new DotNetCategoryv2();
        }


        [DataTestMethod]
        [DataRow(1, "one", true, 0.1)]
        [DataRow(5, "five", false, -4.5)]
        [DataRow(0, "zero", false, 45.0)]
        public void Associativity_ShouldEnforceLaw_AreEqual(int input, string middle, bool result, Double last)
        {
            var m1 = dotnetCategory.Morphism(Type.GetType($"System.Int32"), Type.GetType($"System.String"));
            var m2 = dotnetCategory.Morphism(Type.GetType($"System.String"), Type.GetType($"System.Boolean")); 
            var m3 = dotnetCategory.Morphism(Type.GetType($"System.Boolean"), Type.GetType($"System.Double"));

            var m21 = dotnetCategory.Compose(m2, m1); 
            var m32 = dotnetCategory.Compose(m3, m2); 
            var m32_1 = dotnetCategory.Compose(m32, m1);

            var m3_21 = dotnetCategory.Compose(m3, m21);

            var v1 = m1.DynamicInvoke(input, middle);
            var v2 = m2.DynamicInvoke(middle, result);
            var v32 = m32.DynamicInvoke(middle, result, last); 
            var v3 = m3.DynamicInvoke(result, last);
            var v21 = m21.DynamicInvoke(input, middle, result);

            var v32_1 = m32_1.DynamicInvoke(input, middle, last);
            var v3_21 = m3_21.DynamicInvoke(input, result, last);

            Assert.AreEqual(v32_1, v3_21);
        }

        [DataTestMethod]
        [DataRow(1, "one", true)]
        [DataRow(5, "five", false)]
        [DataRow(0, "zero", false)]
        public void Compose_ShouldCompose_AreEqual(int input, string middle, bool result)
        {
            var m1 = dotnetCategory.Morphism(Type.GetType($"System.Int32"), Type.GetType($"System.String"));
            var m2 = dotnetCategory.Morphism(Type.GetType($"System.String"), Type.GetType($"System.Boolean"));
            var m3 = dotnetCategory.Compose(m2, m1);
            var v1 = m1.DynamicInvoke(input, middle);
            var v2 = m2.DynamicInvoke(middle, result);
            var v3 = m3.DynamicInvoke(input, middle, result);
            Assert.AreEqual(v3, v2);
        }

        [TestMethod]
        [DataRow("Int32", "String")]
        public void Morphism_ShouldCheckTypeGiven_True(string stypeIn, string stypeOut)
        {
            Type typeIn = Type.GetType($"System.{stypeIn}");
            Type typeOut = Type.GetType($"System.{stypeOut}");
            var morphism = dotnetCategory.Morphism(typeIn, typeOut);
            Assert.IsNotNull(morphism);
        }

        [TestMethod]
        public void Identity_ShouldCheckTypeGiven_True()
        {
            IdentityTestHelper<string>("String", "I'm a string");
            IdentityTestHelper<double>("Double", -12.0);
            IdentityTestHelper<Single>("Single", 34f);
            IdentityTestHelper<Int32>("Int32", 29);
        }

        private void IdentityTestHelper<T>(string stypeIn, T value)
        {
            Type typeIn = Type.GetType($"System.{stypeIn}");
            var identity = dotnetCategory.Identity(typeIn);
            var i = Convert.ChangeType(value, typeIn);
            Assert.AreEqual(value, identity.DynamicInvoke(i, i));
        }
    }
}
