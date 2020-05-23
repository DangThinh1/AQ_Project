using System;
using System.Linq;
using System.Text;

namespace AQBooking.FileStreamAPI.Core.Helpers
{
    public partial class UniqueIDHelper
    {
        private static readonly Random _RandomSize = new Random();
        private static readonly Random _random = new Random();
        private static readonly int[] _UnicodeCharactersList =
                    Enumerable.Range(48, 10)              // Numbers           48   - 57
                    .Concat(Enumerable.Range(65, 26))     // English uppercase 65   - 90
                                                          //.Concat(Enumerable.Range(97, 26))     // English lowercase 97   - 122
                                                          //.Concat(Enumerable.Range(1488, 27))   // Hebrew            1488 - 1514
                .ToArray();

        /// <summary>
        ///
        /// </summary>
        /// <param name="sMaxSize"></param>
        /// <param name="IsFixed"></param>
        /// <returns></returns>
        //[return: SqlFacet(MaxSize = -1)]
        public static string GenarateRandomString(
            int sMaxSize,
            bool IsFixed = false
        )
        {
            if (IsFixed)
            {
                sMaxSize = _RandomSize.Next(1, sMaxSize);
            }

            System.Random randomInt = new System.Random((int)System.DateTime.Now.Ticks);
            int start = 1;
            int size = 1000;
            int randomIntFromDateTime = randomInt.Next(start, size);
            int pivot = sMaxSize - randomIntFromDateTime.ToString().Length;
            StringBuilder builder = new StringBuilder();
            Random randomIndex = new Random();
            char ch;
            for (int i = 0; i < pivot; i++)
            {
                ch = Convert.ToChar(
                    _UnicodeCharactersList[_random.Next(1, _UnicodeCharactersList.Length)]
                );
                builder.Append(ch);
            }
            int index = randomIndex.Next(0, builder.Length);
            var randomStringCLR = builder.ToString().Insert(index, randomIntFromDateTime.ToString());
            return randomStringCLR;
        }
    }
}
