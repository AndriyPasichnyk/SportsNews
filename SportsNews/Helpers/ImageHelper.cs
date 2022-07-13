using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews
{
    public static class ImageHelper
    {
        public static async Task<byte[]> ConvertFileToByteArray(IFormFile file)
        {
            if (file != null)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    await file.CopyToAsync(m);
                    byte[] imageBytes = m.ToArray();
                    return m.ToArray();
                }
            }
            return Array.Empty<byte>();
        }
    }
}
