using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ENOYAEntegrasyonV2.Services.Infrastructure
{
    public static class EncryptionService
    {
        //// 16 karakter (128-bit) sabit key ve IV
        //private static readonly string Key = "MySuperSecretKey123"; // 16 karakter
        //private static readonly string IV = "MyInitVector123456";   // 16 karakter
        public static string AesEncryptString(string refid, string plainText)
        {
            refid = refid.Trim().ToUpper().PadLeft(16, '0').Substring(0, 16);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(refid.Substring(0, 16));
                aes.IV = Encoding.UTF8.GetBytes(Reverse(refid.Substring(0, 16)));

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs, Encoding.UTF8))
                        {
                            sw.Write(plainText);
                        }
                    }

                    byte[] encrypted = ms.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public static string AesDecryptString(string refid, string cipherText)
        {
            refid = refid.Trim().ToUpper().PadLeft(16, '0').Substring(0, 16);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(refid.Substring(0, 16));
                aes.IV = Encoding.UTF8.GetBytes(Reverse(refid.Substring(0, 16)));

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs, Encoding.UTF8))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

    }
}