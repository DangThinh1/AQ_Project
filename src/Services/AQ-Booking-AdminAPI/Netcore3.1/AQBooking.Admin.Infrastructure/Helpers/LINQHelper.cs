using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Helpers
{
    public static class LINQHelper
    {
        // <summary>
        ///
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderByString">{Column Name} {ASC/DESC}</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string orderByString)
        {
            var splitOrderBy = orderByString.Split(" ");
            string columnName = splitOrderBy.First();
            string sortType = splitOrderBy.Last();
            // parametro => expressão
            var parametro = Expression.Parameter(typeof(TSource), "r");
            var expressao = Expression.Property(parametro, columnName);
            var lambda = Expression.Lambda(expressao, parametro); // r => r.AlgumaCoisa
            var tipo = typeof(TSource).GetProperty(columnName).PropertyType;
            var nome = "OrderBy";
            if (string.Equals(sortType, "desc", StringComparison.InvariantCultureIgnoreCase))
            {
                nome = "OrderByDescending";
            }
            var metodo = typeof(Queryable).GetMethods().First(m => m.Name == nome && m.GetParameters().Length == 2);
            var metodoGenerico = metodo.MakeGenericMethod(new[] { typeof(TSource), tipo });
            return metodoGenerico.Invoke(source, new object[] { source, lambda }) as IOrderedQueryable<TSource>;
        }
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, string orderByString)
        {
            var splitOrderBy = orderByString.Split(" ");
            string columnName = splitOrderBy.First();
            string sortType = splitOrderBy.Last();
            var parametro = Expression.Parameter(typeof(TSource), "r");
            var expressao = Expression.Property(parametro, columnName);
            var lambda = Expression.Lambda<Func<TSource, string>>(expressao, parametro); // r => r.AlgumaCoisa
            var tipo = typeof(TSource).GetProperty(columnName).PropertyType;
            var nome = "ThenBy";
            if (string.Equals(sortType, "desc", StringComparison.InvariantCultureIgnoreCase))
            {
                nome = "ThenByDescending";
            }
            var metodo = typeof(Queryable).GetMethods().First(m => m.Name == nome && m.GetParameters().Length == 2);
            var metodoGenerico = metodo.MakeGenericMethod(new[] { typeof(TSource), tipo });
            return metodoGenerico.Invoke(source, new object[] { source, lambda }) as IOrderedQueryable<TSource>;
        }
    }
}
