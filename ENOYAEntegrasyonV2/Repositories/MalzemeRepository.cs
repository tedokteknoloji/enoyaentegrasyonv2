using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ENOYAEntegrasyonV2.Models.Entities;
using ENOYAEntegrasyonV2.Repositories.Interfaces;
using ENOYAEntegrasyonV2.Services.Interfaces;

namespace ENOYAEntegrasyonV2.Repositories
{
    /// <summary>
    /// MALZEME repository implementasyonu
    /// </summary>
    public class MALZEMERepository : IMALZEMERepository
    {
        private readonly IDatabaseService _database;

        public MALZEMERepository(IDatabaseService database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<MALZEME> GetByIdAsync(decimal kod)
        {
            var query = $"SELECT * FROM [dbo].[MALZEME] WHERE KOD = {kod}";
            var dataTable = await _database.ExecuteQueryAsync(query);
            
            if (dataTable.Rows.Count == 0)
                return null;

            return MapToEntity(dataTable.Rows[0]);
        }

        public async Task<MALZEME> GetByPartNoAsync(string partNo)
        {
            var query = $"SELECT * FROM [dbo].[MALZEME] WHERE PART_NO = '{partNo}'";
            var dataTable = await _database.ExecuteQueryAsync(query);
            
            if (dataTable.Rows.Count == 0)
                return null;

            return MapToEntity(dataTable.Rows[0]);
        }

        public async Task<List<MALZEME>> GetAllAsync()
        {
            var query = "SELECT * FROM [dbo].[MALZEME] ORDER BY KOD";
            var dataTable = await _database.ExecuteQueryAsync(query);
            
            return dataTable.Rows.Cast<DataRow>()
                .Select(MapToEntity)
                .ToList();
        }

        public async Task<int> InsertAsync(MALZEME malzeme)
        {
            var command = $@"
                INSERT INTO [dbo].[MALZEME] (
                    AD, STOK_MIKTARI, STOK_KONTROL, ACIKLAMA, MINIMUM_MIKTAR, SILO_KAPASITE,
                    AGREGA, CIMENTO, KUL, TOZ, KUM, SU, KATKI, EKSINEMLIMIT,
                    PART_NO, PROD_FAMILY, GKS_YOG, GKS_INCE_YOG
                ) VALUES (
                    '{malzeme.AD ?? ""}', '{malzeme.STOK_MIKTARI ?? ""}', '{malzeme.STOK_KONTROL ?? ""}',
                    '{malzeme.ACIKLAMA ?? ""}', '{malzeme.MINIMUM_MIKTAR ?? ""}', '{malzeme.SILO_KAPASITE ?? ""}',
                    '{malzeme.AGREGA ?? ""}', '{malzeme.CIMENTO ?? ""}', '{malzeme.KUL ?? ""}',
                    '{malzeme.TOZ ?? ""}', '{malzeme.KUM ?? ""}', '{malzeme.SU ?? ""}',
                    '{malzeme.KATKI ?? ""}', '{malzeme.EKSINEMLIMIT ?? ""}',
                    '{malzeme.PART_NO ?? ""}', '{malzeme.PROD_FAMILY ?? ""}',
                    '{malzeme.GKS_YOG ?? ""}', '{malzeme.GKS_INCE_YOG ?? ""}'
                )";

            return await _database.ExecuteNonQueryAsync(command);
        }

        public async Task<int> UpdateAsync(MALZEME malzeme)
        {
            var command = $@"
                UPDATE [dbo].[MALZEME] SET
                    AD = '{malzeme.AD ?? ""}',
                    STOK_MIKTARI = '{malzeme.STOK_MIKTARI ?? ""}',
                    STOK_KONTROL = '{malzeme.STOK_KONTROL ?? ""}',
                    ACIKLAMA = '{malzeme.ACIKLAMA ?? ""}',
                    MINIMUM_MIKTAR = '{malzeme.MINIMUM_MIKTAR ?? ""}',
                    SILO_KAPASITE = '{malzeme.SILO_KAPASITE ?? ""}',
                    PART_NO = '{malzeme.PART_NO ?? ""}',
                    PROD_FAMILY = '{malzeme.PROD_FAMILY ?? ""}'
                WHERE KOD = {malzeme.KOD}";

            return await _database.ExecuteNonQueryAsync(command);
        }

        public async Task<int> DeleteAsync(decimal kod)
        {
            var command = $"DELETE FROM [dbo].[MALZEME] WHERE KOD = {kod}";
            return await _database.ExecuteNonQueryAsync(command);
        }

        private MALZEME MapToEntity(DataRow row)
        {
            return new MALZEME
            {
                KOD = Convert.ToInt32(row["KOD"]),
                AD = row["AD"]?.ToString(),
                STOK_MIKTARI = row["STOK_MIKTARI"]?.ToString(),
                STOK_KONTROL = row["STOK_KONTROL"]?.ToString(),
                ACIKLAMA = row["ACIKLAMA"]?.ToString(),
                MINIMUM_MIKTAR = row["MINIMUM_MIKTAR"]?.ToString(),
                SILO_KAPASITE = row["SILO_KAPASITE"]?.ToString(),
                AGREGA = row["AGREGA"]?.ToString(),
                CIMENTO = row["CIMENTO"]?.ToString(),
                KUL = row["KUL"]?.ToString(),
                TOZ = row["TOZ"]?.ToString(),
                KUM = row["KUM"]?.ToString(),
                SU = row["SU"]?.ToString(),
                KATKI = row["KATKI"]?.ToString(),
                EKSINEMLIMIT = row["EKSINEMLIMIT"]?.ToString(),
                PART_NO = row["PART_NO"]?.ToString(),
                PROD_FAMILY = row["PROD_FAMILY"]?.ToString(),
                GKS_YOG = row["GKS_YOG"]?.ToString(),
                GKS_INCE_YOG = row["GKS_INCE_YOG"]?.ToString()
            };
        }

        
    }
}

