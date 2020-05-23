namespace AQDiningPortal.Core.Helpers
{
    public static class Generator
    {
        public static void GenerateEmptyStringProperties(object obj)
        {
            if (obj != null)
            {
                var type = obj.GetType();
                if (type.IsClass)
                {
                    var properties = type.GetProperties();
                    if (properties != null)
                    {
                        foreach (var prop in properties)
                        {
                            if (prop.PropertyType.Equals(typeof(string)))
                            {
                                prop.SetValue(obj, string.Empty);
                            }
                        }
                    }
                }
            }
        }
    }
}
