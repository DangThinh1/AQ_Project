using System;

namespace AccommodationMerchant.Core.Helpers
{
    public static class DateTimeHelper
    {
        #region DateTime With Format

        private const string SPLIT = "_";
        private const string FORMAT = "MM/dd/yyyy HH:mm";

        public static DateTime? ToDateTimeNullable(string str)
        {
            try
            {
                string split = SPLIT;
                var array = str.Split(split);
                var dateTimeStr = array[0];
                var format = array[1];
                var dateTime = DateTime.ParseExact(dateTimeStr, format, null);
                return dateTime;
            }
            catch
            {
                return null;
            }
        }

        public static string ToString(DateTime dateTime)
        {
            try
            {
                string split = SPLIT;
                string format = FORMAT;
                var dateTimeStr = dateTime.ToString(format);
                var result = $"{dateTimeStr}{split}{format}";
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static string ToString(DateTime? dateTime)
        {
            try
            {
                if (!dateTime.HasValue)
                    return null;
                string split = SPLIT;
                string format = FORMAT;
                var dateTimeStr = dateTime.Value.ToString(format);
                var result = $"{dateTimeStr}{split}{format}";
                return result;
            }
            catch
            {
                return null;
            }
        }

        #endregion DateTime With Format
    }
}
