using System;
using System.Linq;
using System.Text;

namespace GenerateUniqueCodeAES
{
    public class GenerateCode
    {
        private static readonly Random _random = new Random();
        private static readonly int[] _UnicodeCharactersList =
                Enumerable.Range(48, 10)              // Numbers           48   - 57
                .Concat(Enumerable.Range(65, 26))     // English uppercase 65   - 90
                                                      //.Concat(Enumerable.Range(97, 26))     // English lowercase 97   - 122
                                                      //.Concat(Enumerable.Range(1488, 27))   // Hebrew            1488 - 1514
            .ToArray();

        public static string GenerateUniqueCode(int sMaxSize)
        {
            StringBuilder builder = new StringBuilder();

            char ch;
            for (int i = 0; i < sMaxSize; i++)
            {
                ch = Convert.ToChar(
                    _UnicodeCharactersList[_random.Next(1, _UnicodeCharactersList.Length)]
                );
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
