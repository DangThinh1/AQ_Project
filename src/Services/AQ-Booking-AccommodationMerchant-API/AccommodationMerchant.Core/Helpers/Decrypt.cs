using AQEncrypts;

namespace AccommodationMerchant.Core.Helpers
{
    public static class Decrypt
    {
        public static int DecryptToInt32(this string target)
        {
            try
            {
                var decrypted = Terminator.Decrypt(target);
                return int.Parse(decrypted);
            }
            catch
            {
                return 0;
            }
        }

        public static long DecryptToInt64(this string target)
        {
            try
            {
                var decrypted = Terminator.Decrypt(target);
                return int.Parse(decrypted);
            }
            catch
            {
                return 0;
            }
        }
    }
}