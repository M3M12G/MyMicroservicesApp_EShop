using System;
using System.Linq;

namespace Promocode.API.Helpers
{
    public class PromocodeGenerator
    {
        public static string GenerateCode()
        {
            var alphabet_numbers = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var code = new string(
                        Enumerable.Repeat(alphabet_numbers, 8)
                                  .Select(c => c[random.Next(c.Length)])
                                  .ToArray()
                    );
            return code;
        }
}
}
