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
    /// IRSALIYE repository implementasyonu
    /// </summary>
    public class IrsaliyeRepository : IIrsaliyeRepository
    {
        private readonly IDatabaseService _database;

        public IrsaliyeRepository(IDatabaseService database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<Irsaliye> GetByIdAsync(decimal kod)
        {
            // SQL Injection koruması için parametreli sorgu kullanılmalı
            // Şimdilik basit implementasyon, production'da SqlParameter kullanılmalı
            var query = $"SELECT * FROM [dbo].[IRSALIYE] WHERE KOD = {kod}";
            var dataTable = await _database.ExecuteQueryAsync(query);
            
            if (dataTable.Rows.Count == 0)
                return null;

            return MapToEntity(dataTable.Rows[0]);
        }

        public async Task<List<Irsaliye>> GetAllAsync()
        {
            var query = "SELECT * FROM [dbo].[IRSALIYE] ORDER BY KOD DESC";
            var dataTable = await _database.ExecuteQueryAsync(query);
            
            return dataTable.Rows.Cast<DataRow>()
                .Select(MapToEntity)
                .ToList();
        }

        public async Task<int> InsertAsync(Irsaliye irsaliye)
        {
            // NOT: Production'da SqlParameter kullanılmalı (SQL Injection koruması)
            // Şimdilik basit string replacement ile escape ediliyor
            var escape = new Func<string, string>(s => (s ?? "").Replace("'", "''"));
            
            var command = $@"
                INSERT INTO [dbo].[IRSALIYE] (
                    IRS_SERI_NO, IRS_NO, TARIH, SAAT, TARIHSAAT, MIKTAR, OTOMATIK, KULLANICI,
                    RECETE_KOD, RECETE_INDEX, RECETE_AD, MUSTERI_KOD, MUSTERI_INDEX, MUSTERI_AD,
                    SANTIYE_KOD, SANTIYE_AD, KAMYON_KOD, KAMYON_PLK, SURUCU_KOD, SURUCU_AD,
                    AGREGA1_IST, AGREGA1_TART, AGREGA1_VER, AGREGA2_IST, AGREGA2_TART, AGREGA2_VER,
                    CIMENTO1_IST, CIMENTO1_VER, SU1_IST, SU1_TART, SU1_VER,
                    IFSORDER_NO, URTSUCIM, URTSUCIMKAT
                ) VALUES (
                    '{escape(irsaliye.IRS_SERI_NO)}', '{escape(irsaliye.IRS_NO)}', '{escape(irsaliye.TARIH)}', 
                    '{escape(irsaliye.SAAT)}', '{escape(irsaliye.TARIHSAAT)}', '{escape(irsaliye.MIKTAR)}',
                    '{escape(irsaliye.OTOMATIK)}', '{escape(irsaliye.KULLANICI)}',
                    '{escape(irsaliye.RECETE_KOD)}', '{escape(irsaliye.RECETE_INDEX)}', '{escape(irsaliye.RECETE_AD)}',
                    '{escape(irsaliye.MUSTERI_KOD)}', '{escape(irsaliye.MUSTERI_INDEX)}', '{escape(irsaliye.MUSTERI_AD)}',
                    '{escape(irsaliye.SANTIYE_KOD)}', '{escape(irsaliye.SANTIYE_AD)}',
                    '{escape(irsaliye.KAMYON_KOD)}', '{escape(irsaliye.KAMYON_PLK)}',
                    '{escape(irsaliye.SURUCU_KOD)}', '{escape(irsaliye.SURUCU_AD)}',
                    '{escape(irsaliye.AGREGA1_IST)}', '{escape(irsaliye.AGREGA1_TART)}', '{escape(irsaliye.AGREGA1_VER)}',
                    '{escape(irsaliye.AGREGA2_IST)}', '{escape(irsaliye.AGREGA2_TART)}', '{escape(irsaliye.AGREGA2_VER)}',
                    '{escape(irsaliye.CIMENTO1_IST)}', '{escape(irsaliye.CIMENTO1_VER)}',
                    '{escape(irsaliye.SU1_IST)}', '{escape(irsaliye.SU1_TART)}', '{escape(irsaliye.SU1_VER)}',
                    '{escape(irsaliye.IFSORDER_NO)}', '{escape(irsaliye.URTSUCIM)}', '{escape(irsaliye.URTSUCIMKAT)}'
                )";

            return await _database.ExecuteNonQueryAsync(command);
        }

        public async Task<int> UpdateAsync(Irsaliye irsaliye)
        {
            var command = $@"
                UPDATE [dbo].[IRSALIYE] SET
                    IRS_SERI_NO = '{irsaliye.IRS_SERI_NO ?? ""}',
                    IRS_NO = '{irsaliye.IRS_NO ?? ""}',
                    TARIH = '{irsaliye.TARIH ?? ""}',
                    SAAT = '{irsaliye.SAAT ?? ""}',
                    TARIHSAAT = '{irsaliye.TARIHSAAT ?? ""}',
                    MIKTAR = '{irsaliye.MIKTAR ?? ""}',
                    RECETE_KOD = '{irsaliye.RECETE_KOD ?? ""}',
                    MUSTERI_KOD = '{irsaliye.MUSTERI_KOD ?? ""}',
                    IFSORDER_NO = '{irsaliye.IFSORDER_NO ?? ""}'
                WHERE KOD = {irsaliye.KOD}";

            return await _database.ExecuteNonQueryAsync(command);
        }

        public async Task<int> DeleteAsync(decimal kod)
        {
            var command = $"DELETE FROM [dbo].[IRSALIYE] WHERE KOD = {kod}";
            return await _database.ExecuteNonQueryAsync(command);
        }

        public async Task<List<Irsaliye>> GetByOrderNoAsync(string orderNo)
        {
            var query = $"SELECT * FROM [dbo].[IRSALIYE] WHERE IFSORDER_NO = '{orderNo}' ORDER BY KOD DESC";
            var dataTable = await _database.ExecuteQueryAsync(query);
            
            return dataTable.Rows.Cast<DataRow>()
                .Select(MapToEntity)
                .ToList();
        }

        private Irsaliye MapToEntity(DataRow row)
        {
            return new Irsaliye
            {
                KOD = Convert.ToDecimal(row["KOD"]),
                IRS_SERI_NO = row["IRS_SERI_NO"]?.ToString(),
                IRS_NO = row["IRS_NO"]?.ToString(),
                TARIH = row["TARIH"]?.ToString(),
                SAAT = row["SAAT"]?.ToString(),
                TARIHSAAT = row["TARIHSAAT"]?.ToString(),
                MIKTAR = row["MIKTAR"]?.ToString(),
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
                AGREGA1_IST = row["AGREGA1_IST"]?.ToString(),
                AGREGA1_TART = row["AGREGA1_TART"]?.ToString(),
                AGREGA1_VER = row["AGREGA1_VER"]?.ToString(),
                AGREGA2_IST = row["AGREGA2_IST"]?.ToString(),
                AGREGA2_TART = row["AGREGA2_TART"]?.ToString(),
                AGREGA2_VER = row["AGREGA2_VER"]?.ToString(),
                CIMENTO1_IST = row["CIMENTO1_IST"]?.ToString(),
                CIMENTO1_VER = row["CIMENTO1_VER"]?.ToString(),
                SU1_IST = row["SU1_IST"]?.ToString(),
                SU1_TART = row["SU1_TART"]?.ToString(),
                SU1_VER = row["SU1_VER"]?.ToString(),
                IFSORDER_NO = row["IFSORDER_NO"]?.ToString(),
                URTSUCIM = row["URTSUCIM"]?.ToString(),
                URTSUCIMKAT = row["URTSUCIMKAT"]?.ToString()
            };
        }
    }
}

