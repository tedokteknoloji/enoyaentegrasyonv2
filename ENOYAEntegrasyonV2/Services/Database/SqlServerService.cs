using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ENOYAEntegrasyonV2.Models.Configuration;
using ENOYAEntegrasyonV2.Services.Interfaces;

namespace ENOYAEntegrasyonV2.Services.Database
{
    /// <summary>
    /// MSSQL veritabanı servisi
    /// ENOYAMODELLEME.sql tablolarına bağlanır
    /// </summary>
    public class SqlServerService : IDatabaseService
    {
        private readonly DatabaseSettings _settings;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public SqlServerService(DatabaseSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Bağlantıyı aç
        /// </summary>
        private async Task OpenConnectionAsync()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_settings.GetConnectionString());
            }

            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }
        }

        /// <summary>
        /// Bağlantıyı test et
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await OpenConnectionAsync();
                using (var cmd = new SqlCommand("SELECT 1", _connection))
                {
                    await cmd.ExecuteScalarAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// SQL sorgusu çalıştır (SELECT)
        /// </summary>
        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            await OpenConnectionAsync();
            var dataTable = new DataTable();
            
            using (var adapter = new SqlDataAdapter(query, _connection))
            {
                adapter.SelectCommand.CommandTimeout = _settings.CommandTimeout;
                adapter.Fill(dataTable);
            }
            
            return dataTable;
        }

        /// <summary>
        /// SQL komutu çalıştır (INSERT, UPDATE, DELETE)
        /// </summary>
        public async Task<int> ExecuteNonQueryAsync(string command)
        {
            await OpenConnectionAsync();
            
            using (var cmd = new SqlCommand(command, _connection))
            {
                if (_transaction != null)
                {
                    cmd.Transaction = _transaction;
                }
                cmd.CommandTimeout = _settings.CommandTimeout;
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Scalar değer döndür
        /// </summary>
        public async Task<object> ExecuteScalarAsync(string query)
        {
            await OpenConnectionAsync();
            
            using (var cmd = new SqlCommand(query, _connection))
            {
                if (_transaction != null)
                {
                    cmd.Transaction = _transaction;
                }
                cmd.CommandTimeout = _settings.CommandTimeout;
                return await cmd.ExecuteScalarAsync();
            }
        }

        /// <summary>
        /// Transaction başlat
        /// </summary>
        public void BeginTransaction()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                OpenConnectionAsync().Wait();
            }
            
            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        /// Transaction commit et
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Transaction rollback et
        /// </summary>
        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}

