using System;
using System.Data;
using System.Threading.Tasks;

namespace ENOYAEntegrasyonV2.Services.Interfaces
{
    /// <summary>
    /// MSSQL veritabanı servisi interface
    /// </summary>
    public interface IDatabaseService : IDisposable
    {
        /// <summary>
        /// Bağlantıyı test et
        /// </summary>
        Task<bool> TestConnectionAsync();

        /// <summary>
        /// SQL sorgusu çalıştır (SELECT)
        /// </summary>
        Task<DataTable> ExecuteQueryAsync(string query);

        /// <summary>
        /// SQL komutu çalıştır (INSERT, UPDATE, DELETE)
        /// </summary>
        Task<int> ExecuteNonQueryAsync(string command);

        /// <summary>
        /// Scalar değer döndür
        /// </summary>
        Task<object> ExecuteScalarAsync(string query);

        /// <summary>
        /// Transaction başlat
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Transaction commit et
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Transaction rollback et
        /// </summary>
        void RollbackTransaction();
    }
}

