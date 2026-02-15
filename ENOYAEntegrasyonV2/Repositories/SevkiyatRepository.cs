using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ENOYAEntegrasyonV2.DbContxt;
using ENOYAEntegrasyonV2.Models.Entities;
using ENOYAEntegrasyonV2.Repositories.Interfaces;
using ENOYAEntegrasyonV2.Services.Interfaces;

namespace ENOYAEntegrasyonV2.Repositories
{
    /// <summary>
    /// SEVKIYAT repository implementasyonu
    /// </summary>
    public class SevkiyatRepository : ISevkiyatRepository
    {
        private readonly IDatabaseService _database;

        public SevkiyatRepository(IDatabaseService database)
        {
            _database = database;// ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<SEVKIYAT> GetByIdAsync(int kod)
        {
            var query = $"SELECT * FROM [dbo].[SEVKIYAT] WHERE KOD = {kod}";
            var dataTable = await _database.ExecuteQueryAsync(query);
            
            if (dataTable.Rows.Count == 0)
                return null;

            return MapToEntity(dataTable.Rows[0]);
        }

        public async Task<List<SEVKIYAT>> GetAllAsync()
        {
            var query = "SELECT * FROM [dbo].[SEVKIYAT] ORDER BY KOD DESC";
            var dataTable = await _database.ExecuteQueryAsync(query);
            
            return dataTable.Rows.Cast<DataRow>()
                .Select(MapToEntity)
                .ToList();
        }

        public async Task<int> InsertAsync(SEVKIYAT sevkiyat)
        {
            var command = $@"
                INSERT INTO [dbo].[SEVKIYAT] (
                    IRS_SERI_NO, IRS_NO, TARIH, SAAT, TARIHSAAT, MIKTAR, OTOMATIK, KULLANICI,
                    RECETE_KOD, RECETE_INDEX, RECETE_AD, MUSTERI_KOD, MUSTERI_INDEX, MUSTERI_AD,
                    SANTIYE_KOD, SANTIYE_AD, KAMYON_KOD, KAMYON_PLK, SURUCU_KOD, SURUCU_AD,
                    ORDER_NO, RELEASE_NO, SEQUENCE_NO, ORDER_CODE, START_TIME, FINISH_TIME,
                    DURUM, URTSUCIM, URTSUCIMKAT
                ) VALUES (
                    '{sevkiyat.IRS_SERI_NO ?? ""}', '{sevkiyat.IRS_NO ?? ""}', '{sevkiyat.TARIH ?? ""}',
                    '{sevkiyat.SAAT ?? ""}', '{sevkiyat.TARIHSAAT ?? ""}', {sevkiyat.MIKTAR},
                    '{sevkiyat.OTOMATIK ?? ""}', '{sevkiyat.KULLANICI ?? ""}',
                    '{sevkiyat.RECETE_KOD ?? ""}', '{sevkiyat.RECETE_INDEX ?? ""}', '{sevkiyat.RECETE_AD ?? ""}',
                    '{sevkiyat.MUSTERI_KOD ?? ""}', '{sevkiyat.MUSTERI_INDEX ?? ""}', '{sevkiyat.MUSTERI_AD ?? ""}',
                    '{sevkiyat.SANTIYE_KOD ?? ""}', '{sevkiyat.SANTIYE_AD ?? ""}',
                    '{sevkiyat.KAMYON_KOD ?? ""}', '{sevkiyat.KAMYON_PLK ?? ""}',
                    '{sevkiyat.SURUCU_KOD ?? ""}', '{sevkiyat.SURUCU_AD ?? ""}',
                    '{sevkiyat.ORDER_NO ?? ""}', '{sevkiyat.RELEASE_NO ?? ""}', '{sevkiyat.SEQUENCE_NO ?? ""}',
                    '{sevkiyat.ORDER_CODE ?? ""}', '{sevkiyat.START_TIME ?? ""}', '{sevkiyat.FINISH_TIME ?? ""}',
                    '{sevkiyat.DURUM ?? ""}', '{sevkiyat.URTSUCIM ?? ""}', '{sevkiyat.URTSUCIMKAT ?? ""}'
                )";

            return await _database.ExecuteNonQueryAsync(command);
        }

        public async Task<int> UpdateAsync(SEVKIYAT sevkiyat)
        {
            var command = $@"
                UPDATE [dbo].[SEVKIYAT] SET
                    IRS_SERI_NO = '{sevkiyat.IRS_SERI_NO ?? ""}',
                    IRS_NO = '{sevkiyat.IRS_NO ?? ""}',
                    TARIH = '{sevkiyat.TARIH ?? ""}',
                    SAAT = '{sevkiyat.SAAT ?? ""}',
                    TARIHSAAT = '{sevkiyat.TARIHSAAT ?? ""}',
                    MIKTAR = {sevkiyat.MIKTAR},
                    ORDER_NO = '{sevkiyat.ORDER_NO ?? ""}',
                    RELEASE_NO = '{sevkiyat.RELEASE_NO ?? ""}',
                    SEQUENCE_NO = '{sevkiyat.SEQUENCE_NO ?? ""}',
                    DURUM = '{sevkiyat.DURUM ?? ""}'
                WHERE KOD = {sevkiyat.KOD}";

            return await _database.ExecuteNonQueryAsync(command);
        }

        public async Task<int> DeleteAsync(int kod)
        {
            var command = $"DELETE FROM [dbo].[SEVKIYAT] WHERE KOD = {kod}";
            return await _database.ExecuteNonQueryAsync(command);
        }

        public async Task<List<SEVKIYAT>> GetByOrderNoAsync(string orderNo)
        {
            var query = $"SELECT * FROM [dbo].[SEVKIYAT] WHERE ORDER_NO = '{orderNo}' ORDER BY KOD DESC";
            var dataTable = await _database.ExecuteQueryAsync(query);
            
            return dataTable.Rows.Cast<DataRow>()
                .Select(MapToEntity)
                .ToList();
        }

        public async Task<List<SEVKIYAT>> GetUnreportedAsync()
        {
            // DURUM = '' veya NULL olanlar raporlanmamış demektir
            var query = "SELECT * FROM [dbo].[SEVKIYAT] WHERE (DURUM = '2') AND ORDER_NO IS NOT NULL AND ORDER_NO != '' ORDER BY KOD DESC";
            using var ctx = new TesisContext();
            var listt = await ctx.Database.SqlQuery<SEVKIYAT>(
                        query
                    ).ToListAsync();

            //var dataTable = await ctx.Database.R //await _database.ExecuteQueryAsync(query);

            //return dataTable.Rows.Cast<DataRow>()
            //    .Select(MapToEntity)
            //    .ToList();
            return listt;
        }

        private SEVKIYAT MapToEntity(DataRow row)
        {
            return new SEVKIYAT
            {
                KOD = Convert.ToInt32(row["KOD"]),
                IRS_SERI_NO = row["IRS_SERI_NO"]?.ToString(),
                IRS_NO = row["IRS_NO"]?.ToString(),
                TARIH = row["TARIH"]?.ToString(),
                SAAT = row["SAAT"]?.ToString(),
                TARIHSAAT = row["TARIHSAAT"]?.ToString(),
                MIKTAR = row["MIKTAR"] != DBNull.Value ? Convert.ToSingle(row["MIKTAR"]) : 0,
                OTOMATIK = row["OTOMATIK"]?.ToString(),
                KULLANICI = row["KULLANICI"]?.ToString(),
                RECETE_KOD = row["RECETE_KOD"]?.ToString(),
                RECETE_INDEX = row["RECETE_INDEX"]?.ToString(),
                RECETE_AD = row["RECETE_AD"]?.ToString(),
                MUSTERI_KOD = row["MUSTERI_KOD"]?.ToString(),
                MUSTERI_INDEX = row["MUSTERI_INDEX"]?.ToString(),
                MUSTERI_AD = row["MUSTERI_AD"]?.ToString(),
                SANTIYE_KOD = row["SANTIYE_KOD"]?.ToString(),
                SANTIYE_AD = row["SANTIYE_AD"]?.ToString(),
                KAMYON_KOD = row["KAMYON_KOD"]?.ToString(),
                KAMYON_PLK = row["KAMYON_PLK"]?.ToString(),
                SURUCU_KOD = row["SURUCU_KOD"]?.ToString(),
                SURUCU_AD = row["SURUCU_AD"]?.ToString(),
                ORDER_NO = row["ORDER_NO"]?.ToString(),
                RELEASE_NO = row["RELEASE_NO"]?.ToString(),
                SEQUENCE_NO = row["SEQUENCE_NO"]?.ToString(),
                ORDER_CODE = row["ORDER_CODE"]?.ToString(),
                START_TIME = row["START_TIME"]?.ToString(),
                FINISH_TIME = row["FINISH_TIME"]?.ToString(),
                DURUM = row["DURUM"]?.ToString(),
                URTSUCIM = row["URTSUCIM"]?.ToString(),
                URTSUCIMKAT = row["URTSUCIMKAT"]?.ToString()
            };
        }
    }
}

