using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace UnitTestsCategoryTheory
{
    [TestClass]
    public class UnitTestInt32
    {
        Int32Category int32Category = null;

        [TestInitialize]
        public void Init()
        {
            int32Category = new Int32Category();
        }

        [DataTestMethod]
        [DataRow(5, 6)]
        [DataRow(7, 9)]
        [DataRow(12, 33)]
        [DataRow(-3, 8)]
        public void Morphism_ShouldGetLessThanOrEqual_AreEqual(int a, int b)
        {
            var expression = int32Category.Morphism(Expression.Constant(a), Expression.Constant(b));
            Assert.AreEqual(expression.ToString(), $"({a} <= {b})");
        }

        [DataTestMethod]
        [DataRow(22, 6)]
        [DataRow(-3, -5)]
        public void Morphism_ShouldGetMoreThanOrEqual_False(int a, int b)
        {
            var expression = int32Category.Morphism(Expression.Constant(a), Expression.Constant(b));
            var result = Expression.Lambda<Func<bool>>(expression).Compile()();
            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(-3)]
        [DataRow(0)]
        public void Identity_ShouldBeEqualToItself_True(int a)
        {
            var expression = int32Category.Identity(Expression.Constant(a));
            var result = Expression.Lambda<Func<bool>>(expression).Compile()();
            Assert.IsTrue(result);
        }
        [DataTestMethod]
        [DataRow(12)]
        [DataRow(-13)]
        [DataRow(0)]
        public void Identity_ShouldBeEqualToItself_AreEqual(int a)
        {
            var expression = int32Category.Identity(Expression.Constant(a));
            var result = Expression.Lambda<Func<bool>>(expression).Compile()();
            Assert.AreEqual(expression.ToString(), $"({a} <= {a})");
        }

        [DataTestMethod]
        [DataRow(1, 5, 7)]
        [DataRow(-5, -1, 0)]
        public void Compose_ShouldCompose_AreEqual(int a, int b, int c)
        {
            var expression1 = int32Category.Morphism(Expression.Constant(a), Expression.Constant(b));
            var expression2 = int32Category.Morphism(Expression.Constant(b), Expression.Constant(c));

            var expression = int32Category.Compose(expression2, expression1);
            Assert.AreEqual(expression.ToString(), $"({a} <= {c})");
        }

        [DataTestMethod]
        [DataRow(1, 5, 7)]
        [DataRow(-5, -1, 0)]
        public void Compose_ShouldCompose_True(int a, int b, int c)
        {
            var expression1 = int32Category.Morphism(Expression.Constant(a), Expression.Constant(b));
            var expression2 = int32Category.Morphism(Expression.Constant(b), Expression.Constant(c));
            var expression = int32Category.Compose(expression2, expression1);
            var result = Expression.Lambda<Func<bool>>(expression).Compile()();

            Assert.IsTrue(result);
        }

        //[DataTestMethod]
        //[DataRow(int.MinValue)]
        //[DataRow(int.MinValue + 4)]
        //[DataRow(int.MinValue + 229)]
        //public void Object_IntegersShouldbeAnObject_True(int a)
        //{
        //    var expression = int32Category.Objects.Where(x => x == Expression.Constant(a)).FirstOrDefault();

        //    Assert.AreEqual(expression.ToString(), $"{a}");
        //}
    }
}
