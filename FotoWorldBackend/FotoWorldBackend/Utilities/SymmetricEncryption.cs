using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FotoWorldBackend.Utilities
{
    /// <summary>
    //  Symmetric Encryption services
    /// </summary>
    public static class SymmetricEncryption
    {

        /// <summary>
        /// Encrypts passed text 
        /// </summary>
        /// <param name="key">32 sign key</param>
        /// <param name="text">Text to encrypt</param>
        /// <returns>Cipher</returns>
        public static string Encrypt(string key, string text)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(text);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }



        /// <summary>
        /// Translates given cipher to plain text
        /// </summary>
        /// <param name="key">32 sign same as in encruption</param>
        /// <param name="cipher">Cipher to translate</param>
        /// <returns>Plain Text</returns>
        public static string Decrypt(string key, string cipher)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipher);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    } 
}
