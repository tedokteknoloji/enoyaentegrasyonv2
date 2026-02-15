using System;
using ENOYAEntegrasyonV2.Services.Infrastructure;
using ENOYAEntegrasyonV2.Services.Interfaces;

namespace ENOYAEntegrasyonV2
{
    /// <summary>
    /// Şifreleme servisi test sınıfı
    /// </summary>
    public static class TestEncryption
    {
        public static void RunTest()
        {
            try
            {
                Console.WriteLine("=== Şifreleme Servisi Testi ===");
                Console.WriteLine();

                IEncryptionService encryptionService = new AesEncryptionService();

                // Test 1: SQL Şifresi
                string sqlPassword = "MySecretPassword123!";
                Console.WriteLine($"Orijinal SQL Şifresi: {sqlPassword}");
                string encryptedSql = encryptionService.Encrypt(sqlPassword);
                Console.WriteLine($"Şifrelenmiş: {encryptedSql}");
                string decryptedSql = encryptionService.Decrypt(encryptedSql);
                Console.WriteLine($"Çözülmüş: {decryptedSql}");
                Console.WriteLine($"Test 1 Sonucu: {(sqlPassword == decryptedSql ? "BAŞARILI ✓" : "BAŞARISIZ ✗")}");
                Console.WriteLine();

                // Test 2: Client Secret
                string clientSecret = "6GrI9QilCrUqFMlsvdE5WdljD9Hg4Tfc";
                Console.WriteLine($"Orijinal Client Secret: {clientSecret}");
                string encryptedSecret = encryptionService.Encrypt(clientSecret);
                Console.WriteLine($"Şifrelenmiş: {encryptedSecret}");
                string decryptedSecret = encryptionService.Decrypt(encryptedSecret);
                Console.WriteLine($"Çözülmüş: {decryptedSecret}");
                Console.WriteLine($"Test 2 Sonucu: {(clientSecret == decryptedSecret ? "BAŞARILI ✓" : "BAŞARISIZ ✗")}");
                Console.WriteLine();

                // Test 3: Boş string
                string emptyString = "";
                Console.WriteLine($"Boş String Testi");
                string encryptedEmpty = encryptionService.Encrypt(emptyString);
                string decryptedEmpty = encryptionService.Decrypt(encryptedEmpty);
                Console.WriteLine($"Test 3 Sonucu: {(emptyString == decryptedEmpty ? "BAŞARILI ✓" : "BAŞARISIZ ✗")}");
                Console.WriteLine();

                // Test 4: Null string
                string nullString = null;
                Console.WriteLine($"Null String Testi");
                string encryptedNull = encryptionService.Encrypt(nullString);
                string decryptedNull = encryptionService.Decrypt(encryptedNull);
                Console.WriteLine($"Test 4 Sonucu: {(nullString == decryptedNull ? "BAŞARILI ✓" : "BAŞARISIZ ✗")}");
                Console.WriteLine();

                Console.WriteLine("=== Test Tamamlandı ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}

