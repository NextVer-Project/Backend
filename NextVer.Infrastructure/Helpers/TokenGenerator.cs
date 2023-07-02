using System.Security.Cryptography;

namespace NextVer.Infrastructure.Helpers
{
    public static class TokenGenerator
    {
        public static string GenerateToken(int size = 32)
        {
            byte[] randomBytes = new byte[size];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return ToBase64String(randomBytes);
        }

        private static string ToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}