using System.Security.Cryptography;
using System.Text;

namespace Core.Application.Utils
{
    public class PasswordUtil
    {
        public static string GenerateHashPassword(string password, out string passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("Password cannot be empty");
            }

            using var hmac = new HMACSHA512();
            passwordSalt = Convert.ToBase64String(hmac.Key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        public static bool IsPasswordMatch(string password, string passwordSalf, string passwordHash)
        {
            var key = Encoding.UTF8.GetBytes(passwordSalf);
            using var hmac = new HMACSHA256(key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash).SequenceEqual(passwordHash);
        }
    }
}
