using ENOYAEntegrasyonV2.Business;
using ENOYAEntegrasyonV2.DbContxt;
using ENOYAEntegrasyonV2.Models.Entities;
using ENOYAEntegrasyonV2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ENOYAEntegrasyonV2.Services
{
    public class UretimService
    {
        private readonly IRestApiService restApiService;
        private readonly ILoggerService _logger;
        
        public UretimService(IRestApiService restApiService, ILoggerService logger)
        {
            this.restApiService = restApiService;
            _logger = logger;
        }
        
        public async Task UretimVerisiGonder()
        {
            using (var ctx = new TesisContext())
            {
                // 1) Gönderilecek sevkiyatlar
                var productionList = ctx.SEVKIYATs
                    .Where(s => s.DURUM == "1" && s.HURDA_INFO == "0")
                    .OrderBy(s => s.KOD)
                    .ToList();

                if (!productionList.Any())
                    return;

                // Silo tanımları (SiloAdlari.listSiloAdlari() mevcut)
                var siloList = SiloAdlari.listSiloAdlari();

                foreach (var sev in productionList)
                {
                    if (sev.ORDER_CODE == "M")
                    {
                        GonderMKaydi(ctx, sev, siloList);
                    }
                    else if (sev.ORDER_CODE == "F")
                    {
                        // TODO: İstersen eski F gönderim kodunu buraya taşıyabiliriz.
                        // Şimdilik M kayıtlarını düzelttik.
                    }

                    // Durum güncellemeleri
                    ctx.Database.ExecuteSqlCommand(
                        "UPDATE SEVKIYAT SET DURUM = '2' WHERE ORDER_NO = @p0 AND KOD = @p1",
                        sev.ORDER_NO, sev.KOD);

                    ctx.Database.ExecuteSqlCommand(
                        "UPDATE IFSPLAN SET DURUM = '4' WHERE ORDER_NO = @p0",
                        sev.ORDER_NO);
                }
            }
        }

        // M tipi siparişlerin gönderimi (güncellenmiş, periyot bazlı hesaplama)
        private async void GonderMKaydi(TesisContext ctxA, SEVKIYAT sev, List<SiloAdlari> siloList)
        {
            try
            {
                using var ctx = new TesisContext();
                // İlgili plan kaydı
                var plan = await ctx.IFSPLANs.FirstOrDefaultAsync(p => p.ORDER_NO == sev.ORDER_NO);
                if (plan == null)
                {
                    MessageBox.Show($"IFSPLAN tablosunda {sev.ORDER_NO} nolu sipariş bulunamadı.");
                    return;
                }

                // Bu sevkiyata ait periyot detayları
                //var periyotDetaylar = ctx.PERDETAYs
                //    .Where(p => p.URTKOD == sev.KOD)
                //    .OrderBy(p => p.PERKOD)
                //    .ToList();

                var periyotDetaylar = await ctx.Database.SqlQuery<PERDETAY>(
                            "SELECT * FROM PERDETAY WHERE URTKOD = '" + sev.KOD + "' ORDER BY PERKOD "
                        ).ToListAsync();


                if (!periyotDetaylar.Any())
                {
                    MessageBox.Show($"PERDETAY tablosunda {sev.ORDER_NO} / KOD={sev.KOD} için kayıt yok.");
                    return;
                }

                // Hangi ITEMNO'ların aktif line olduğunu çıkar
                var lineNumbers = new SortedSet<int>();
                foreach (var silo in siloList)
                {
                    if (!silo.NAMEITM.HasValue)
                        continue;

                    var itemProp = typeof(IFSPLAN).GetProperty(silo.NAMEITM.Value.ToString());
                    if (itemProp == null)
                        continue;

                    var valStr = itemProp.GetValue(plan) as string;
                    if (int.TryParse(valStr, out var lineNo) && lineNo > 0)
                    {
                        lineNumbers.Add(lineNo);
                    }
                }

                // SU1 satırının line numarası (su satırının tespiti)
                int? su1LineNo = null;
                if (int.TryParse(plan.SU1_ITEMNO, out var su1Ln) && su1Ln > 0)
                    su1LineNo = su1Ln;

                double toplamMiktar = 0;// plan.MIKTAR.ToDbl();
                double periyotMiktar = 0;
                // Her PERDETAY satırı (periyot) için
                for (int perIndex = 0; perIndex < periyotDetaylar.Count; perIndex++)
                {
                    var per = periyotDetaylar[perIndex];
                    toplamMiktar = toplamMiktar + sev.MIXCAP.Replace('.', ',').ToDbl();
                    periyotMiktar = sev.MIXCAP.Replace('.', ',').ToDbl();
                    //_logger.LogDebug($"{perIndex}-{periyotMiktar}-{toplamMiktar}-{plan.MIKTAR}");
                    if (perIndex == periyotDetaylar.Count - 1)//(((plan.MIKTAR.ToDbl() - toplamMiktar) < periyotMiktar) && )
                    {
                        //_logger.LogDebug($"Bingo _ {perIndex}-{periyotMiktar}-{toplamMiktar}-{plan.MIKTAR}");
                        periyotMiktar = Math.Round(((plan.MIKTAR.ToDbl() - toplamMiktar) + periyotMiktar), 2);
                    }

                    // Baş kısım (sadece periyot bazında bir kez)
                    int mixingTime = 0;
                    if (int.TryParse(per.PMIXTIME, out var mt))
                        mixingTime = mt / 100;
                    //double mixCap = sev.MIXCAP.ToDbl();
                    string queryText = "";
                    queryText =
                            $"ORDER_NO^{sev.ORDER_NO}~" +
                            $"RELEASE_NO^{sev.RELEASE_NO}~" +
                            $"SEQUENCE_NO^{sev.SEQUENCE_NO}~" +
                            $"START_TIME^{sev.START_TIME}~" +
                            $"FINISH_TIME^{sev.FINISH_TIME}~" +
                            $"PERIOD_VALUE^{(perIndex + 1)}~" +
                            $"MIXING_TIME^{mixingTime.ToString().Replace(',', '.')}~" +
                            $"RECYCLE_WATER_QTY^{per.SU2PVER}~" +
                            $"RESET_INFO^{sev.RESET_INFO}~" +
                            $"CLEANING_INFO^{sev.HURDA_INFO}~" +
                            $"QTY_COMPLETED^{periyotMiktar.ToString().Replace(",", ".")}~"; // sev.MIXCAP.Replace(',', '.')
                    //if (perIndex < periyotDetaylar.Count - 1)
                    //    queryText =
                    //        $"ORDER_NO^{sev.ORDER_NO}~" +
                    //        $"RELEASE_NO^{sev.RELEASE_NO}~" +
                    //        $"SEQUENCE_NO^{sev.SEQUENCE_NO}~" +
                    //        $"START_TIME^{sev.START_TIME}~" +
                    //        $"FINISH_TIME^{sev.FINISH_TIME}~" +
                    //        $"PERIOD_VALUE^{(perIndex + 1)}~" +
                    //        $"MIXING_TIME^{mixingTime.ToString().Replace(',', '.')}~" +
                    //        $"RECYCLE_WATER_QTY^{per.SU2PVER}~" +
                    //        $"RESET_INFO^{sev.RESET_INFO}~" +
                    //        $"CLEANING_INFO^{sev.HURDA_INFO}~" +
                    //        $"QTY_COMPLETED^{periyotMiktar.ToString().Replace(",", ".")}~"; // sev.MIXCAP.Replace(',', '.')
                    //else
                    //    queryText =
                    //        $"ORDER_NO^{sev.ORDER_NO}~" +
                    //        $"RELEASE_NO^{sev.RELEASE_NO}~" +
                    //        $"SEQUENCE_NO^{sev.SEQUENCE_NO}~" +
                    //        $"START_TIME^{sev.START_TIME}~" +
                    //        $"FINISH_TIME^{sev.FINISH_TIME}~" +
                    //        $"PERIOD_VALUE^{(perIndex + 1)}~" +
                    //        $"MIXING_TIME^{mixingTime.ToString().Replace(',', '.')}~" +
                    //        $"RECYCLE_WATER_QTY^{per.SU2PVER}~" +
                    //        $"RESET_INFO^{sev.RESET_INFO}~" +
                    //        $"CLEANING_INFO^{sev.HURDA_INFO}~" +
                    //        $"QTY_COMPLETED^{kalanMiktar.ToString().Replace(",", ".")}~"; // sev.MIXCAP.Replace(',', '.')

                    // Her line (ITEMNO) için
                    foreach (var lineNo in lineNumbers)
                    {
                        bool isWaterLine = su1LineNo.HasValue && su1LineNo.Value == lineNo;

                        // === LINE_ITEM_NO ===
                        queryText += $"LINE_ITEM_NO_{lineNo}^{lineNo}~";

                        // === ISSUE_QUANTITY (AGxPVER + AGyPVER, CMxPVER, SUxPVER, KTxPVER) ===
                        double issueQuantity = 0;

                        foreach (var silo in siloList)
                        {
                            if (!silo.NAMEITM.HasValue || !silo.ISSUEQUANTITY.HasValue)
                                continue;

                            // Bu silonun IFSPLAN’daki ITEMNO kolonu (AG3_ITEMNO, AG4_ITEMNO, ...)
                            var itemProp = typeof(IFSPLAN).GetProperty(silo.NAMEITM.Value.ToString());
                            if (itemProp == null)
                                continue;

                            var valStr = itemProp.GetValue(plan) as string;
                            if (!int.TryParse(valStr, out var itemLineNo) || itemLineNo != lineNo)
                                continue; // Bu silo bu line’a ait değil

                            // Bu silonun PERDETAY tarafındaki ISSUE_QUANTITY alanı (AG3PVER, AG4PVER vs.)
                            var issueProp = typeof(PERDETAY).GetProperty(silo.ISSUEQUANTITY.Value.ToString());
                            if (issueProp == null)
                                continue;

                            var issueVal = issueProp.GetValue(per);
                            issueQuantity += ConvertToDouble(issueVal);
                        }

                        double kalan = issueQuantity % 1;
                        double tamSayi = issueQuantity - kalan;
                        string kalanStr = "";

                        if (kalan > 0)
                        {
                            kalanStr = Math.Round(kalan, 2).ToString().Replace(',', '.'); // "0.xx"
                            if (kalanStr.StartsWith("0"))
                                kalanStr = kalanStr.Substring(1); // ".xx"
                        }

                        string issueQtyStr = (kalan > 0
                                ? Math.Round(tamSayi, 0).ToString() + kalanStr
                                : Math.Round(tamSayi, 0).ToString())
                            .Replace(',', '.');

                        queryText += $"ISSUE_QTY_{lineNo}^{issueQtyStr}~";

                        // === RECYCLE_DENSITY === (su satırı için agrega ortalaması / 10000)
                        double recycleDensityWater = 0;
                        if (isWaterLine)
                        {
                            var densities = new List<double>();

                            foreach (var silo in siloList.Where(x => x.CONTYPE == enumBunkerIcerikTipi.Agrega))
                            {
                                if (!silo.RECYCLEDENSITY.HasValue)
                                    continue;

                                var recProp = typeof(PERDETAY).GetProperty(silo.RECYCLEDENSITY.Value.ToString());
                                if (recProp == null)
                                    continue;

                                var recVal = recProp.GetValue(per);
                                var d = ConvertToDouble(recVal);
                                if (d != 0)
                                    densities.Add(d);
                            }

                            if (densities.Count > 0)
                                recycleDensityWater = densities.Average() / 10000.0;
                        }

                        queryText += $"RECYCLE_DENSITY_{lineNo}^{(isWaterLine ? recycleDensityWater.ToString().Replace(',', '.') : "0")}~";

                        // === RECYCLE_ADJUST_QTY ===
                        double recycleAdjustQuantity = 0;

                        if (!isWaterLine)
                        {
                            // Bu line’a bağlı, nem hesabı olan silolardaki RADJUSTQUANTITY alanlarını topla /10
                            foreach (var silo in siloList.Where(x => x.IFSDATA == enumBunkerIfsData.Nem_HesabıVar))
                            {
                                if (!silo.NAMEITM.HasValue || !silo.RADJUSTQUANTITY.HasValue)
                                    continue;

                                if (!SiloBelongsToLine(plan, silo, lineNo))
                                    continue;

                                var radjProp = typeof(PERDETAY).GetProperty(silo.RADJUSTQUANTITY.Value.ToString());
                                if (radjProp == null)
                                    continue;

                                var radjVal = radjProp.GetValue(per);
                                recycleAdjustQuantity += ConvertToDouble(radjVal) / 10.0;
                            }
                        }
                        else
                        {
                            // Su satırı: tüm agrega silolarının RADJUSTQUANTITY ortalaması /10, sonra -1 ile çarp
                            var vals = new List<double>();
                            foreach (var silo in siloList.Where(x => x.CONTYPE == enumBunkerIcerikTipi.Agrega))
                            {
                                if (!silo.RADJUSTQUANTITY.HasValue)
                                    continue;

                                var radjProp = typeof(PERDETAY).GetProperty(silo.RADJUSTQUANTITY.Value.ToString());
                                if (radjProp == null)
                                    continue;

                                var radjVal = radjProp.GetValue(per);
                                var d = ConvertToDouble(radjVal);
                                if (d != 0)
                                    vals.Add(d);
                            }

                            if (vals.Count > 0)
                                recycleAdjustQuantity = vals.Average() / 10.0;

                            recycleAdjustQuantity *= -1; // su satırı için negatif
                        }

                        queryText += $"RECYCLE_ADJUST_QTY_{lineNo}^{recycleAdjustQuantity.ToString().Replace(',', '.')}~";

                        // === EXTRA_WATER_QTY === (ILVSUA - ILVSUE)
                        double extraWaterQuantity =
                            ConvertToDouble(per.ILVSUA) - ConvertToDouble(per.ILVSUE);

                        queryText += $"EXTRA_WATER_QTY_{lineNo}^{extraWaterQuantity.ToString().Replace(',', '.')}~";

                        // === HUMIDITY ===
                        double needLineAmount = 0;
                        double needTotalAmount = 0;

                        foreach (var silo in siloList.Where(x => x.IFSDATA == enumBunkerIfsData.Nem_HesabıVar))
                        {
                            if (!silo.NAMEITM.HasValue || !silo.NAMEIST.HasValue || !silo.HUMIDITY.HasValue)
                                continue;

                            if (!SiloBelongsToLine(plan, silo, lineNo))
                                continue;

                            var needProp = typeof(IFSPLAN).GetProperty(silo.NAMEIST.Value.ToString());
                            var humProp = typeof(PERDETAY).GetProperty(silo.HUMIDITY.Value.ToString());
                            if (needProp == null || humProp == null)
                                continue;

                            var needVal = needProp.GetValue(plan);
                            var humVal = humProp.GetValue(per);

                            double needD = ConvertToDouble(needVal);
                            double humD = ConvertToDouble(humVal);

                            needLineAmount += (needD * humD / 1000.0);
                            needTotalAmount += needD;
                        }

                        double humidity = 0;
                        if (needLineAmount != 0 && needTotalAmount != 0)
                            humidity = (needLineAmount / needTotalAmount) * 100.0;

                        queryText += $"HUMIDITY_{lineNo}^{Math.Round(humidity, 2).ToString().Replace(',', '.')}~";

                        // === REVISED_HUMIDITY ===
                        double revisedHumidity = 0;

                        if (!isWaterLine)
                        {
                            foreach (var silo in siloList.Where(x => x.IFSDATA == enumBunkerIfsData.Nem_HesabıVar))
                            {
                                if (!silo.NAMEITM.HasValue || !silo.REVISEDHUMIDITY.HasValue)
                                    continue;

                                if (!SiloBelongsToLine(plan, silo, lineNo))
                                    continue;

                                var revProp = typeof(PERDETAY).GetProperty(silo.REVISEDHUMIDITY.Value.ToString());
                                if (revProp == null)
                                    continue;

                                var revVal = revProp.GetValue(per);
                                revisedHumidity += ConvertToDouble(revVal);
                            }

                            queryText += $"REVISED_HUMIDITY_{lineNo}^{Math.Round(revisedHumidity).ToString().Replace(',', '.')}~";
                        }
                        else
                        {
                            double gRevisedHumidity = 0;
                            foreach (var silo in siloList.Where(x => x.CONTYPE == enumBunkerIcerikTipi.Agrega && x.REVISEDHUMIDITY.HasValue))
                            {
                                var revProp = typeof(PERDETAY).GetProperty(silo.REVISEDHUMIDITY.Value.ToString());
                                if (revProp == null)
                                    continue;

                                var revVal = revProp.GetValue(per);
                                gRevisedHumidity += ConvertToDouble(revVal);
                            }

                            var revStr = Math.Round(gRevisedHumidity * -1, 2).ToString().Replace(',', '.');
                            queryText += $"REVISED_HUMIDITY_{lineNo}^{revStr}~";
                        }
                    }

                    // END_FLAG
                    if (perIndex < periyotDetaylar.Count - 1)
                        queryText += "END_FLAG^FALSE~";
                    else
                        queryText += "END_FLAG^TRUE~";

                    // IFS gönderimi + TXT dosya yazımı
                    await GonderIFSveDosyaYaz(sev.ORDER_NO, perIndex, queryText);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private static bool SiloBelongsToLine(IFSPLAN plan, SiloAdlari silo, int lineNo)
        {
            if (!silo.NAMEITM.HasValue)
                return false;

            var itemProp = typeof(IFSPLAN).GetProperty(silo.NAMEITM.Value.ToString());
            if (itemProp == null)
                return false;

            var valStr = itemProp.GetValue(plan) as string;
            return int.TryParse(valStr, out var val) && val == lineNo;
        }

        private static double ConvertToDouble(object? val)
        {
            if (val == null) return 0;
            if (val is double d) return d;
            if (val is float f) return f;
            if (val is decimal dec) return (double)dec;

            double.TryParse(val.ToString()?.Replace('.', ','), out var result);
            return result;
        }

        private async Task<bool> GonderIFSveDosyaYaz(string orderNo, int perIndex, string queryText)
        {
            try
            {
                _logger.LogInfo($"{orderNo + "-" + (perIndex + 1).ToString()} referanslı IFS üretim gönderiliyor.");

                string filePath = Application.StartupPath + "\\IFSDATASEND";
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                var fileName = $"{DateTime.Today:yyyyMMdd} - {orderNo}{(perIndex + 1)}.txt";
                using (var sw = new StreamWriter(Path.Combine(filePath, fileName)))
                {
                    sw.WriteLine(queryText);
                }

                var success = await restApiService.ReportWorkOrderAsync(
                    orderNo + "-" + (perIndex + 1).ToString(),
                    "URETIM",
                    queryText
                );

                if (success)
                    _logger.LogInfo($"{orderNo + "-" + (perIndex + 1).ToString()} referanslı IFS üretim gönderim işlemi tamamlandı.");
                else
                    _logger.LogInfo($"{orderNo + "-" + (perIndex + 1).ToString()} referanslı IFS üretim gönderim işleminde hata alındı.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Periyot Detay gönderim hatası", ex);
                return false;
            }



        }

        private async Task<bool> GonderUretimDurum()
        {
            try
            {
                using var ctx = new TesisContext();
                // İlgili plan kaydı
                var plan = await ctx.IFSPLANs.FirstOrDefaultAsync(p => p.DURUM == "1");
                if (plan == null)
                    return false;
                string queryText =
                    $"ORDER_NO^{plan.ORDER_NO}~" +
                    $"RELEASE_NO^{plan.RELEASE_NO}~" +
                    $"SEQUENCE_NO^{plan.SEQUENCE_NO}~" +
                    $"START_CODE^TRUE~";

                _logger.LogInfo($"{plan.ORDER_NO}-0 referanslı IFS üretim emir durumu gönderiliyor.");

                var success = await restApiService.ReportWorkOrderAsync(
                    plan.ORDER_NO + "-0",
                    "URETIM",
                    queryText
                );


                if (success)
                {
                    ctx.Database.ExecuteSqlCommand(
                        "UPDATE IFSPLAN SET DURUM = '2' WHERE ORDER_NO = @p0", plan.ORDER_NO);
                    _logger.LogInfo($"{plan.ORDER_NO + "-0"} referanslı IFS üretim emri durum gönderim işlemi tamamlandı.");
                }
                else
                    _logger.LogInfo($"{plan.ORDER_NO + "-0"} referanslı IFS üretim emri durum gönderim işleminde hata alındı.");
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError("GonderUretimDurum gönderim hatası", ex);
                return false;
            }

        }

        public async Task UretimDurumlariniGonder()
        {
            await GonderUretimDurum();
        }

    }

}
