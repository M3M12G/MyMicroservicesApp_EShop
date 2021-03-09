using System;
using System.Linq;

namespace Discount_gRPC.Helpers
{
    public class PromoGenerator
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
