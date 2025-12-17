using System.Collections.Generic;
using System.Linq;

namespace Mahamudra.Core.Extensions
{
    internal static class CoreExtensions
    {
        internal static bool IsNullOrEmpty<T>(this IList<T> myList)
        {
            return myList == null || !myList.Any() ;
        }
    }
} 