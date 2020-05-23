using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtendedUtility
{
    ///<summary>
    ///Extended Double class for utility methods
    ///</summary>
    public static class ExtendedDouble
    {
        ///<summary>
        ///Round 2 decimal double
        ///</summary>
        public static double Round(this double target)
        {
            return Math.Round(target, 2);
        }

        ///<summary>
        ///Round indicated decimal double
        ///</summary>
        public static double Round(this double target, int roundindicate)
        {
            return Math.Round(target, roundindicate);
        }

        ///<summary>
        ///Floor 2 decimal double
        ///</summary>
        public static double Floor2Decimal(this double target)
        {
            return Math.Floor(target * 100) / 100;
        }

        ///<summary>
        ///Floor decimal double
        ///<param name="target">Double variable</param>
        ///<param name="decimalplace">Decimal place, from 0 to 5</param>
        ///</summary>
        public static double FloorDecimal(this double target, int decimalplace = 2)
        {
            switch (decimalplace)
            {
                case 0:
                    return Math.Floor(target);
                case 1:
                    return Math.Floor(target * 10) / 10;
                case 2:
                    return Math.Floor(target * 100) / 100;
                case 3:
                    return Math.Floor(target * 1000) / 1000;
                case 4:
                    return Math.Floor(target * 10000) / 10000;
                default:
                    return Math.Floor(target * 100000) / 100000;
            }
        }

        ///<summary>
        ///Floor 2 decimal double?, default is 0
        ///</summary>
        public static double Floor2Decimal(this double? target)
        {
            try
            {
                return Math.Floor((double)target * 100) / 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Floor decimal double?, default is 0
        ///<param name="target">Double? variable</param>
        ///<param name="decimalplace">Decimal place, from 0 to 5</param>
        ///</summary>
        public static double FloorDecimal(this double? target, int decimalplace = 2)
        {
            try
            {
                double val = (double)target;
                switch (decimalplace)
                {
                    case 0:
                        return Math.Floor(val);
                    case 1:
                        return Math.Floor(val * 10) / 10;
                    case 2:
                        return Math.Floor(val * 100) / 100;
                    case 3:
                        return Math.Floor(val * 1000) / 1000;
                    case 4:
                        return Math.Floor(val * 10000) / 10000;
                    default:
                        return Math.Floor(val * 100000) / 100000;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Get ceiling value 
        ///</summary>
        public static int Ceiling(this double target)
        {
            return Math.Ceiling(target).ToInt32();
        }

        ///<summary>
        ///Convert double? to double
        ///</summary>
        public static double ToDouble(this double? target)
        {
            return target.GetValueOrDefault();
        }

        ///<summary>
        ///Convert double to currency text
        ///<param name="value">double target</param>
        ///<param name="culturecode">Culture code. Default en-SG.</param>
        ///</summary>
        public static string ToCurrencyText(this double value, string culturecode = "en-SG")
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culturecode);
            try
            {
                return String.Format(cultureInfo, "{0:c}", value);
            }
            catch (Exception)
            {
                return String.Format(cultureInfo, "{0:c}", 0);
            }
        }

        ///<summary>
        ///Convert double? to currency text
        ///<param name="value">double target</param>
        ///<param name="culturecode">Culture code. Default en-SG.</param>
        ///</summary>
        public static string ToCurrencyText(this double? value, string culturecode = "en-SG")
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culturecode);
            try
            {
                return String.Format(cultureInfo, "{0:c}", value.GetValueOrDefault());
            }
            catch (Exception)
            {
                return String.Format(cultureInfo, "{0:c}", 0);
            }
        }

        ///<summary>
        ///Convert double to text value  
        ///<param name="value">double target</param>
        ///<param name="culturecode">Culture code. Default en-SG.</param>
        ///<param name="format">Number format. Default {0:0,0.00} eg: 1,000,000.00</param>
        ///</summary>
        public static string ToTextValue(this double value, string culturecode = "en-SG", string format = "{0:0,0.00}")
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culturecode);
            try
            {
                return String.Format(cultureInfo, format, value);
            }
            catch (Exception)
            {
                return String.Format(cultureInfo, format, 0);
            }
        }

        ///<summary>
        ///Convert double? to text value
        ///<param name="value">double? target</param>
        ///<param name="culturecode">Culture code. Default en-SG.</param>
        ///<param name="format">Number format. Default {0:0,0.00} eg: 1,000,000.00</param>
        ///</summary>
        public static string ToTextValue(this double? value, string culturecode = "en-SG", string format = "{0:0,0.00}")
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culturecode);
            try
            {
                return String.Format(cultureInfo, format, value.GetValueOrDefault());
            }
            catch (Exception)
            {
                return String.Format(cultureInfo, format, 0);
            }
        }

        ///<summary>
        ///Convert positive number to negative number
        ///</summary>
        public static double ToNegative(this double target)
        {
            return -Math.Abs(target);
        }

        ///<summary>
        ///Convert negative number to positive number
        ///</summary>
        public static double ToPositive(this double target)
        {
            return Math.Abs(target);
        }

        ///<summary>
        ///Convert positive number to negative number
        ///</summary>
        public static double? ToNegative(this double? target)
        {
            try
            {
                if (target != null)
                    return -Math.Abs((double)target);
                else return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Convert double to int
        ///</summary>
        public static int ToInt32(this double target)
        {
            return Convert.ToInt32(target);
        }

        ///<summary>
        ///Convert double to long
        ///</summary>
        public static long ToInt64(this double target)
        {
            return Convert.ToInt64(target);
        }

        ///<summary>
        ///Calculate value by percentage, default is null
        ///</summary>
        public static double? CalNullPercentValue(this double target, double percentage)
        {
            try
            {
                return (target * percentage) / 100;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is 0
        ///</summary>
        public static double CalPercentValue(this double target, double percentage)
        {
            try
            {
                return (target * percentage) / 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is null
        ///</summary>
        public static double? CalNullPercentValue(this double? target, double percentage)
        {
            try
            {
                return ((double)target * percentage) / 100;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is 0
        ///</summary>
        public static double CalPercentValue(this double? target, double percentage)
        {
            try
            {
                return ((double)target * percentage) / 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Get percentage by compare value, default is 0.
        /// </summary>
        /// <param name="target">double target</param>
        /// <param name="valuetocompare">value to compare</param>
        /// <returns></returns>
        public static double GetPercentByValue(this double target, double valuetocompare)
        {
            try
            {
                return target * 100 / valuetocompare;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Get percentage by compare value, default is null.
        /// </summary>
        /// <param name="target">double target</param>
        /// <param name="valuetocompare">value to compare</param>
        /// <returns></returns>
        public static double? GetNullPercentByValue(this double target, double valuetocompare)
        {
            try
            {
                return target * 100 / valuetocompare;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get percentage by compare value, default is 0.
        /// </summary>
        /// <param name="target">double? target</param>
        /// <param name="valuetocompare">value to compare</param>
        /// <returns></returns>
        public static double GetPercentByValue(this double? target, double valuetocompare)
        {
            try
            {
                return target.GetValueOrDefault() * 100 / valuetocompare;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Get percentage by compare value, default is null.
        /// </summary>
        /// <param name="target">double? target</param>
        /// <param name="valuetocompare">value to compare</param>
        /// <returns></returns>
        public static double? GetNullPercentByValue(this double? target, double valuetocompare)
        {
            try
            {
                return target.GetValueOrDefault() * 100 / valuetocompare;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Return value after minus
        /// </summary>
        /// <param name="target">double target</param>
        /// <param name="valuetominus">double value to minus</param>
        /// <returns></returns>
        public static double Minus(this double target, double valuetominus)
        {
            return (target - valuetominus).Round(10);
        }

        /// <summary>
        /// Return value after minus
        /// </summary>
        /// <param name="target">double target</param>
        /// <param name="valuetominus">double? value to minus</param>
        /// <returns></returns>
        public static double Minus(this double target, double? valuetominus)
        {
            return (target - valuetominus.GetValueOrDefault()).Round(10);
        }

        /// <summary>
        /// Calculate value based on percent and value different
        /// </summary>
        /// <param name="target">double target percent</param>
        /// <param name="basevalue">double base value</param>
        /// <param name="basepercent">double base percent</param>
        /// <returns></returns>
        public static double CalBasedByPercent(this double target, double basevalue, double basepercent)
        {
            try
            {
                return (target * basevalue / basepercent).Round();
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }

    ///<summary>
    ///Extended Decimal class for utility methods
    ///</summary>
    public static class ExtendedDecimal
    {
        ///<summary>
        ///Convert decimal to double
        ///</summary>
        public static double ToDouble(this decimal target)
        {
            return Convert.ToDouble(target);
        }

        ///<summary>
        ///Round 2 decimal double
        ///</summary>
        public static double Round(this decimal target)
        {
            return Math.Round(target, 2).ToDouble();
        }

        ///<summary>
        ///Round indicated decimal double
        ///</summary>
        public static double Round(this decimal target, int roundindicate)
        {
            return Math.Round(target, roundindicate).ToDouble();
        }

        ///<summary>
        ///Floor 2 decimal double
        ///</summary>
        public static double Floor2Decimal(this decimal target)
        {
            return (Math.Floor(target * 100) / 100).ToDouble();
        }

        ///<summary>
        ///Convert decimal? to double
        ///</summary>
        public static decimal ToDecimal(this decimal? target)
        {
            return target.GetValueOrDefault();
        }

        ///<summary>
        ///Convert decimal to currency text
        ///</summary>
        public static string ToCurrencyText(this decimal value, string culturecode = "en-SG")
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culturecode);
            try
            {
                return String.Format(cultureInfo, "{0:c}", value);
            }
            catch (Exception)
            {
                return String.Format(cultureInfo, "{0:c}", 0);
            }
        }

        ///<summary>
        ///Convert positive number to negative number
        ///</summary>
        public static double ToNegative(this decimal target)
        {
            return -Math.Abs(Convert.ToDouble(target));
        }
        ///<summary>
        ///Convert negative number to positive number
        ///</summary>
        public static double ToPositive(this decimal target)
        {
            return Math.Abs(Convert.ToDouble(target));
        }

        ///<summary>
        ///Convert decimal to int
        ///</summary>
        public static int ToInt32(this decimal target)
        {
            return Convert.ToInt32(target);
        }

        ///<summary>
        ///Convert decimal? to int, default value is 0
        ///</summary>
        public static int ToInt32(this decimal? target)
        {
            try
            {
                return Convert.ToInt32(target);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>double?
        ///Convert decimal to long
        ///</summary>
        public static long ToInt64(this decimal target)
        {
            return Convert.ToInt64(target);
        }

        ///<summary>
        ///Convert decimal? to long, default value is 0
        ///</summary>
        public static long ToInt64(this decimal? target)
        {
            try
            {
                return Convert.ToInt64(target);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is null
        ///</summary>
        public static double? CalNullPercentValue(this decimal target, double percentage)
        {
            try
            {
                return (target.ToDouble() * percentage) / 100;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is 0
        ///</summary>
        public static double CalPercentValue(this decimal target, double percentage)
        {
            try
            {
                return (target.ToDouble() * percentage) / 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is null
        ///</summary>
        public static double? CalNullPercentValue(this decimal? target, double percentage)
        {
            try
            {
                return ((double)target * percentage) / 100;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is 0
        ///</summary>
        public static double CalPercentValue(this decimal? target, double percentage)
        {
            try
            {
                return ((double)target * percentage) / 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }


    }

    ///<summary>
    ///Extended Int32 class for utility methods
    ///</summary>
    public static class ExtendedInt
    {
        ///<summary>
        ///Convert int? to int
        ///</summary>
        public static int ToInt32(this int? target)
        {
            return target.GetValueOrDefault();
        }

        ///<summary>
        ///Convert long? to long
        ///</summary>
        public static long ToInt64(this long? target)
        {
            return target.GetValueOrDefault();
        }

        ///<summary>
        ///Convert int? to int
        ///</summary>
        public static int ToInt(this int? target)
        {
            return target.GetValueOrDefault();
        }

        ///<summary>
        ///Convert int to currency text
        ///</summary>
        public static string ToCurrencyText(this int value, string culturecode = "en-SG")
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culturecode);
            try
            {
                return String.Format(cultureInfo, "{0:c}", value);
            }
            catch (Exception)
            {
                return String.Format(cultureInfo, "{0:c}", 0);
            }
        }

        ///<summary>
        ///Convert positive number to negative number
        ///</summary>
        public static double ToNegative(this int target)
        {
            return -Math.Abs(target);
        }

        ///<summary>
        ///Convert negative number to positive number
        ///</summary>
        public static double ToPositive(this int target)
        {
            return Math.Abs(target);
        }


        ///<summary>
        ///Calculate value by percentage, default is null
        ///</summary>
        public static double? CalNullPercentValue(this int target, double percentage)
        {
            try
            {
                return (target.ToDouble() * percentage) / 100;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is 0
        ///</summary>
        public static double CalPercentValue(this int target, double percentage)
        {
            try
            {
                return (target.ToDouble() * percentage) / 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is null
        ///</summary>
        public static double? CalNullPercentValue(this int? target, double percentage)
        {
            try
            {
                return ((double)target * percentage) / 100;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Calculate value by percentage, default is 0
        ///</summary>
        public static double CalPercentValue(this int? target, double percentage)
        {
            try
            {
                return ((double)target * percentage) / 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// Get percentage by compare value, default is 0.
        /// </summary>
        /// <param name="target">int target</param>
        /// <param name="valuetocompare">value to compare</param>
        /// <returns></returns>
        public static double GetPercentByValue(this int target, double valuetocompare)
        {
            try
            {
                return target.ToDouble() * 100 / valuetocompare;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Get percentage by compare value, default is null.
        /// </summary>
        /// <param name="target">int target</param>
        /// <param name="valuetocompare">value to compare</param>
        /// <returns></returns>
        public static double? GetNullPercentByValue(this int target, double valuetocompare)
        {
            try
            {
                return target.ToDouble() * 100 / valuetocompare;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get percentage by compare value, default is 0.
        /// </summary>
        /// <param name="target">int? target</param>
        /// <param name="valuetocompare">value to compare</param>
        /// <returns></returns>
        public static double GetPercentByValue(this int? target, double valuetocompare)
        {
            try
            {
                return target.ToDouble() * 100 / valuetocompare;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Get percentage by compare value, default is null.
        /// </summary>
        /// <param name="target">int? target</param>
        /// <param name="valuetocompare">value to compare</param>
        /// <returns></returns>
        public static double? GetNullPercentByValue(this int? target, double valuetocompare)
        {
            try
            {
                return target.ToDouble() * 100 / valuetocompare;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }


    ///<summary>
    ///Extended String class for utility methods
    ///</summary>
    public static class ExtendedString
    {
        ///<summary>
        ///Remove special character in string
        ///</summary>
        public static string RemoveNotValidCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_./]+", " ", RegexOptions.Compiled);
        }

        ///<summary>
        ///Convert string to double, default value is 0
        ///</summary>
        public static double ToDouble(this string target)
        {
            try
            {
                double value = Convert.ToDouble(target);
                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Convert string to datetime, default value is current date time
        ///</summary>
        public static DateTime ToDateTime(this string target)
        {
            try
            {
                DateTime value = Convert.ToDateTime(target);
                return value;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        /////<summary>
        /////Convert string to datetime?, default value is null
        /////</summary>
        //public static Nullable<DateTime> ToDateTimeNull(this string target)
        //{
        //    try
        //    {
        //        DateTime value = Convert.ToDateTime(target);
        //        return value;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        ///<summary>
        ///Convert string to datetime?, default value is null
        ///</summary>
        public static DateTime? ToNullDateTime(this string target)
        {
            try
            {
                DateTime value = Convert.ToDateTime(target);
                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Convert string to currency text
        ///<param name="target">String variable</param>
        ///<param name="culturecode">Culture format code, default is "en-SG"</param>
        ///</summary>
        public static string ToCurrencyText(this string target, string culturecode = "en-SG")
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culturecode);
            try
            {

                double value = Convert.ToDouble(target);
                return String.Format(cultureInfo, "{0:c}", value);
            }
            catch (Exception)
            {
                return String.Format(cultureInfo, "{0:c}", 0);
            }
        }

        ///<summary>
        ///Convert string to int, default value is 0
        ///</summary>
        public static int ToInt32(this string target)
        {
            try
            {
                int value = Convert.ToInt32(target);
                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Convert string to long, default value is 0
        ///</summary>
        public static long ToInt64(this string target)
        {
            try
            {
                long value = Convert.ToInt64(target);
                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Convert string to int, default value is 0
        ///</summary>
        public static int ToInt(this string target)
        {
            try
            {
                int value = Convert.ToInt32(target);
                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Convert string to bool, default value is false
        ///</summary>
        public static bool ToBool(this string target)
        {
            try
            {
                return target.ToLower() == "true" ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///<summary>
        ///Convert string to int, default value is null
        ///</summary>
        public static int? ToNullInt32(this string target)
        {
            try
            {
                int value = Convert.ToInt32(target);
                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Convert string to long, default value is null
        ///</summary>
        public static long? ToNullInt64(this string target)
        {
            try
            {
                long value = Convert.ToInt64(target);
                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Convert string to decimal, default value is 0
        ///</summary>
        public static decimal ToDecimal(this string target)
        {
            try
            {
                decimal value = Convert.ToDecimal(target);
                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Check string have value or not
        ///</summary>
        public static bool HaveValue(this string target)
        {
            return !String.IsNullOrEmpty(target);
        }

        ///<summary>
        ///Convert string to round double, default value is 0
        ///<param name="target">String variable</param>
        ///<param name="roundindicate">Number of decimal place, default is 2</param>
        ///</summary>
        public static double ToRoundDouble(this string target, int roundindicate = 2)
        {
            try
            {
                return target.ToDouble().Round(roundindicate);

            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Convert string to floor double, default floor decimal is 2 digits and default value is 0
        ///<param name="target">String variable</param>        
        ///</summary>
        public static double ToFloorDouble(this string target)
        {
            try
            {
                return target.ToDouble().Floor2Decimal();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Convert string to double, default value is null
        ///</summary>
        public static double? ToNullDouble(this string target)
        {
            try
            {
                double value = Convert.ToDouble(target);
                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Convert string to floor double?
        ///<param name="target">String variable</param>
        ///<param name="decimalplace">Decimal place, from 0 to 5 default is 2</param>
        ///</summary>
        public static double? ToNullFloorDouble(this string target, int decimalplace = 2)
        {
            try
            {
                return target.ToNullDouble().FloorDecimal(decimalplace);
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Hash Password by MD5
        ///<param name="target">String variable</param>
        ///</summary>
        public static string ToHashMD5(this string target)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(target));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        ///<summary>
        ///Hash Password by SHA256
        ///<param name="target">String variable</param>
        ///</summary>
        public static String ToHashSHA256(this string target)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(target));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        ///<summary>
        ///Hash Password by SHA-1
        ///<param name="target">String variable</param>
        ///</summary>
        public static String ToHashSHA1(this string target)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA1 hash = SHA1Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(target));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }

        ///<summary>
        ///Hash Password by strong encryption method
        ///<param name="target">String variable</param>
        ///</summary>
        public static String ToHashStrongType(this string target)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA1 hash = SHA1Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(target));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString().ToHashMD5();
        }

        ///<summary>
        ///Convert string to Base64 string
        ///<param name="target">String variable</param>
        ///</summary>
        public static string ToBase64String(this string target)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(target);
            return System.Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        ///<summary>
        ///Decode Base64 string to string
        ///<param name="target">Base64 String variable</param>
        ///</summary>
        public static string DecodeBase64ToString(this string target)
        {
            try
            {
                if (IsBase64(target))
                {
                    byte[] bytes = System.Convert.FromBase64String(target);
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    return encoding.GetString(bytes, 0, bytes.Length);
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static bool IsBase64(string target)
        {
            string expression = @"^[a-zA-Z0-9\+/]*={0,2}$";
            return Regex.IsMatch(target, expression);
        }
    }

    ///<summary>
    ///Extended Boolean class for utility methods
    ///</summary>
    public static class ExtendedBool
    {
        ///<summary>
        ///Convert bool? to bool
        ///</summary>
        public static bool ToBool(this bool? target)
        {
            return target.GetValueOrDefault();
        }
    }

    ///<summary>
    ///Extended DateTime class for utility methods
    ///</summary>
    public static class ExtendedDateTime
    {
        #region DateTime With Format

        private const string SPLIT = "_";
        private const string FORMAT = "MM/dd/yyyy HH:mm";

        public static DateTime? ToDateTimeOrDefault(this string str, string split = SPLIT)
        {
            try
            {
                var array = str.Split(split);
                var dateTimeStr = array[0];
                var format = array[1];
                var dateTime = DateTime.ParseExact(str, format, null);
                return dateTime;
            }
            catch
            {
                return null;
            }
        }
        public static string ToStringOrDefault(this DateTime dateTime, string format = FORMAT, string split = SPLIT)
        {
            try
            {
                var dateTimeStr = dateTime.ToString(format);
                var result = $"{dateTimeStr}{split}{format}";
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static string ToStringOrDefault(this DateTime? dateTime, string format = FORMAT, string split = SPLIT)
        {
            try
            {
                if (!dateTime.HasValue)
                    return null;
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
        
        ///<summary>
        ///Convert DateTime? to DateTime, default value is year 0001
        ///</summary>
        public static DateTime ToDateTime(this DateTime? target)
        {
            return target.GetValueOrDefault();
        }

        ///<summary>
        ///Get current date time by UTC time zone.
        ///<param name="target">DateTime variable</param>
        ///<param name="gmtzone">The number of time zone.  Ex: for VN(UTC+7) use 7</param>
        ///</summary>
        public static DateTime ToUTCTimeZone(this DateTime target, int gmtzone)
        {
            return DateTime.UtcNow.AddHours(gmtzone);
        }

        ///<summary>
        ///Convert date to text date
        ///<param name="target">DateTime variable</param>
        ///<param name="format">Date format, default is "dd-MMM-yyyy"</param>
        ///</summary>
        public static string ToTextDate(this DateTime target, string format = "dd-MMM-yyyy")
        {
            return target.ToString(format);
        }

        ///<summary>
        ///Convert date to text time
        ///<param name="target">DateTime variable</param>
        ///<param name="format">Time format, default is "hh:mm tt"</param>
        ///</summary>
        public static string ToTextTime(this DateTime target, string format = "hh:mm tt")
        {
            return target.ToString(format);
        }

        ///<summary>
        ///Conver date to text date time
        ///<param name="target">DateTime variable</param>
        ///<param name="format">DateTime format, default is "dd-MMM-yyyy hh:mm tt"</param>
        ///</summary>
        public static string ToTextDateTime(this DateTime target, string format = "dd-MMM-yyyy hh:mm tt")
        {
            return target.ToString(format);
        }

        ///<summary>
        ///Convert date to text date, if null default is blank
        ///<param name="target">DateTime variable</param>
        ///<param name="format">Date format, default is "dd-MMM-yyyy"</param>
        ///</summary>
        public static string ToTextDate(this DateTime? target, string format = "dd-MMM-yyyy")
        {
            try
            {
                return target.Value.ToString(format);
            }
            catch (Exception)
            {
                return "";
            }
        }

        ///<summary>
        ///Convert date to text time, if null default is blank
        ///<param name="target">DateTime variable</param>
        ///<param name="format">Time format, default is "hh:mm tt"</param>
        ///</summary>
        public static string ToTextTime(this DateTime? target, string format = "hh:mm tt")
        {
            try
            {
                return target.Value.ToString(format);
            }
            catch (Exception)
            {
                return "";
            }
        }

        ///<summary>
        ///Convert date to text date time, if null default is blank
        ///<param name="target">DateTime variable</param>
        ///<param name="format">DateTime format, default is "dd-MMM-yyyy hh:mm tt"</param>
        ///</summary>
        public static string ToTextDateTime(this DateTime? target, string format = "dd-MMM-yyyy hh:mm tt")
        {
            try
            {
                return target.Value.ToString(format);
            }
            catch (Exception)
            {
                return "";
            }
        }

        ///<summary>
        ///Generate timestamp from date time now
        ///<param name="target">DateTime variable</param>
        ///<param name="format">DateTime format, default is "ddMMMyyyyHHmmss"</param>
        ///</summary>
        public static string GenerateTimeStamp(this DateTime target, string format = "ddMMMyyyyHHmmss")
        {
            return target.ToString(format);
        }
    }

    ///<summary>
    ///Extended object class for utility methods
    ///</summary>
    public static class ExtendedObject
    {
        ///<summary>
        ///Convert object to int, default value is 0
        ///</summary>
        public static int ToInt32(this object target)
        {
            try
            {
                return Convert.ToInt32(target);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Convert object to long, default value is 0
        ///</summary>
        public static long ToInt64(this object target)
        {
            try
            {
                return Convert.ToInt64(target);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Convert object to decimal, default value is 0
        ///</summary>
        public static decimal ToDecimal(this object target)
        {
            try
            {
                return Convert.ToDecimal(target);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ///<summary>
        ///Convert object to double, default value is 0
        ///</summary>
        public static double ToDouble(this object target)
        {
            try
            {
                return Convert.ToDouble(target);
            }
            catch (Exception)
            {
                return 0;
            }
        }


        ///<summary>
        ///Convert object to datetime, default value is current datetime
        ///</summary>
        public static DateTime ToDateTime(this object target)
        {
            try
            {
                return Convert.ToDateTime(target);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        ///<summary>
        ///Convert object to datetime, default value is null
        ///</summary>
        public static DateTime? ToDateTimeNull(this object target)
        {
            try
            {
                return Convert.ToDateTime(target);
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Convert object to boolean, default value is false
        ///</summary>
        public static bool ToBool(this object target)
        {
            try
            {
                return Convert.ToBoolean(target);
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///<summary>
        ///Convert object to boolean, default value is null
        ///</summary> 
        public static bool? ToBoolNull(this object target)
        {
            try
            {
                return Convert.ToBoolean(target);
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Convert object to boolean, default value is null
        ///</summary> 
        public static bool? ToNullBoolean(this object target)
        {
            try
            {
                return Convert.ToBoolean(target);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Extended class for List int and List string
    /// </summary>
    public static class ExtendedList
    {
        /// <summary>
        /// Convert List int to comma separated string
        /// </summary>
        /// <param name="target">List Int</param>
        /// <returns>comma separated string</returns>
        public static string ToCommaSeparatedString(this List<int> target)
        {
            if (target.Count() > 0)
            {
                return String.Join(",", target);
            }
            return "";
        }

        /// <summary>
        /// Convert List double to comma separated string
        /// </summary>
        /// <param name="target">List Double</param>
        /// <returns>comma separated string</returns>
        public static string ToCommaSeparatedString(this List<double> target)
        {
            if (target.Count() > 0)
            {
                return String.Join(",", target);
            }
            return "";
        }
        /// <summary>
        /// Convert List string to comma separated string
        /// </summary>
        /// <param name="target">List String</param>
        /// <returns>comma separated string</returns>
        public static string ToCommaSeparatedString(this List<string> target)
        {
            if (target.Count() > 0)
            {
                return String.Join(",", target);
            }
            return "";
        }

        /// <summary>
        /// Convert comma seperated string to List int
        /// </summary>
        /// <param name="target">Comma separated string</param>
        /// <returns>List int</returns>
        public static List<int> ToListInt(this string target)
        {
            try
            {
                List<int> TagIds = target.Split(',').Select(int.Parse).ToList();
                return TagIds;
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }
        /// <summary>
        /// Convert comma seperated string to List double
        /// </summary>
        /// <param name="target">Comma separated string</param>
        /// <returns>List double</returns>
        public static List<double> ToListDouble(this string target)
        {
            try
            {
                List<double> TagIds = target.Split(',').Select(double.Parse).ToList();
                return TagIds;
            }
            catch (Exception)
            {
                return new List<double>();
            }
        }

        /// <summary>
        /// Convert comma seperated string to List string
        /// </summary>
        /// <param name="target">Comma separated string</param>
        /// <returns>List string</returns>
        public static List<string> ToListString(this string target)
        {
            try
            {
                List<string> TagIds = target.Split(',').ToList();
                return TagIds;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}
