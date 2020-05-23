using Omu.ValueInjecter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Core.Extentions
{
    public static class ValueInjecterExtention
    {
        public static List<T> ConvertToList<T>(this IEnumerable source) where T : class
        {
            List<T> result = new List<T>();
            foreach(var item in source)
            {
                var t = (T)Activator.CreateInstance(typeof(T));
                t.InjectFrom(item);
                result.Add(t);
            }
            return result;
        }
    }
}
