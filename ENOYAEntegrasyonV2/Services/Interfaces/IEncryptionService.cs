namespace ENOYAEntegrasyonV2.Services.Interfaces
{
    /// <summary>
    /// Şifreleme servisi interface
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Metni şifreler
        /// </summary>
        string Encrypt(string plainText);

        /// <summary>
        /// Şifrelenmiş metni çözer
        /// </summary>
        string Decrypt(string cipherText);
    }
}

