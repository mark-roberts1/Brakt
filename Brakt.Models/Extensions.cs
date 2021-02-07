using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brakt
{
    public static class Extensions
    {

        public static T ThrowIf<T>(this T obj, Func<T, string> getMessage, Func<T, bool> shouldThrow)
        {
            if (shouldThrow(obj)) throw new ArgumentException(getMessage(obj));

            return obj;
        }

        public static T ThrowIfNull<T>(this T obj, string name)
        {
            if (obj == null) throw new ArgumentNullException(name);

            return obj;
        }

        public static T ThrowIfDefault<T>(this T obj, string name)
        {
            if (obj.Equals(default(T))) throw new ArgumentException($"{name} is required.");

            return obj;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}
