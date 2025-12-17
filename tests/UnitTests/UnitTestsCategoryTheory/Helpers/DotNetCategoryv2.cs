using Mahamudra.Core.CategoryTheory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestsCategoryTheory
{
    public class DotNetCategoryv2 : ICategory<Type, Delegate>
    {
        public static TResult C<TSource, TMiddle, TResult>(TSource value, TMiddle middle, TResult result) => result;
        public static TResult M<TSource, TResult>(TSource value, TResult result) => result;

        public IEnumerable<Type> Objects =>
                AppDomain.CurrentDomain.GetAssemblies()
                      .SelectMany(t => t.GetTypes())
                      .Where(t => t.Namespace == "System");

        public Delegate Compose(Delegate morphism2, Delegate morphism1)
        { 
            var source = morphism1.Method.GetParameters().First().ParameterType;
            var middle = morphism1.Method.ReturnType;
            var result = morphism2.Method.ReturnType;
            return typeof(DotNetCategoryv2)
               .GetMethod(nameof(DotNetCategoryv2.C))
               .MakeGenericMethod(
                source,
                middle,
                result)
               .CreateDelegate(typeof(Func<,,,>).MakeGenericType(source, middle, result, result));
        }

        public Delegate Identity(Type @object)
        {
            return Morphism(@object, @object);
        }

        public Delegate Morphism(Type @objectLeft, Type @objectRight)
        {
            return typeof(DotNetCategoryv2)
               .GetMethod(nameof(DotNetCategoryv2.M)).
                MakeGenericMethod(@objectLeft, @objectRight)
               .CreateDelegate(typeof(Func<,,>).MakeGenericType(@objectLeft, @objectRight, @objectRight));
        }
    }
}
