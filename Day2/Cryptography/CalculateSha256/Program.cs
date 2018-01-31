using System;

namespace CalculateSha256
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            var result = GetSha256("Hello_World");
            Console.WriteLine(result);
        }

        private static string GetSha256(string text)
        {
            using (var hasher = new SHA256Managed())
            {
                var hash = hasher.ComputeHash(Encoding.Unicode.GetBytes(text));
                var strHash = string.Join("", hash.Select(h => h.ToString("x2")));
                return strHash;
            }
        }
    }
}
