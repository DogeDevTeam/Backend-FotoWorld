using System.Security.Cryptography;
using FotoWorldBackend.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace FotoWorldBackend.Utilities
{
    /// <summary>
    /// All functionalities connected to password hashing
    /// </summary>
    public static class  PasswordHash
    {

        /// <summary>
        /// Generates Salt 
        /// </summary>
        /// <returns>salt</returns>
        public static string GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128/8);
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Hashes given password using provided salt
        /// </summary>
        /// <param name="password">Password to hash</param>
        /// <param name="salt">Salt</param>
        /// <returns>Hashed Password</returns>
        public static string HashPassword(string password, string salt)
        {
            string hashed= Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        /// <summary>
        /// Verifies given password with given user
        /// </summary>
        /// <param name="password">Password provided in login form</param>
        /// <param name="user">User found using username or email</param>
        /// <returns>True if password is correct</returns>
        public static bool VerifyPassword(string password, User user) {
            string test = HashPassword(password, user.PasswordSalt);
            if(test == user.HashedPassword)
            {
                return true;
            }
            return false;
        
        
        }



    }
}
