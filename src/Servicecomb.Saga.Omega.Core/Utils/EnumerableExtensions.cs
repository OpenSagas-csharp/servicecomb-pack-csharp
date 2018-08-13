using System;
using System.Collections.Generic;
using System.Text;

namespace Servicecomb.Saga.Omega.Core.Utils
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Distinct<T, TK>(this IEnumerable<T> source, Func<T, TK> predicate)
        {
            var sets = new HashSet<TK>();
            foreach (var item in source)
            {
                if (sets.Add(predicate(item)))
                {
                    yield return item;
                }
            }
        }
    }
}
