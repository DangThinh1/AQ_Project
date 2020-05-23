using Newtonsoft.Json;
using System;
 
namespace AccommodationMerchant.Core.Helpers
{
    public static class Base64Helper
    {
        public static T DecodeFromBase64String<T>(string base64String)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(base64String);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string decoded_string = new String(decoded_char);

            return JsonConvert.DeserializeObject<T>(decoded_string);
        }
        public static string DecodeFromBase64String(string base64String)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64String);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string EncodeToBase64String(object obj)
        {
            var jsonObject = JsonConvert.SerializeObject(obj);
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(jsonObject);
            var base64String = Convert.ToBase64String(byt);
            return base64String;
        }

        public static string EncodeToBase64String(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
