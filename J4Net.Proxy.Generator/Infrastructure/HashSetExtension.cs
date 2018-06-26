using System.Collections.Generic;

namespace ProxyGenerator.Infrastructure
{
    public static class HashSetExtension
    {
        public static void AddMany<T>(this HashSet<T> hashSet, IEnumerable<T> items)
        {
            foreach (var item in items)
                hashSet.Add(item);
        }
    }
}
