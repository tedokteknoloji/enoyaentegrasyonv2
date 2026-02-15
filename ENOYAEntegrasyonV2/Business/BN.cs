using DevExpress.Utils.DirectXPaint.Svg;
using ENOYAEntegrasyonV2.DbContxt;
using ENOYAEntegrasyonV2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENOYAEntegrasyonV2.Business
{
    public class BN
    {
        public static string FieldReturnValues(string _fieldValues, int _fieldType)
        {
            string returnValue = "";
            switch (_fieldType)
            {
                case 1:
                    returnValue = "'" + _fieldValues + "'";
                    break;
                case 2:
                    returnValue = _fieldValues.ToString().Replace(',', '.');
                    break;
                case 3:
                    returnValue = _fieldValues.ToString().Replace(',', '.');
                    break;
                case 4:
                    returnValue = "CONVERT(SMALLDATETIME,'" + _fieldValues + "',103)";
                    break;
                case 5:
                    if (_fieldValues == "Checked")
                    {
                        returnValue = "1";
                    }
                    else
                        returnValue = "0";
                    break;
                default:
                    break;
            }
            return returnValue;
        }
        public static string KarakterDuzelt(string _karakterDizesi)
        {
            string _TurkceKarakter = "";
            _TurkceKarakter = _karakterDizesi.Replace("ı", "i");
            _TurkceKarakter = _TurkceKarakter.Replace("ç", "c");
            _TurkceKarakter = _TurkceKarakter.Replace("ö", "o");
            _TurkceKarakter = _TurkceKarakter.Replace("ş", "s");
            _TurkceKarakter = _TurkceKarakter.Replace("ü", "u");
            _TurkceKarakter = _TurkceKarakter.Replace("ğ", "g");
            _TurkceKarakter = _TurkceKarakter.Replace("İ", "I");
            _TurkceKarakter = _TurkceKarakter.Replace("Ç", "C");
            _TurkceKarakter = _TurkceKarakter.Replace("Ö", "O");
            _TurkceKarakter = _TurkceKarakter.Replace("Ş", "S");
            _TurkceKarakter = _TurkceKarakter.Replace("Ü", "U");
            _TurkceKarakter = _TurkceKarakter.Replace("Ğ", "G");
            return _TurkceKarakter;
        }
        public static async Task<HashSet<string>> GetValidPartNosAsync(TesisContext ctx)
        {
            // SILO_AD tablosunda tek satır (KOD = 1) varsayıyorum
            var siloRow = await ctx.SILO_ADs
                                   .SingleAsync(x => x.KOD.Equals("1"));

            var validPartNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // SiloAdlari listesinde her bunker kolonunun adı var
            foreach (var siloDef in SiloAdlari.listSiloAdlari())
            {
                // Örn: "AGREGA1", "CIMENTO1" vs.
                var columnName = siloDef.IDCOLUMNNAME.ToString();

                // Reflection ile SILO_AD entity'sindeki property’yi bul
                var prop = siloRow.GetType().GetProperty(columnName);
                if (prop == null) continue; // kolon yoksa atla (örneğin eski tablo vs.)

                var value = prop.GetValue(siloRow) as string;

                // 0, boş, -1 gibi kullanılmayanları hariç tut
                if (!string.IsNullOrWhiteSpace(value) && value != "0" && value != "-1")
                {
                    validPartNos.Add(value);
                }
            }

            return validPartNos;
        }
        public static async Task<SiloValidationResult> ValidatePlanLinesAsync(
            TesisContext ctx,
            List<IFSPLANLine> planLines)
        {
            var result = new SiloValidationResult();

            // 1) SILO_AD’dan geçerli hammadde kodlarını al
            var validPartNos = await GetValidPartNosAsync(ctx);

            // 2) Geçersiz satırları bul (ComponentPartNo, SILO_AD içinde yoksa)
            var invalidLines = planLines
                .Where(l => !string.IsNullOrEmpty(l.ComponentPartNo))
                .Where(l => !validPartNos.Contains(l.ComponentPartNo))
                .ToList();

            if (!invalidLines.Any())
            {
                result.IsValid = true;
                result.Message = "Tüm sipariş satırları SILO_AD hammadde kodlarıyla uyumlu.";
                return result;
            }

            // 3) Sipariş bazlı özet mesaj
            var grouped = invalidLines
                .GroupBy(l => l.OrderNo)
                .Select(g => new
                {
                    OrderNo = g.Key,
                    Parts = g.Select(x => x.ComponentPartNo).Distinct().ToList()
                })
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Aşağıdaki siparişlerde SILO_AD ile uyuşmayan hammadde kodları var:");
            foreach (var g in grouped)
            {
                sb.AppendLine($"Sipariş No: {g.OrderNo} → Uyuşmayan Kodlar: {string.Join(", ", g.Parts)}");
            }

            result.IsValid = false;
            result.Message = sb.ToString();
            result.InvalidLines = invalidLines;
            return result;
        }

        public static async Task<SiloValidationResult> ValidateSiloadAsync(TesisContext ctx)
        {
            var result = new SiloValidationResult();

            // 1) SILO_AD’dan geçerli hammadde kodlarını al
            var validPartNos = await GetValidPartNosAsync(ctx);

            // 2) Geçersiz satırları bul (ComponentPartNo, SILO_AD içinde yoksa)
            var invalidLines = SiloAdlari.siloTanimlari()
                .Where(l => !string.IsNullOrEmpty(l.IFSPARTNO) && !l.IFSPARTNO.ToUpper().Equals("-1"))
                .Where(l => !validPartNos.Contains(l.IFSPARTNO))
                .ToList();

            if (!invalidLines.Any())
            {
                result.IsValid = true;
                result.Message = "Tüm silo tanımları SILO_AD hammadde kodlarıyla uyumlu.";
                return result;
            }

            //// 3) Sipariş bazlı özet mesaj
            var grouped = invalidLines
                .GroupBy(l => l.IFSPARTNO)
                .Select(g => new
                {
                    OrderNo = g.Key,
                    Parts = g.Select(x => x.IFSPARTNO).Distinct().ToList()
                })
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Aşağıdaki siparişlerde SILO_AD ile uyuşmayan hammadde kodları var:");
            foreach (var g in grouped)
            {
                sb.AppendLine($"Sipariş No: {g.OrderNo} → Uyuşmayan Kodlar: {string.Join(", ", g.Parts)}");
            }

            result.IsValid = false;
            result.Message = sb.ToString();
            result.InvalidLines = null;
            return result;
        }

        public static async Task<SiloValidationResult> ValidateCimentoKulAsync(TesisContext ctx)
        {
            var result = new SiloValidationResult();

            // 1) SILO_AD’dan geçerli hammadde kodlarını al
            var validPartNos = await GetValidPartNosAsync(ctx);

            // 2) Geçersiz satırları bul (ComponentPartNo, SILO_AD içinde yoksa)
            var invalidLines = SiloAdlari.siloTanimlari()
                .Where(l => !string.IsNullOrEmpty(l.IFSPARTNO) && !l.IFSPARTNO.ToUpper().Equals("-1"))
                .Where(l => !validPartNos.Contains(l.IFSPARTNO))
                .ToList();

            if (!invalidLines.Any())
            {
                result.IsValid = true;
                result.Message = "Tüm silo tanımları SILO_AD hammadde kodlarıyla uyumlu.";
                return result;
            }

            //// 3) Sipariş bazlı özet mesaj
            var grouped = invalidLines
                .GroupBy(l => l.IFSPARTNO)
                .Select(g => new
                {
                    OrderNo = g.Key,
                    Parts = g.Select(x => x.IFSPARTNO).Distinct().ToList()
                })
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Aşağıdaki siparişlerde SILO_AD ile uyuşmayan hammadde kodları var:");
            foreach (var g in grouped)
            {
                sb.AppendLine($"Sipariş No: {g.OrderNo} → Uyuşmayan Kodlar: {string.Join(", ", g.Parts)}");
            }

            result.IsValid = false;
            result.Message = sb.ToString();
            result.InvalidLines = null;
            return result;
        }

    }
    public class SiloValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public List<IFSPLANLine> InvalidLines { get; set; } = new();
    }
    public enum enumBunkerTRField
    {
        AGREGA1_VER = 1,
        AGREGA2_VER = 2,
        AGREGA3_VER = 3,
        AGREGA4_VER = 4,
        AGREGA5_VER = 5,
        AGREGA6_VER = 6,
        CIMENTO1_VER = 7,
        CIMENTO2_VER = 8,
        CIMENTO3_VER = 9,
        CIMENTO4_VER = 10,
        SU1_VER = 11,
        SU2_VER = 12,
        KATKI1_VER = 13,
        KATKI2_VER = 14,
        KATKI3_VER = 15,
        KATKI4_VER = 16
    }
    public enum enumBunkerItmPartNo
    {
        AG1_PARTNO = 1,
        AG2_PARTNO = 2,
        AG3_PARTNO = 3,
        AG4_PARTNO = 4,
        AG5_PARTNO = 5,
        AG6_PARTNO = 6,
        CM1_PARTNO = 7,
        CM2_PARTNO = 8,
        CM3_PARTNO = 9,
        CM4_PARTNO = 10,
        SU1_PARTNO = 11,
        SU2_PARTNO = 12,
        KT1_PARTNO = 13,
        KT2_PARTNO = 14,
        KT3_PARTNO = 15,
        KT4_PARTNO = 16
    }
    public enum enumBunkerPercentField
    {
        AG1BLMYZDE = 1,
        AG2BLMYZDE = 2,
        AG3BLMYZDE = 3,
        AG4BLMYZDE = 4,
        AG5BLMYZDE = 5,
        AG6BLMYZDE = 6
    }
    public enum enumBunkerRAdjustQuantity
    {
        AG1PGKSUTOP = 1,
        AG2PGKSUTOP = 2,
        AG3PGKSUTOP = 3,
        AG4PGKSUTOP = 4,
        AG5PGKSUTOP = 5,
        AG6PGKSUTOP = 6
    }
    public enum enumBunkerRevisedHumidity
    {
        AG1PNEMSUTOP = 1,
        AG2PNEMSUTOP = 2,
        AG3PNEMSUTOP = 3,
        AG4PNEMSUTOP = 4,
        AG5PNEMSUTOP = 5,
        AG6PNEMSUTOP = 6
    }
    public enum enumBunkerRecycleDensity
    {
        AG1PGKORT = 1,
        AG2PGKORT = 2,
        AG3PGKORT = 3,
        AG4PGKORT = 4,
        AG5PGKORT = 5,
        AG6PGKORT = 6
    }
    public enum enumBunkerHumidity
    {
        AG1PNEMORT = 1,
        AG2PNEMORT = 2,
        AG3PNEMORT = 3,
        AG4PNEMORT = 4,
        AG5PNEMORT = 5,
        AG6PNEMORT = 6
    }
    public enum enumBunkerIssueQuantity
    {
        AG1PVER = 1,
        AG2PVER = 2,
        AG3PVER = 3,
        AG4PVER = 4,
        AG5PVER = 5,
        AG6PVER = 6,
        CM1PVER = 7,
        CM2PVER = 8,
        CM3PVER = 9,
        CM4PVER = 10,
        SU1PVER = 11,
        SU2PVER = 12,
        KT1PVER = 13,
        KT2PVER = 14,
        KT3PVER = 15,
        KT4PVER = 16
    }
    public enum enumBunkerIfsData
    {
        Nem_Hesabı_Yok = 0,
        Nem_HesabıVar = 1
    }
    public enum enumBunkerIcerikTipi
    {
        Agrega = 1,
        Çimento_Kül = 2,
        Su1 = 3,
        Su2 = 4,
        Katkı = 5
    }
    //Bunker tanımının ENOYA ile IFS arasındaki eşleşmesi için kullanılacak ITEMNO eşleşmesi ve değer
    public enum enumBunkerItemAlanAdi
    {
        AG1_ITEMNO = 1,
        AG2_ITEMNO = 2,
        AG3_ITEMNO = 3,
        AG4_ITEMNO = 4,
        AG5_ITEMNO = 5,
        AG6_ITEMNO = 6,
        CM1_ITEMNO = 7,
        CM2_ITEMNO = 8,
        CM3_ITEMNO = 9,
        CM4_ITEMNO = 10,
        SU1_ITEMNO = 11,
        SU2_ITEMNO = 12,
        KT1_ITEMNO = 13,
        KT2_ITEMNO = 14,
        KT3_ITEMNO = 15,
        KT4_ITEMNO = 16
    }
    // Bunker tanımının ENOYA tablolarındaki ISTENEN kolon adları ve değer
    public enum enumBunkerIstenenAlanAdi
    {
        AG1_IST = 1,
        AG2_IST = 2,
        AG3_IST = 3,
        AG4_IST = 4,
        AG5_IST = 5,
        AG6_IST = 6,
        CM1_IST = 7,
        CM2_IST = 8,
        CM3_IST = 9,
        CM4_IST = 10,
        SU1_IST = 11,
        SU2_IST = 12,
        KT1_IST = 13,
        KT2_IST = 14,
        KT3_IST = 15,
        KT4_IST = 16
    }
    //Bunker tanımının ENOYA tablolarındaki kolon adları ve değer
    public enum enumBunkerKolonAdi
    {
        AGREGA1 = 1,
        AGREGA2 = 2,
        AGREGA3 = 3,
        AGREGA4 = 4,
        AGREGA5 = 5,
        AGREGA6 = 6,
        CIMENTO1 = 7,
        CIMENTO2 = 8,
        CIMENTO3 = 9,
        CIMENTO4 = 10,
        SU1 = 11,
        SU2 = 12,
        KATKI1 = 13,
        KATKI2 = 14,
        KATKI3 = 15,
        KATKI4 = 16
    }
    // Bunker tanımındaki görünen açıklama ve değer
    public enum enumBunkerTanimi
    {
        Agrega_Bunkeri_1 = 1,
        Agrega_Bunkeri_2 = 2,
        Agrega_Bunkeri_3 = 3,
        Agrega_Bunkeri_4 = 4,
        Agrega_Bunkeri_5 = 5,
        Agrega_Bunkeri_6 = 6,
        Çimento_Silo_1 = 7,
        Çimento_Silo_2 = 8,
        Çimento_Silo_3 = 9,
        Çimento_Silo_4 = 10,
        Su_1 = 11,
        Su_2 = 12,
        Katkı_Bunkeri_1 = 13,
        Katkı_Bunkeri_2 = 14,
        Katkı_Bunkeri_3 = 15,
        Katkı_Bunkeri_4 = 16
    }

    public class KolonMalzemeEsleme
    {
        public string? IFSPARTNO { get; set; }
        public string? ALAN { get; set; }
        public int? YUZDE { get; set; }
        public int? SAYI { get; set; }
    }

    public class GonderimEsleme
    {
        public Int16? REFID { get; set; }
        public String? NAMEIST { get; set; }
        public Double? AMOUNT { get; set; }
        public String? NAMEITEM { get; set; }
        public Int16? ITEMNO { get; set; }
        public Int16? FTYPE { get; set; }
        public String? ITMPARTNO { get; set; }
        public String? PARTNO { get; set; }
    }

}
