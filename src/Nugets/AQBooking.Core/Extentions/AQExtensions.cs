using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AQBooking.Core.Extentions
{
   public static class AQExtensions
    {
        public static byte[] ToBytes(this IFormFile file)
        {
            using (var ms=new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
        
    }
}
