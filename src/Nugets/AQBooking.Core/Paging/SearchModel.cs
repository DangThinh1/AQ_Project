using AQBooking.Core.Extentions;
using AQBooking.Core.Paging;
using System.Collections.Generic;
using System.Reflection;

namespace AQBooking.Core.Paging
{
    public class SearchModel : PagableModel
    {
        public string SearchString { get; set; }

        public SearchModel() : base()
        {
            SearchString = string.Empty;
        }

        public List<PropertyInfo> GetFillterProperties()
        {
            var fillterProperties = new List<PropertyInfo>();
            var thisProperties = this.GetType().GetProperties();

            foreach (var prop in thisProperties)
            {
                var value = prop.GetValue(this, null);
                if (value == null)
                    continue;

                var type = value.GetType();

                if (type == typeof(string))
                {
                    if(string.IsNullOrEmpty(value.ToString()))
                        continue;
                }
                else
                {
                    if (type == typeof(int) && (int)value < 0)
                        continue;
                    if (type == typeof(long) && (int)value < 0)
                        continue;
                    if (type == typeof(float) && (int)value < 0)
                        continue;
                }

                if (prop.NotBelongTo(typeof(SearchModel)))
                {
                    fillterProperties.Add(prop);
                }
            }
            return fillterProperties;
        }
    }
}
