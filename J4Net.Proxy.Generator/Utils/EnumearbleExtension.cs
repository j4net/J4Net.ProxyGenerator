using System;
using System.Collections.Generic;
using System.Linq;

namespace ProxyGenerator.Utils
{
    internal static class EnumearbleExtension
    {
        public static bool ContainsAnyFrom<T>(this IEnumerable<T> enumerable, params T[] items)
        {
            return enumerable.Count(items.Contains) > 0;
        }
    }
}
