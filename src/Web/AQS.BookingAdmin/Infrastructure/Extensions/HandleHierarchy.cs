using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Infrastructure.Extensions
{
  public static class HandleHierarchy
  {
    //public static IEnumerable<T> SelectManyRecursiveFunc<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
    //{
    //  var result = source.SelectMany(selector);
    //  if (!result.Any())
    //  {
    //    return result;
    //  }
    //  return result.Concat(result.SelectManyRecursiveFunc(selector));
    //}

    //public static IEnumerable<T> Flatten<T, R>(this IEnumerable<T> source, Func<T, R> recursion) where R : IEnumerable<T>
    //{
    //  return source.SelectMany(x => (recursion(x) != null && recursion(x).Any()) ? recursion(x).Flatten(recursion) : null)
    //               .Where(x => x != null);
    //}
  }
}
