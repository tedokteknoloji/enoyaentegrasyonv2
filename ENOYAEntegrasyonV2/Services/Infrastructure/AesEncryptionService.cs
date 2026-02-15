using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ENOYAEntegrasyonV2.Services.Interfaces;

namespace ENOYAEntegrasyonV2.Services.Infrastructure
{
    /// <summary>
    /// AES-256 şifreleme servisi
    /// </summary>
    public class AesEncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public AesEncryptionService()
        {
            // Sabit bir key ve IV kullanıyoruz (production'da daha güvenli bir yöntem kullanılmalı)
            // Key: 32 byte (256 bit), IV: 16 byte (128 bit)
            string keyString = "ENOYA2024EncryptionKey32Byte!!"; // 32 karakter
            string ivString = "ENOYA2024IV16Byte"; // 16 karakter

            _key = Encoding.UTF8.GetBytes(keyString);
            _iv = Encoding.UTF8.GetBytes(ivString);
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                        byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
            catch
            {
                // Hata durumunda orijinal metni döndür
                return plainText;
            }
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipherText);
                        byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            catch
            {
                // Hata durumunda (eski format veya şifrelenmemiş) orijinal metni döndür
                return cipherText;
            }
        }
    }
}

