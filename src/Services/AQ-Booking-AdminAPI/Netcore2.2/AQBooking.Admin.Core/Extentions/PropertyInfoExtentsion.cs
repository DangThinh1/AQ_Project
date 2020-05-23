using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AQBooking.Admin.Core.Extentions
{
    public static class PropertyInfoExtentsion
    {
        private const string _deletedPropertyName = "Deleted";

        #region Common Method

        public static bool CompareByPropertyName(this object obj, string propertyName, object fillterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(propertyName))
                    return true;

                var objPropValue = obj.GetPropertyValueByPropertyName(propertyName);
                if (objPropValue == null)
                {
                    return fillterValue == null;
                }

                return objPropValue.Equals(fillterValue);
            }
            catch
            {
                return false;
            }
        }

        public static Dictionary<string, string> ToDictionary(this object obj)
        {
            if (obj == null)
                return null;

            var propertyInfos = obj.GetType().GetProperties();
            if (propertyInfos.Count() == 0)
                return null;

            var dictionary = new Dictionary<string, string>();

            foreach (var property in propertyInfos)
            {
                dictionary.Add(property.Name, obj.GetPropertyValueByPropertyName(property.Name).ToString());
            }

            return dictionary;
        }

        public static bool IsNotDeleted(this object obj)
        {
            try
            {
                var propertyValue = obj.GetPropertyValueByPropertyName(_deletedPropertyName);
                if (propertyValue != null)
                {
                    var isDeleted = (bool)propertyValue;
                    return !isDeleted;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ContainsString(this object obj, string searchString, string[] ignoreFields = null)
        {
            try
            {
                if (string.IsNullOrEmpty(searchString))
                    return true;

                var propertyInfos = obj.GetType().GetProperties();
                foreach (var property in propertyInfos)
                {
                    var value = obj.GetPropertyValueByPropertyName(property.Name);
                    if (value != null && value.ToString().ToLower().Contains(searchString.ToLower()))
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool CompareTo(this object obj, SearchModel fillterObj)
        {
            try
            {
                var fillterProperties = fillterObj.GetFillterProperties();
                var objProperties = obj.GetType().GetProperties();
                foreach (var fillterProp in fillterProperties)
                {
                    var fillterValue = fillterProp.GetValue(fillterObj, null);
                    foreach (var objProp in objProperties)
                    {
                        if (objProp.Name.Equals(fillterProp.Name))
                        {
                            var objValue = objProp.GetValue(obj, null);
                            if (!objValue.Equals(fillterValue))
                                return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool NotBelongTo(this PropertyInfo Property, Type Class)
        {
            var classProps = Class.GetProperties();
            foreach (var prop in classProps)
            {
                if (prop.Name.Equals(Property.Name))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool NotBelongTo(this PropertyInfo Property, object obj)
        {
            var classProps = obj.GetType().GetProperties();
            foreach (var prop in classProps)
            {
                if (prop.Name.Equals(Property.Name))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool HasProperty(this object obj, string propertyName, Type type = null, bool ignoreCase = false)
        {
            try
            {
                var prop = GetPropertyInfoByPropertyName(obj, propertyName, type, ignoreCase);
                if (prop != null)
                {
                    if (type != null)
                    {
                        if (prop.PropertyType.Equals(type))
                        {
                            return true;
                        }

                        return false;
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                if (ex is PropertyInfoNotFoundException)
                {
                    return false;
                }
                throw ex;
            }
        }

        #endregion Common Method

        #region Set Method

        public static bool SetPropertyValueByPropertyName(this object obj, string propertyName, object propertyValue)
        {
            try
            {
                var objProp = obj.GetPropertyInfoByPropertyName(propertyName);

                if (objProp != null)
                {
                    objProp.SetValue(obj, propertyValue);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                if (ex is PropertyInfoNotFoundException)
                {
                    return false;
                }
                throw ex;
            }
        }

        public static bool SetValueByNameIfExists(this object obj, string propertyName, object propertyValue, bool ignoreCase = false)
        {
            try
            {
                if (obj.HasProperty(propertyName, null, ignoreCase))
                {
                    return obj.SetPropertyValueByPropertyName(propertyName, propertyValue);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool TrySetPropertyValueByPropertyName(this object obj, string propertyName, object propertyValue)
        {
            try
            {
                PropertyInfo objProp = obj.GetPropertyInfoByPropertyName(propertyName);

                if (obj.TryGetPropertyInfoByPropertyName(propertyName, out objProp))
                {
                    objProp.SetValue(obj, propertyValue);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Set Method

        #region Get Method

        public static PropertyInfo GetPropertyInfoByPropertyName(this object obj, string propertyName, Type type = null, bool ignoreCase = false)
        {
            try
            {
                var objProperties = obj.GetType().GetProperties();
                PropertyInfo propFound = null; ;
                if (ignoreCase)
                {
                    if (type == null)
                        propFound = objProperties.FirstOrDefault(p => p.Name.ToLower().Equals(propertyName.ToLower()));
                    else
                        propFound = objProperties.FirstOrDefault(p =>
                        p.Name.ToLower().Equals(propertyName.ToLower()) &&
                        p.GetType().Equals(type));
                }
                else
                {
                    if (type == null)
                        propFound = objProperties.FirstOrDefault(p => p.Name.Equals(propertyName));
                    else
                        propFound = objProperties.FirstOrDefault(p =>
                        p.Name.Equals(propertyName) &&
                        p.GetType().Equals(type));
                }

                if (propFound == null)
                    throw new PropertyInfoNotFoundException(obj, propertyName);
                return propFound;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object GetPropertyValueByPropertyName(this object obj, string propertyName)
        {
            try
            {
                var objProp = obj.GetPropertyInfoByPropertyName(propertyName);
                if (objProp != null)
                {
                    var objPropValue = objProp.GetValue(obj, null);
                    return objPropValue;
                }
                return null;
            }
            catch (Exception ex)
            {
                if (ex is PropertyInfoNotFoundException)
                {
                    return null;
                }
                throw ex;
            }
        }

        public static object GetValueByNameIfExists(this object obj, string propertyName)
        {
            try
            {
                if (obj.HasProperty(propertyName))
                {
                    return obj.GetPropertyValueByPropertyName(propertyName);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static bool TryGetPropertyInfoByPropertyName(this object obj, string propertyName, out PropertyInfo propertyInfo)
        {
            try
            {
                propertyInfo = obj.GetPropertyInfoByPropertyName(propertyName);
                return true;
            }
            catch (Exception ex)
            {
                if (ex is PropertyInfoNotFoundException)
                {
                    propertyInfo = null;
                    return false;
                }
                throw ex;
            }
        }

        #endregion Get Method
    }

    public class PropertyInfoNotFoundException : Exception
    {
        public string PropertyName { get; set; }
        public object Object { get; set; }

        public PropertyInfoNotFoundException(object obj, string propertyName)
            : base(string.Format("Property with name {0} not found in Type {1}", propertyName, obj.GetType()))
        {
            this.Object = obj;
            this.PropertyName = propertyName;
        }
    }

    public class SetPropertyValueFailedException : Exception
    {
        public string PropertyName { get; set; }
        public object Object { get; set; }
        public object Value { get; set; }

        public SetPropertyValueFailedException(object obj, string propertyName, object value)
            : base(string.Format("Failed to set the value: {0} with type: {1} of property: {2} on type: {3} ", value, value.GetType(), propertyName, obj.GetType()))
        {
            this.Value = value;
            this.Object = obj;
            this.PropertyName = propertyName;
        }
    }

    public class GetPropertyValueFailedException : Exception
    {
        public string PropertyName { get; set; }
        public object Object { get; set; }

        public GetPropertyValueFailedException(object obj, string propertyName, object value)
            : base(string.Format("Failed to get the value of property: {0} on type: {1} ", propertyName, obj.GetType()))
        {
            this.Object = obj;
            this.PropertyName = propertyName;
        }
    }
}