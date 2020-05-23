using AQEncrypts;

namespace Accommodation.Core.Helpers
{
    public static class Decrypt
    {
        public static int ToInt32(this string target)
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

        public static long ToInt64(this string target)
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
