using System.Collections.Generic;

namespace ProxyGenerator.Infrastructure
{
    internal static class QueueExtension
    {
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                queue.Enqueue(item);
            }
        } 
    }
}
