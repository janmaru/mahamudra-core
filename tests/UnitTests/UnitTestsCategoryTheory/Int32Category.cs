using Mahamudra.Core.CategoryTheory;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnitTestsCategoryTheory
{
    public class Int32Category : ICategory<Expression, BinaryExpression>
    {
        /// <summary>Gets the objects.</summary>
        /// <value>The objects.</value>
        public IEnumerable<Expression> Objects
        {
            get
            {
                for (int int32 = int.MinValue; int32 <= int.MaxValue; int32++)
                {
                    yield return Expression.Constant(int32);
                }
            }
        }
 
        public BinaryExpression Morphism(Expression @objectLeft, Expression @objectRight)
        {
            return Expression.LessThanOrEqual(@objectLeft, @objectRight);
        }

        /// <summary>Composes the specified morphisms.</summary>
        /// (Y <= Z) [(X <= Y)] => X <= Z.
        /// Should keep the order like in g[f]
        /// <param name="morphism2">The morphism2.</param>
        /// <param name="morphism1">The morphism1.</param>
        /// <returns></returns>
        public BinaryExpression Compose(BinaryExpression morphism2, BinaryExpression morphism1) =>
            Morphism(morphism1.Left, morphism2.Right);

        /// <summary>Identities the specified object.</summary>
        /// X <= X.
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public BinaryExpression Identity(Expression @object)
        {
            return Morphism(@object, @object);
        }
    }
}
