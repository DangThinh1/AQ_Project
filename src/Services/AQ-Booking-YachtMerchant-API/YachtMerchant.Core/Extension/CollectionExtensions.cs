using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YachtMerchant.Core.Extension
{
    public static class CollectionExtensions
    {
        
        /*
         **** Example use*****
         var result = db.Stocks
            .WhereIf(batchNumber != null, s => s.Number == batchNumber)
            .WhereIf(name != null,        s => s.Name.StartsWith(name))       
            .ToList();
         */
        /// <summary>
        /// Extention use filter multiple where with condition
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns> IQueryable</returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source,  bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate).AsQueryable();
            else
                return source;
        }



        /*
         * Example use extention
         List<int> intList = new List<int> {1,2,3,4,5,6 };
         List<double> doubleList = new List<double> {0.4, 0.6,0.8,4.5,0.2 };

         //Extension method
         Console.WriteLine(intList.randomElement());
         Console.WriteLine(doubleList.randomElement())
        */
        /// <summary>
        /// Get random 1 element in list extention
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this List<T> list)
        {
            Random ran = new Random();
            int randomIndex = ran.Next(list.Count - 1);

            return list[randomIndex];
        }
    }
}
