using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace AQBooking.Core.Extentions
{
    public static class SelectListExtension
    {
        public static List<SelectList> ToSelectList(this List<object> source, string displayFieldName, string valueFieldName)
        {
            var obj = source.FirstOrDefault();
            if(obj == null || !obj.HasProperty(displayFieldName) || !obj.HasProperty(valueFieldName))
            foreach (var item in source)
            {

            }
            return null;
        }
    }
}
