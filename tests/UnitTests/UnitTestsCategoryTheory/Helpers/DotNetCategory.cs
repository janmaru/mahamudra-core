using Mahamudra.Core.CategoryTheory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestsCategoryTheory
{
    public class DotNetCategory : ICategory<Type, Func<Type, Type>>
    {
        public DotNetCategory()
        {

        }

        public IEnumerable<Type> Objects =>
                 AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.Namespace == "System");

        public Func<Type, Type> Compose(Func<Type, Type> morphism2, Func<Type, Type> morphism1)
        {
            return value => morphism2(morphism1(value));
        }

        public Func<Type, Type> Identity(Type @object)
        {
            return Morphism(@object, @object);
        }

        public Func<Type, Type> Morphism(Type first, Type second)
        {
            return first => second;
        }

    }
}
