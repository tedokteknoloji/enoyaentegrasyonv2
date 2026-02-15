using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraPrinting.Native;
using ENOYAEntegrasyonV2.DbContxt;
using ENOYAEntegrasyonV2.Models.Entities;
using ENOYAEntegrasyonV2.Repositories.Interfaces;
using ENOYAEntegrasyonV2.Services.Interfaces;
using Newtonsoft.Json;

namespace ENOYAEntegrasyonV2.Business
{
    /// <summary>
    /// IFS entegrasyon servisi
    /// İş emirlerini çeker, raporları gönderir
    /// </summary>
    public class IntegrationService
    {
        private readonly IRestApiService _apiService;
        private readonly ISevkiyatRepository _sevkiyatRepository;
        //private readonly IIrsaliyeRepository _irsaliyeRepository;
        //private readonly IMALZEMERepository _malzemeRepository;
        private readonly ILoggerService _logger;

        public IntegrationService(
            IRestApiService apiService,
            ISevkiyatRepository sevkiyatRepository,
            //IIrsaliyeRepository irsaliyeRepository,
            //IMALZEMERepository malzemeRepository,
            ILoggerService logger)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _sevkiyatRepository = sevkiyatRepository ?? throw new ArgumentNullException(nameof(sevkiyatRepository));
            //_irsaliyeRepository = irsaliyeRepository ?? throw new ArgumentNullException(nameof(irsaliyeRepository));
            //_malzemeRepository = malzemeRepository ?? throw new ArgumentNullException(nameof(malzemeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        void AddOrUpdateCimentoEsleme(List<GonderimEsleme> gonderimEsleme, int siloNo, IFSPLANLine satir, double amount, int ftype, string partNo)
        {
            string nameIst = $"CM{siloNo}_IST";
            string nameItem = $"CM{siloNo}_ITEMNO";
            string itmPartNo = $"CM{siloNo}_PARTNO";

            var mevcut = gonderimEsleme.FirstOrDefault(x => x.FTYPE == ftype && x.NAMEIST.Equals(nameIst, StringComparison.OrdinalIgnoreCase));

            if (mevcut == null || mevcut.ITEMNO != (short)satir.LineItemNo)
            {
                if (mevcut == null)
                {
                    gonderimEsleme.Add(new GonderimEsleme
                    {
                        REFID = (short)(gonderimEsleme.Count + 1),
                        NAMEIST = nameIst,
                        AMOUNT = Math.Round(amount, 0),
                        NAMEITEM = nameItem,
                        ITEMNO = (short)satir.LineItemNo,
                        FTYPE = (short)ftype,
                        ITMPARTNO = itmPartNo,
                        PARTNO = partNo
                    });
                }
                else
                {
                    // İstersen burada yeni kayıt açmak yerine mevcutu güncelleyebilirsin.
                    // Senin mevcut mantığın "ITEMNO farklıysa yeni ekle" şeklinde.
                    gonderimEsleme.Add(new GonderimEsleme
                    {
                        REFID = (short)(gonderimEsleme.Count + 1),
                        NAMEIST = nameIst,
                        AMOUNT = Math.Round(amount, 0),
                        NAMEITEM = nameItem,
                        ITEMNO = (short)satir.LineItemNo,
                        FTYPE = (short)ftype,
                        ITMPARTNO = itmPartNo,
                        PARTNO = partNo
                    });
                }
            }
        }


        /// <summary>
        /// İş emirlerini çek ve IFSPLAN tablosuna kaydet
        /// </summary>
        public async Task<bool> SyncShopOrdersAsync(string contract = null, string routingAlternative = "*")
        {
            try
            {
                _logger.LogInfo("İş emri senkronizasyonu başlatılıyor...");

                var orders = await _apiService.GetShopOrderListAsync(contract, "", routingAlternative);

                if (orders == null || orders.Count == 0)
                {
                    _logger.LogWarning("İş emri bulunamadı");
                    return false;
                }

                _logger.LogInfo($"{orders.Count} adet iş emri bulundu");

                // TODO: IFSPLAN tablosuna kaydetme işlemi
                using var ctx = new TesisContext();
                List<IFSPLANLine> siparisler = new List<IFSPLANLine>();
                foreach (IFSPLANLine item in orders)
                {
                    siparisler.Add(item);
                }

                var configIcerik = await ctx.CONFIGs.FirstAsync();
                if (configIcerik == null)
                {
                    MessageBox.Show("Config tanımları yapılmalı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    _logger.LogInfo("Config tanımları yapılmalı...");
                    throw new Exception("Config tanımları yapılmalı...");
                }
                bool farkliCimento = false;
                int hedefCimentoSiloNo;

                var siloIcerik = await ctx.SILO_ADs.SingleAsync(x => x.KOD.Equals("1"));
                if (siloIcerik == null)
                {
                    MessageBox.Show("Silo tanımları yapılmalı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    _logger.LogInfo("Silo tanımları yapılmalı...");
                    throw new Exception("Silo tanımları yapılmalı...");
                }

                var validation = await BN.ValidatePlanLinesAsync(ctx, siparisler);
                if (!validation.IsValid)
                {
                    _logger.LogError(validation.Message);
                    MessageBox.Show("SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
                        validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // Örnek: kullanıcıya bilgi ver, işleme devam etme
                    // return / throw / dialog vs.
                    throw new InvalidOperationException(
                        "SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
                        validation.Message);
                }

                var validationSilo = await BN.ValidateSiloadAsync(ctx);
                if (!validationSilo.IsValid)
                {
                    _logger.LogError(validation.Message);
                    MessageBox.Show("SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
                        validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // Örnek: kullanıcıya bilgi ver, işleme devam etme
                    // return / throw / dialog vs.
                    throw new InvalidOperationException(
                        "SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
                        validation.Message);
                }

                ////if (siloIcerik != null)
                ////{
                ////    // 1) Alanlardan gelen kodları topla (null/boşları at)
                ////    var kodlar = new[] { siloIcerik.CIMENTO1, siloIcerik.CIMENTO2, siloIcerik.CIMENTO3, siloIcerik.CIMENTO4 }
                ////        .Where(k => !string.IsNullOrWhiteSpace(k))
                ////        .Select(k => k.Trim())
                ////        .Distinct()
                ////        .ToList();

                ////    if (kodlar.Count > 0)
                ////    {
                ////        // 2) Bu kodlardan MALZEME'de var olan ve "04" ile başlayanları çek
                ////        var cimentoKodlari = await ctx.MALZEMEs
                ////            .Where(m => kodlar.Contains(m.PART_NO) && m.PART_NO.StartsWith("04"))
                ////            .Select(m => m.PART_NO)
                ////            .Distinct()
                ////            .ToListAsync();

                ////        // 3) En az 2 farklı çimento varsa true
                ////        farkliCimento = cimentoKodlari.Count >= 2;
                ////    }
                ////}
                //// siloIcerik yüklendikten sonra:
                //var cimentoToSiloNo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                //// CIMENTO alanları boş değilse map'e koy
                //if (!string.IsNullOrWhiteSpace(siloIcerik.CIMENTO1)) cimentoToSiloNo[siloIcerik.CIMENTO1.Trim()] = 1;
                //if (!string.IsNullOrWhiteSpace(siloIcerik.CIMENTO2)) cimentoToSiloNo[siloIcerik.CIMENTO2.Trim()] = 2;
                //if (!string.IsNullOrWhiteSpace(siloIcerik.CIMENTO3)) cimentoToSiloNo[siloIcerik.CIMENTO3.Trim()] = 3;
                //if (!string.IsNullOrWhiteSpace(siloIcerik.CIMENTO4)) cimentoToSiloNo[siloIcerik.CIMENTO4.Trim()] = 4;

                //// farklı çimento var mı?
                //farkliCimento = cimentoToSiloNo.Keys.Distinct(StringComparer.OrdinalIgnoreCase).Count() >= 2;

                var cimentoToSiloNo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                void AddCimentoIf04(string? partNo, int siloNo)
                {
                    var k = (partNo ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(k)) return;
                    if (!k.StartsWith("04", StringComparison.OrdinalIgnoreCase)) return; // <-- kritik koşul

                    cimentoToSiloNo[k] = siloNo; // aynı çimento 2 alanda tanımlıysa overwrite eder
                }

                AddCimentoIf04(siloIcerik.CIMENTO1, 1);
                AddCimentoIf04(siloIcerik.CIMENTO2, 2);
                AddCimentoIf04(siloIcerik.CIMENTO3, 3);
                AddCimentoIf04(siloIcerik.CIMENTO4, 4);

                // Dictionary zaten unique key tutar; Distinct'e gerek yok
                farkliCimento = cimentoToSiloNo.Count >= 2;

                var incomingOrderNos = orders
                    .Select(x => x.OrderNo?.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var localActivePlans = await ctx.IFSPLANs
                    .Where(p => p.DURUM != "4")
                    .Select(p => new { p.KOD, p.ORDER_NO, p.DURUM })  
                    .ToListAsync();

                var toCloseIds = localActivePlans
                    .Where(p => !incomingOrderNos.Contains((p.ORDER_NO ?? "").Trim()))
                    .Select(p => p.KOD)
                    .ToList();

                if (toCloseIds.Count > 0)
                {
                    var toCloseEntities = await ctx.IFSPLANs
                        .Where(p => toCloseIds.Contains(p.KOD))
                        .ToListAsync();

                    foreach (var p in toCloseEntities)
                        p.DURUM = "4";

                    await ctx.SaveChangesAsync();
                }

                var sipariss = siparisler.Select(x => x.OrderNo)
                            .Distinct()
                            .OrderBy(x => x)
                            .ToList();
                foreach (string item in sipariss)
                {
                    IFSPLAN aktifPlan = await ctx.IFSPLANs.Where(x => x.ORDER_NO.ToUpper().Equals(item.ToUpper())).FirstOrDefaultAsync();
                    if (aktifPlan != null) continue;

                    List<IFSPLANLine> tekilSiparisIcerik = siparisler.Where(x => x.OrderNo.Equals(item)).ToList();
                    List<GonderimEsleme> gonderimEsleme = new List<GonderimEsleme>();
                    int ComponentCount = 0;
                    foreach (IFSPLANLine satir in tekilSiparisIcerik)
                    {
                        if (satir.OrderCode.ToUpper().Equals("M"))
                        {
                            if (farkliCimento)
                            {
                                // satırdaki çimento hangi CIMENTO alanına denk geliyor?
                                if (!cimentoToSiloNo.TryGetValue(satir.ComponentPartNo?.Trim() ?? "", out hedefCimentoSiloNo))
                                {
                                    // SILO_AD içinde bu çimento tanımlı değilse (normalde validation yakalamalı)
                                    // Güvenli fallback: config'teki siloya yaz veya hata ver
                                    hedefCimentoSiloNo = Convert.ToInt32(configIcerik.CIMENTOSILO);
                                }
                            }
                            else
                            {
                                hedefCimentoSiloNo = Convert.ToInt32(configIcerik.CIMENTOSILO);
                            }
                            MALZEME aktifMalzeme = await ctx.MALZEMEs.Where(x => x.PART_NO.Equals(satir.ComponentPartNo)).FirstOrDefaultAsync();
                            if (aktifMalzeme == null) break;
                            List<SiloAdlari> uretimMalzemeleri = AppGlobals.siloList.Where(x => x.IFSPARTNO.ToUpper().Equals(satir.ComponentPartNo)).ToList();
                            if (uretimMalzemeleri == null || uretimMalzemeleri.Count == 0) break;
                            foreach (SiloAdlari bunker in uretimMalzemeleri)
                            {
                                decimal Amount = 0;
                                Amount = Math.Round(satir.QtyPerAssembly.ToDecimal(), 0);
                                switch (bunker.CONTYPE)
                                {
                                    case enumBunkerIcerikTipi.Agrega:
                                        if (bunker.ID == 1)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG1BLMYZDE) / 100));
                                        if (bunker.ID == 2)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG2BLMYZDE) / 100));
                                        if (bunker.ID == 3)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG3BLMYZDE) / 100));
                                        if (bunker.ID == 4)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG4BLMYZDE) / 100));
                                        if (bunker.ID == 5)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG5BLMYZDE) / 100));
                                        if (bunker.ID == 6)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG6BLMYZDE) / 100));
                                        _logger.LogInfo(item + "|" + bunker.DEFINITION_ + "|" + satir.ComponentPartNo + "|" + Amount.ToString() + "|" + bunker.NAMEIST.ToString() + " \r\n");
                                        gonderimEsleme.Add(new GonderimEsleme()
                                        {
                                            REFID = (Int16)(gonderimEsleme.Count + 1),
                                            NAMEIST = bunker.NAMEIST.ToString(),
                                            AMOUNT = Math.Round(Amount.ToDbl(), 0),
                                            NAMEITEM = bunker.NAMEITM.ToString(),
                                            ITEMNO = (Int16)satir.LineItemNo,
                                            FTYPE = (Int16)bunker.CONTYPE,
                                            ITMPARTNO = bunker.ITMPARTNO.ToString(),
                                            PARTNO = satir.ComponentPartNo
                                        });
                                        break;
                                    case enumBunkerIcerikTipi.Çimento_Kül:
                                        bool TmpCC = false;
                                        if (aktifMalzeme.PART_NO.StartsWith("04"))
                                        {
                                            if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("FALSE") && aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("TRUE"))
                                            {
                                                // hedefCimentoSiloNo'yu yukarıdaki kuralla hesapla
                                                AddOrUpdateCimentoEsleme(gonderimEsleme, hedefCimentoSiloNo, satir, Amount.ToDbl(), (int)bunker.CONTYPE, satir.ComponentPartNo);
                                            }
                                        }
                                        else if (aktifMalzeme.PART_NO.StartsWith("06"))
                                        {
                                            int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
                                            if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
                                                aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
                                                KulSiloNo > 0)
                                            {
                                                List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
                                                                                    x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
                                                if (cimentoEsleme.Count == 0)
                                                {
                                                    GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + KulSiloNo.ToString() + "_IST")).FirstOrDefault();
                                                    if (varMee == null)
                                                        gonderimEsleme.Add(new GonderimEsleme()
                                                        {
                                                            REFID = (Int16)(gonderimEsleme.Count + 1),
                                                            NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
                                                            AMOUNT = Math.Round(Amount.ToDbl(), 0),
                                                            NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
                                                            ITEMNO = (Int16)satir.LineItemNo,
                                                            FTYPE = (Int16)bunker.CONTYPE,
                                                            ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
                                                            PARTNO = satir.ComponentPartNo
                                                        });
                                                }
                                                else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
                                                {
                                                    GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + KulSiloNo.ToString() + "_IST")).FirstOrDefault();
                                                    if (varMee == null)
                                                        gonderimEsleme.Add(new GonderimEsleme()
                                                        {
                                                            REFID = (Int16)(gonderimEsleme.Count + 1),
                                                            NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
                                                            AMOUNT = Math.Round(Amount.ToDbl(), 0),
                                                            NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
                                                            ITEMNO = (Int16)satir.LineItemNo,
                                                            FTYPE = (Int16)bunker.CONTYPE,
                                                            ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
                                                            PARTNO = satir.ComponentPartNo
                                                        });
                                                }
                                            }
                                        }
                                        break;
                                    case enumBunkerIcerikTipi.Su1:
                                        double RecycleAmount = 0, NeedAmount = 0;
                                        try
                                        {
                                            RecycleAmount = (Amount.ToDbl() * (Convert.ToDouble(satir.RecycleUsageFactor.ToDecimal()) / 100));
                                        }
                                        catch
                                        {
                                            RecycleAmount = 0;
                                        }

                                        NeedAmount = Amount.ToDbl() - RecycleAmount;
                                        List<GonderimEsleme> suEsleme = gonderimEsleme.Where(x => x.FTYPE == 3 &&
                                                                    x.NAMEIST.ToUpper().Equals("SU1_IST")).ToList();
                                        if (suEsleme.Count == 0)
                                        {
                                            gonderimEsleme.Add(new GonderimEsleme()
                                            {
                                                REFID = (Int16)(gonderimEsleme.Count + 1),
                                                NAMEIST = "SU1_IST",
                                                AMOUNT = Math.Round(NeedAmount.ToDbl(), 0),
                                                NAMEITEM = "SU1_ITEMNO",
                                                ITEMNO = (Int16)satir.LineItemNo,
                                                FTYPE = (Int16)bunker.CONTYPE,
                                                ITMPARTNO = "SU1_PARTNO",
                                                PARTNO = satir.ComponentPartNo
                                            });
                                            gonderimEsleme.Add(new GonderimEsleme()
                                            {
                                                REFID = (Int16)(gonderimEsleme.Count + 1),
                                                NAMEIST = "SU2_IST",
                                                AMOUNT = Math.Round(RecycleAmount.ToDbl(), 0),
                                                NAMEITEM = "SU2_ITEMNO",
                                                ITEMNO = (Int16)satir.LineItemNo,
                                                FTYPE = (Int16)bunker.CONTYPE,
                                                ITMPARTNO = "SU2_PARTNO",
                                                PARTNO = satir.ComponentPartNo
                                            });
                                        }
                                        break;
                                    case enumBunkerIcerikTipi.Su2:
                                        break;
                                    case enumBunkerIcerikTipi.Katkı:
                                        if (aktifMalzeme.PART_NO.StartsWith("06"))
                                        {
                                            int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
                                            if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
                                                aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
                                                KulSiloNo > 0)
                                            {
                                                List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
                                                                                    x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
                                                if (cimentoEsleme.Count == 0)
                                                {
                                                    gonderimEsleme.Add(new GonderimEsleme()
                                                    {
                                                        REFID = (Int16)(gonderimEsleme.Count + 1),
                                                        NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
                                                        AMOUNT = Math.Round(Amount.ToDbl(), 2),
                                                        NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
                                                        ITEMNO = (Int16)satir.LineItemNo,
                                                        FTYPE = (Int16)bunker.CONTYPE,
                                                        ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
                                                        PARTNO = satir.ComponentPartNo
                                                    });
                                                }
                                                else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
                                                {
                                                    gonderimEsleme.Add(new GonderimEsleme()
                                                    {
                                                        REFID = (Int16)(gonderimEsleme.Count + 1),
                                                        NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
                                                        AMOUNT = Math.Round(Amount.ToDbl(), 2),
                                                        NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
                                                        ITEMNO = (Int16)satir.LineItemNo,
                                                        FTYPE = (Int16)bunker.CONTYPE,
                                                        ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
                                                        PARTNO = satir.ComponentPartNo
                                                    });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            gonderimEsleme.Add(new GonderimEsleme()
                                            {
                                                REFID = (Int16)(gonderimEsleme.Count + 1),
                                                NAMEIST = bunker.NAMEIST.ToString(),
                                                AMOUNT = Math.Round(Amount.ToDbl(), 2),
                                                NAMEITEM = bunker.NAMEITM.ToString(),
                                                ITEMNO = (Int16)satir.LineItemNo,
                                                FTYPE = (Int16)bunker.CONTYPE,
                                                ITMPARTNO = bunker.ITMPARTNO.ToString(),
                                                PARTNO = satir.ComponentPartNo
                                            });
                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }
                            string ss = gonderimEsleme.Count.ToString();
                        }
                        if (satir.OrderCode.ToUpper().Equals("F"))
                        {

                        }

                    }
                    if (tekilSiparisIcerik != null && tekilSiparisIcerik.Count > 0 && gonderimEsleme.Count > 0)
                    {
                        string FieldNames = "", FieldValues = "", UpdateQuery = "";
                        FieldNames = "KOD,ORDER_NO,RELEASE_NO,SEQUENCE_NO,ORDER_CODE,RECETE_PART_NO, RECETE_PART_DESC,MIKTAR,MIXING_TIME,MIKSER_NO,CUSTOMER_ID, CUSTOMER_NAME, ADRESS_ID, ADRESS_DESC";
                        FieldValues = "(SELECT ISNULL(MAX(KOD),0)+1 FROM IFSPLAN)"
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderNo, 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].ReleaseNo, 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].SequenceNo, 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderCode, 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].PartNo, 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].PartDesc), 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].RevisedQtyDue.ToString().Replace(',', '.'), 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].MixingTime.ToString().Replace(',', '.'), 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].Mikser.ToString().Replace(',', '.'), 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerId), 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerName), 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressId), 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressDesc), 1)
                            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMAA.ToString().Replace(',', '.'), 2)
                            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMBB.ToString().Replace(',', '.'), 2)
                            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMCC.ToString().Replace(',', '.'), 2)
                            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMDD.ToString().Replace(',', '.'), 2)
                            ;

                        File.WriteAllText("OrnekKAyit.txt", JsonConvert.SerializeObject(gonderimEsleme));
                        foreach (var esleme in gonderimEsleme)
                        {
                            bool hasDouble = false;
                            int ValueInt = 0;
                            double ValueDouble = 0.00;

                            if (esleme.FTYPE == 5)
                            {
                                hasDouble = true;
                                ValueDouble = Convert.ToDouble(Math.Round(Convert.ToDouble(esleme.AMOUNT), 2));
                            }
                            else
                            {
                                hasDouble = false;
                                ValueInt = Convert.ToInt32(Math.Round(Convert.ToDouble(esleme.AMOUNT), 0));
                            }

                            FieldNames = FieldNames + "," + esleme.NAMEIST.ToString() + "," + esleme.NAMEITEM.ToString()
                                                    + "," + esleme.ITMPARTNO.ToString();
                            FieldValues = FieldValues + "," + BN.FieldReturnValues((hasDouble == false ? ValueInt.ToString() : ValueDouble.ToString()).Replace(',', '.'), 1)
                                                    + "," + BN.FieldReturnValues(esleme.ITEMNO.ToString().Replace(',', '.'), 1)
                                                    + "," + BN.FieldReturnValues(esleme.PARTNO.ToString().Replace(',', '.'), 1);
                        }
                        UpdateQuery = "INSERT INTO dbo.IFSPLAN (" + FieldNames + ") VALUES (" + FieldValues + ")";
                        File.WriteAllText("OrnekSorgu" + tekilSiparisIcerik[0].OrderNo + ".txt", UpdateQuery);
                        int affected = ctx.Database.ExecuteSqlCommand(UpdateQuery);
                        if (affected <= 0)
                        {
                            MessageBox.Show(tekilSiparisIcerik[0].OrderNo + " nolu İş Emri eklenemedi.");
                        }
                    }
                }

                _logger.LogInfo("İş emri senkronizasyonu tamamlandı");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("İş emri senkronizasyonu hatası", ex);
                return false;
            }

            //try
            //{
            //    _logger.LogInfo("İş emri senkronizasyonu başlatılıyor...");

            //    var orders = await _apiService.GetShopOrderListAsync(contract, "", routingAlternative);

            //    if (orders == null || orders.Count == 0)
            //    {
            //        _logger.LogWarning("İş emri bulunamadı");
            //        return false;
            //    }

            //    _logger.LogInfo($"{orders.Count} adet iş emri bulundu");

            //    // TODO: IFSPLAN tablosuna kaydetme işlemi
            //    using var ctx = new TesisContext();
            //    List<IFSPLANLine> siparisler = new List<IFSPLANLine>();
            //    foreach (IFSPLANLine item in orders)
            //    {
            //        siparisler.Add(item);
            //    }

            //    var configIcerik = await ctx.CONFIGs.FirstAsync();
            //    if (configIcerik == null)
            //    {
            //        MessageBox.Show("Config tanımları yapılmalı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        _logger.LogInfo("Config tanımları yapılmalı...");
            //        throw new Exception("Config tanımları yapılmalı...");
            //    }
            //    bool farkliCimento = false;

            //    var siloIcerik = await ctx.SILO_ADs.SingleAsync(x => x.KOD.Equals("1"));
            //    if (siloIcerik == null)
            //    {
            //        MessageBox.Show("Silo tanımları yapılmalı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        _logger.LogInfo("Silo tanımları yapılmalı...");
            //        throw new Exception("Silo tanımları yapılmalı...");
            //    }

            //    var validation = await BN.ValidatePlanLinesAsync(ctx, siparisler);
            //    if (!validation.IsValid)
            //    {
            //        _logger.LogError(validation.Message);
            //        MessageBox.Show("SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //            validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        // Örnek: kullanıcıya bilgi ver, işleme devam etme
            //        // return / throw / dialog vs.
            //        throw new InvalidOperationException(
            //            "SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //            validation.Message);
            //    }

            //    var validationSilo = await BN.ValidateSiloadAsync(ctx);
            //    if (!validationSilo.IsValid)
            //    {
            //        _logger.LogError(validation.Message);
            //        MessageBox.Show("SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //            validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        // Örnek: kullanıcıya bilgi ver, işleme devam etme
            //        // return / throw / dialog vs.
            //        throw new InvalidOperationException(
            //            "SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //            validation.Message);
            //    }

            //    if (siloIcerik != null)
            //    {
            //        // 1) Alanlardan gelen kodları topla (null/boşları at)
            //        var kodlar = new[] { siloIcerik.CIMENTO1, siloIcerik.CIMENTO2, siloIcerik.CIMENTO3, siloIcerik.CIMENTO4 }
            //            .Where(k => !string.IsNullOrWhiteSpace(k))
            //            .Select(k => k.Trim())
            //            .Distinct()
            //            .ToList();

            //        if (kodlar.Count > 0)
            //        {
            //            // 2) Bu kodlardan MALZEME'de var olan ve "04" ile başlayanları çek
            //            var cimentoKodlari = await ctx.MALZEMEs
            //                .Where(m => kodlar.Contains(m.PART_NO) && m.PART_NO.StartsWith("04"))
            //                .Select(m => m.PART_NO)
            //                .Distinct()
            //                .ToListAsync();

            //            // 3) En az 2 farklı çimento varsa true
            //            farkliCimento = cimentoKodlari.Count >= 2;
            //        }
            //    }


            //    var sipariss = siparisler.Select(x => x.OrderNo)
            //                .Distinct()
            //                .OrderBy(x => x)
            //                .ToList();
            //    foreach (string item in sipariss)
            //    {
            //        IFSPLAN aktifPlan = await ctx.IFSPLANs.Where(x => x.ORDER_NO.ToUpper().Equals(item.ToUpper())).FirstOrDefaultAsync();
            //        if (aktifPlan != null) continue;

            //        List<IFSPLANLine> tekilSiparisIcerik = siparisler.Where(x => x.OrderNo.Equals(item)).ToList();
            //        List<GonderimEsleme> gonderimEsleme = new List<GonderimEsleme>();
            //        int ComponentCount = 0;
            //        foreach (IFSPLANLine satir in tekilSiparisIcerik)
            //        {
            //            if (satir.OrderCode.ToUpper().Equals("M"))
            //            {
            //                MALZEME aktifMalzeme = await ctx.MALZEMEs.Where(x => x.PART_NO.Equals(satir.ComponentPartNo)).FirstOrDefaultAsync();
            //                if (aktifMalzeme == null) break;
            //                List<SiloAdlari> uretimMalzemeleri = AppGlobals.siloList.Where(x => x.IFSPARTNO.ToUpper().Equals(satir.ComponentPartNo)).ToList();
            //                if (uretimMalzemeleri == null || uretimMalzemeleri.Count == 0) break;
            //                foreach (SiloAdlari bunker in uretimMalzemeleri)
            //                {
            //                    decimal Amount = 0;
            //                    Amount = Math.Round(satir.QtyPerAssembly.ToDecimal(), 0);
            //                    switch (bunker.CONTYPE)
            //                    {
            //                        case enumBunkerIcerikTipi.Agrega:
            //                            if (bunker.ID == 1)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG1BLMYZDE) / 100));
            //                            if (bunker.ID == 2)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG2BLMYZDE) / 100));
            //                            if (bunker.ID == 3)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG3BLMYZDE) / 100));
            //                            if (bunker.ID == 4)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG4BLMYZDE) / 100));
            //                            if (bunker.ID == 5)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG5BLMYZDE) / 100));
            //                            if (bunker.ID == 6)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG6BLMYZDE) / 100));
            //                            _logger.LogInfo(item + "|" + bunker.DEFINITION_ + "|" + satir.ComponentPartNo + "|" + Amount.ToString() + "|" + bunker.NAMEIST.ToString() + " \r\n");
            //                            gonderimEsleme.Add(new GonderimEsleme()
            //                            {
            //                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                NAMEIST = bunker.NAMEIST.ToString(),
            //                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                NAMEITEM = bunker.NAMEITM.ToString(),
            //                                ITEMNO = (Int16)satir.LineItemNo,
            //                                FTYPE = (Int16)bunker.CONTYPE,
            //                                ITMPARTNO = bunker.ITMPARTNO.ToString(),
            //                                PARTNO = satir.ComponentPartNo
            //                            });
            //                            break;
            //                        case enumBunkerIcerikTipi.Çimento_Kül:
            //                            bool TmpCC = false;
            //                            if (aktifMalzeme.PART_NO.StartsWith("04"))
            //                            {
            //                                int CimentoSiloNo = Convert.ToInt32(configIcerik.CIMENTOSILO);
            //                                if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("FALSE") &&
            //                                    aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("TRUE"))
            //                                {
            //                                    if (CimentoSiloNo > 0)
            //                                    {
            //                                        List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                            x.NAMEIST.ToUpper().Equals("CM" + CimentoSiloNo.ToString())).ToList();
            //                                        if (cimentoEsleme.Count == 0)
            //                                        {
            //                                            GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + CimentoSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                            if (varMee == null)
            //                                                gonderimEsleme.Add(new GonderimEsleme()
            //                                                {
            //                                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                    NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                                    AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                    NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                                    ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                                    PARTNO = satir.ComponentPartNo
            //                                                });
            //                                        }
            //                                        else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                        {
            //                                            GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + CimentoSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                            if (varMee == null)
            //                                                gonderimEsleme.Add(new GonderimEsleme()
            //                                                {
            //                                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                    NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                                    AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                    NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                                    ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                                    PARTNO = satir.ComponentPartNo
            //                                                });
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                            x.NAMEIST.ToUpper().Equals("CM" + CimentoSiloNo.ToString())).ToList();
            //                                        if (cimentoEsleme.Count == 0)
            //                                        {
            //                                            GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + CimentoSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                            if (varMee == null)
            //                                                gonderimEsleme.Add(new GonderimEsleme()
            //                                                {
            //                                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                    NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                                    AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                    NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                                    ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                                    PARTNO = satir.ComponentPartNo
            //                                                });
            //                                        }
            //                                        else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                        {
            //                                            GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + CimentoSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                            if (varMee == null)
            //                                                gonderimEsleme.Add(new GonderimEsleme()
            //                                                {
            //                                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                    NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                                    AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                    NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                                    ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                                    PARTNO = satir.ComponentPartNo
            //                                                });
            //                                        }
            //                                    }
            //                                }
            //                                /*    int CimentoSiloNo = Convert.ToInt32(configIcerik.CIMENTOSILO);
            //                                if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("FALSE") &&
            //                                    aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("TRUE") &&
            //                                    CimentoSiloNo > 0)
            //                                {
            //                                    List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                        x.NAMEIST.ToUpper().Equals("CM" + CimentoSiloNo.ToString())).ToList();
            //                                    if (cimentoEsleme.Count == 0)
            //                                    {
            //                                        GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + CimentoSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                        if (varMee == null)
            //                                            gonderimEsleme.Add(new GonderimEsleme()
            //                                            {
            //                                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                                ITEMNO = (Int16)satir.LineItemNo,
            //                                                FTYPE = (Int16)bunker.CONTYPE,
            //                                                ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                                PARTNO = satir.ComponentPartNo
            //                                            });
            //                                    }
            //                                    else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                    {
            //                                        GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + CimentoSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                        if (varMee == null)
            //                                            gonderimEsleme.Add(new GonderimEsleme()
            //                                            {
            //                                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                                ITEMNO = (Int16)satir.LineItemNo,
            //                                                FTYPE = (Int16)bunker.CONTYPE,
            //                                                ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                                PARTNO = satir.ComponentPartNo
            //                                            });
            //                                    }
            //                                }
            //                            */
            //                            }
            //                            else if (aktifMalzeme.PART_NO.StartsWith("06"))
            //                            {
            //                                int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
            //                                if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
            //                                    aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
            //                                    KulSiloNo > 0)
            //                                {
            //                                    List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                        x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
            //                                    if (cimentoEsleme.Count == 0)
            //                                    {
            //                                        GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + KulSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                        if (varMee == null)
            //                                            gonderimEsleme.Add(new GonderimEsleme()
            //                                            {
            //                                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                                ITEMNO = (Int16)satir.LineItemNo,
            //                                                FTYPE = (Int16)bunker.CONTYPE,
            //                                                ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                                PARTNO = satir.ComponentPartNo
            //                                            });
            //                                    }
            //                                    else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                    {
            //                                        GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + KulSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                        if (varMee == null)
            //                                            gonderimEsleme.Add(new GonderimEsleme()
            //                                            {
            //                                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                                ITEMNO = (Int16)satir.LineItemNo,
            //                                                FTYPE = (Int16)bunker.CONTYPE,
            //                                                ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                                PARTNO = satir.ComponentPartNo
            //                                            });
            //                                    }
            //                                }
            //                            }
            //                            break;
            //                        case enumBunkerIcerikTipi.Su1:
            //                            double RecycleAmount = 0, NeedAmount = 0;
            //                            try
            //                            {
            //                                RecycleAmount = (Amount.ToDbl() * (Convert.ToDouble(satir.RecycleUsageFactor.ToDecimal()) / 100));
            //                            }
            //                            catch
            //                            {
            //                                RecycleAmount = 0;
            //                            }

            //                            NeedAmount = Amount.ToDbl() - RecycleAmount;
            //                            List<GonderimEsleme> suEsleme = gonderimEsleme.Where(x => x.FTYPE == 3 &&
            //                                                        x.NAMEIST.ToUpper().Equals("SU1_IST")).ToList();
            //                            if (suEsleme.Count == 0)
            //                            {
            //                                gonderimEsleme.Add(new GonderimEsleme()
            //                                {
            //                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                    NAMEIST = "SU1_IST",
            //                                    AMOUNT = Math.Round(NeedAmount.ToDbl(), 0),
            //                                    NAMEITEM = "SU1_ITEMNO",
            //                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                    ITMPARTNO = "SU1_PARTNO",
            //                                    PARTNO = satir.ComponentPartNo
            //                                });
            //                                gonderimEsleme.Add(new GonderimEsleme()
            //                                {
            //                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                    NAMEIST = "SU2_IST",
            //                                    AMOUNT = Math.Round(RecycleAmount.ToDbl(), 0),
            //                                    NAMEITEM = "SU2_ITEMNO",
            //                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                    ITMPARTNO = "SU2_PARTNO",
            //                                    PARTNO = satir.ComponentPartNo
            //                                });
            //                            }
            //                            break;
            //                        case enumBunkerIcerikTipi.Su2:
            //                            break;
            //                        case enumBunkerIcerikTipi.Katkı:
            //                            if (aktifMalzeme.PART_NO.StartsWith("06"))
            //                            {
            //                                int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
            //                                if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
            //                                    aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
            //                                    KulSiloNo > 0)
            //                                {
            //                                    List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                        x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
            //                                    if (cimentoEsleme.Count == 0)
            //                                    {
            //                                        gonderimEsleme.Add(new GonderimEsleme()
            //                                        {
            //                                            REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                            NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                            AMOUNT = Math.Round(Amount.ToDbl(), 2),
            //                                            NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                            ITEMNO = (Int16)satir.LineItemNo,
            //                                            FTYPE = (Int16)bunker.CONTYPE,
            //                                            ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                            PARTNO = satir.ComponentPartNo
            //                                        });
            //                                    }
            //                                    else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                    {
            //                                        gonderimEsleme.Add(new GonderimEsleme()
            //                                        {
            //                                            REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                            NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                            AMOUNT = Math.Round(Amount.ToDbl(), 2),
            //                                            NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                            ITEMNO = (Int16)satir.LineItemNo,
            //                                            FTYPE = (Int16)bunker.CONTYPE,
            //                                            ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                            PARTNO = satir.ComponentPartNo
            //                                        });
            //                                    }
            //                                }
            //                            }
            //                            else
            //                            {
            //                                gonderimEsleme.Add(new GonderimEsleme()
            //                                {
            //                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                    NAMEIST = bunker.NAMEIST.ToString(),
            //                                    AMOUNT = Math.Round(Amount.ToDbl(), 2),
            //                                    NAMEITEM = bunker.NAMEITM.ToString(),
            //                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                    ITMPARTNO = bunker.ITMPARTNO.ToString(),
            //                                    PARTNO = satir.ComponentPartNo
            //                                });
            //                            }
            //                            break;
            //                        default:
            //                            break;
            //                    }

            //                }
            //                string ss = gonderimEsleme.Count.ToString();
            //            }
            //            if (satir.OrderCode.ToUpper().Equals("F"))
            //            {

            //            }

            //        }
            //        if (tekilSiparisIcerik != null && tekilSiparisIcerik.Count > 0 && gonderimEsleme.Count > 0)
            //        {
            //            string FieldNames = "", FieldValues = "", UpdateQuery = "";
            //            FieldNames = "KOD,ORDER_NO,RELEASE_NO,SEQUENCE_NO,ORDER_CODE,RECETE_PART_NO, RECETE_PART_DESC,MIKTAR,MIXING_TIME,MIKSER_NO,CUSTOMER_ID, CUSTOMER_NAME, ADRESS_ID, ADRESS_DESC";
            //            FieldValues = "(SELECT ISNULL(MAX(KOD),0)+1 FROM IFSPLAN)"
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderNo, 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].ReleaseNo, 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].SequenceNo, 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderCode, 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].PartNo, 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].PartDesc), 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].RevisedQtyDue.ToString().Replace(',', '.'), 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].MixingTime.ToString().Replace(',', '.'), 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].Mikser.ToString().Replace(',', '.'), 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerId), 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerName), 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressId), 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressDesc), 1)
            //                //+ "," + EOFGlobalMethods.FieldReturnValues(KUMAA.ToString().Replace(',', '.'), 2)
            //                //+ "," + EOFGlobalMethods.FieldReturnValues(KUMBB.ToString().Replace(',', '.'), 2)
            //                //+ "," + EOFGlobalMethods.FieldReturnValues(KUMCC.ToString().Replace(',', '.'), 2)
            //                //+ "," + EOFGlobalMethods.FieldReturnValues(KUMDD.ToString().Replace(',', '.'), 2)
            //                ;

            //            File.WriteAllText("OrnekKAyit.txt", JsonConvert.SerializeObject(gonderimEsleme));
            //            foreach (var esleme in gonderimEsleme)
            //            {
            //                bool hasDouble = false;
            //                int ValueInt = 0;
            //                double ValueDouble = 0.00;

            //                if (esleme.FTYPE == 5)
            //                {
            //                    hasDouble = true;
            //                    ValueDouble = Convert.ToDouble(Math.Round(Convert.ToDouble(esleme.AMOUNT), 2));
            //                }
            //                else
            //                {
            //                    hasDouble = false;
            //                    ValueInt = Convert.ToInt32(Math.Round(Convert.ToDouble(esleme.AMOUNT), 0));
            //                }

            //                FieldNames = FieldNames + "," + esleme.NAMEIST.ToString() + "," + esleme.NAMEITEM.ToString()
            //                                        + "," + esleme.ITMPARTNO.ToString();
            //                FieldValues = FieldValues + "," + BN.FieldReturnValues((hasDouble == false ? ValueInt.ToString() : ValueDouble.ToString()).Replace(',', '.'), 1)
            //                                        + "," + BN.FieldReturnValues(esleme.ITEMNO.ToString().Replace(',', '.'), 1)
            //                                        + "," + BN.FieldReturnValues(esleme.PARTNO.ToString().Replace(',', '.'), 1);
            //            }
            //            UpdateQuery = "INSERT INTO dbo.IFSPLAN (" + FieldNames + ") VALUES (" + FieldValues + ")";
            //            File.WriteAllText("OrnekSorgu" + tekilSiparisIcerik[0].OrderNo + ".txt", UpdateQuery);
            //            int affected = ctx.Database.ExecuteSqlCommand(UpdateQuery);
            //            if (affected <= 0)
            //            {
            //                MessageBox.Show(tekilSiparisIcerik[0].OrderNo + " nolu İş Emri eklenemedi.");
            //            }
            //        }
            //    }



            //    _logger.LogInfo("İş emri senkronizasyonu tamamlandı");
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("İş emri senkronizasyonu hatası", ex);
            //    return false;
            //}

        }


        /// <summary>
        /// İş emirlerini çek ve IFSPLAN tablosuna kaydet
        /// </summary>
        public async Task<List<IFSPLANLine>> GetShopOrdersAsync(string contract = null, string routingAlternative = "*", bool yerelOkuma = false)
        {

            try
            {
                List<IFSPLANLine> orders = new List<IFSPLANLine>();

                _logger.LogInfo("İş emri senkronizasyonu başlatılıyor...");

                if (yerelOkuma)
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "siparis.json");
                    if (!File.Exists(filePath))
                    {
                        _logger.LogWarning($"siparis.json dosyası bulunamadı: {filePath}");
                        return new List<IFSPLANLine>();
                    }
                    var jsonContent = File.ReadAllText(filePath);
                    var innerJson = JsonConvert.DeserializeObject<string>(jsonContent);
                    var odataResponse = JsonConvert.DeserializeObject<ODataResponse<IFSPLANLine>>(innerJson); //
                    orders = (List<IFSPLANLine>)odataResponse?.Value ?? new List<IFSPLANLine>();
                }
                else
                    orders = await _apiService.GetShopOrderListAsync(contract, "", routingAlternative);

                if (orders == null || orders.Count == 0)
                {
                    _logger.LogWarning("İş emri bulunamadı");
                    return null;
                }
                else return orders;
                //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "siparis.json");


                //if (!File.Exists(filePath))
                //{
                //    _logger.LogWarning($"siparis.json dosyası bulunamadı: {filePath}");
                //    return new List<IFSPLANLine>();
                //}

                //var jsonContent = File.ReadAllText(filePath);
                //var innerJson = JsonConvert.DeserializeObject<string>(jsonContent);
                //var odataResponse = JsonConvert.DeserializeObject<ODataResponse<IFSPLANLine>>(innerJson); //
                //var planLines = (List<IFSPLANLine>)odataResponse?.Value ?? new List<IFSPLANLine>();

                ////var planLines = JsonConvert.DeserializeObject<List<IFSPLANLine>>(jsonContent);
                //return planLines ?? new List<IFSPLANLine>();
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON deserialize hatası", ex);
                //throw;
            }
            return null;
            //try
            //{
            //    List<IFSPLANLine> orders = new List<IFSPLANLine>();

            //    _logger.LogInfo("İş emri senkronizasyonu başlatılıyor...");

            //    if (yerelOkuma)
            //    {
            //        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "siparis.json");
            //        if (!File.Exists(filePath))
            //        {
            //            _logger.LogWarning($"siparis.json dosyası bulunamadı: {filePath}");
            //            return new List<IFSPLANLine>();
            //        }
            //        var jsonContent = File.ReadAllText(filePath);
            //        var innerJson = JsonConvert.DeserializeObject<string>(jsonContent);
            //        var odataResponse = JsonConvert.DeserializeObject<ODataResponse<IFSPLANLine>>(innerJson); //
            //        orders = (List<IFSPLANLine>)odataResponse?.Value ?? new List<IFSPLANLine>();
            //    }
            //    else
            //        orders = await _apiService.GetShopOrderListAsync(contract, "", routingAlternative);

            //    if (orders == null || orders.Count == 0)
            //    {
            //        _logger.LogWarning("İş emri bulunamadı");
            //        return null;
            //    }

            //    _logger.LogInfo($"{orders.Count} adet iş emri bulundu");

            //    // TODO: IFSPLAN tablosuna kaydetme işlemi
            //    using var ctx = new TesisContext();
            //    //List<IFSPLANLine> siparisler = new List<IFSPLANLine>();
            //    //foreach (IFSPLANLine item in bindIFSPlanline)
            //    //{
            //    //    siparisler.Add(item);
            //    //}

            //    var siloIcerik = await ctx.SILO_ADs.SingleAsync(x => x.KOD.Equals("1"));
            //    var configIcerik = await ctx.CONFIGs.FirstAsync();

            //    if (siloIcerik == null)
            //    {
            //        MessageBox.Show("Silo tanımları yapılmalı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        _logger.LogInfo("Silo tanımları yapılmalı...");
            //        throw new Exception("Silo tanımları yapılmalı...");
            //    }



            //    var validation = await BN.ValidatePlanLinesAsync(ctx, orders);

            //    if (!validation.IsValid)
            //    {
            //        _logger.LogError(validation.Message);
            //        MessageBox.Show("SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //            validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        // Örnek: kullanıcıya bilgi ver, işleme devam etme
            //        // return / throw / dialog vs.
            //        throw new InvalidOperationException(
            //            "SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //            validation.Message);
            //    }

            //    var validationSilo = await BN.ValidateSiloadAsync(ctx);

            //    if (!validationSilo.IsValid)
            //    {
            //        _logger.LogError(validation.Message);
            //        MessageBox.Show("SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //            validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        // Örnek: kullanıcıya bilgi ver, işleme devam etme
            //        // return / throw / dialog vs.
            //        throw new InvalidOperationException(
            //            "SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //            validation.Message);
            //    }

            //    var sipariss = orders.Select(x => x.OrderNo)
            //                    .Distinct()
            //                    .OrderBy(x => x)
            //                    .ToList();
            //    foreach (string item in sipariss)
            //    {
            //        IFSPLAN aktifPlan = await ctx.IFSPLANs.Where(x => x.ORDER_NO.ToUpper().Equals(item.ToUpper())).FirstOrDefaultAsync();
            //        if (aktifPlan != null) continue;

            //        List<IFSPLANLine> tekilSiparisIcerik = orders.Where(x => x.OrderNo.Equals(item)).ToList();
            //        List<GonderimEsleme> gonderimEsleme = new List<GonderimEsleme>();
            //        int ComponentCount = 0;
            //        foreach (IFSPLANLine satir in tekilSiparisIcerik)
            //        {
            //            if (satir.OrderCode.ToUpper().Equals("M"))
            //            {
            //                MALZEME aktifMalzeme = await ctx.MALZEMEs.Where(x => x.PART_NO.Equals(satir.ComponentPartNo)).FirstOrDefaultAsync();
            //                if (aktifMalzeme == null) break;
            //                List<SiloAdlari> uretimMalzemeleri = AppGlobals.siloList.Where(x => !String.IsNullOrEmpty(x.IFSPARTNO)
            //                                    && x.IFSPARTNO.ToUpper().Equals(satir.ComponentPartNo)).ToList();
            //                if (uretimMalzemeleri == null || uretimMalzemeleri.Count == 0) break;
            //                foreach (SiloAdlari bunker in uretimMalzemeleri)
            //                {
            //                    decimal Amount = 0;

            //                    Amount = Math.Round(satir.QtyPerAssembly.ToDecimal(), 0);
            //                    switch (bunker.CONTYPE)
            //                    {
            //                        case enumBunkerIcerikTipi.Agrega:
            //                            if (bunker.ID == 1)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG1BLMYZDE) / 100));
            //                            if (bunker.ID == 2)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG2BLMYZDE) / 100));
            //                            if (bunker.ID == 3)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG3BLMYZDE) / 100));
            //                            if (bunker.ID == 4)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG4BLMYZDE) / 100));
            //                            if (bunker.ID == 5)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG5BLMYZDE) / 100));
            //                            if (bunker.ID == 6)
            //                                Amount = (Amount * (Convert.ToDecimal(configIcerik.AG6BLMYZDE) / 100));
            //                            _logger.LogInfo(item + "|" + bunker.DEFINITION_ + "|" + satir.ComponentPartNo + "|" + Amount.ToString() + "|" + bunker.NAMEIST.ToString() + " \r\n");
            //                            gonderimEsleme.Add(new GonderimEsleme()
            //                            {
            //                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                NAMEIST = bunker.NAMEIST.ToString(),
            //                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                NAMEITEM = bunker.NAMEITM.ToString(),
            //                                ITEMNO = (Int16)satir.LineItemNo,
            //                                FTYPE = (Int16)bunker.CONTYPE,
            //                                ITMPARTNO = bunker.ITMPARTNO.ToString(),
            //                                PARTNO = satir.ComponentPartNo
            //                            });
            //                            break;
            //                        case enumBunkerIcerikTipi.Çimento_Kül:
            //                            bool TmpCC = false;
            //                            if (aktifMalzeme.PART_NO.StartsWith("04"))
            //                            {
            //                                int CimentoSiloNo = Convert.ToInt32(configIcerik.CIMENTOSILO);
            //                                if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("FALSE") &&
            //                                    aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("TRUE") &&
            //                                    CimentoSiloNo > 0)
            //                                {
            //                                    List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                        x.NAMEIST.ToUpper().Equals("CM" + CimentoSiloNo.ToString())).ToList();
            //                                    if (cimentoEsleme.Count == 0)
            //                                    {
            //                                        GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + CimentoSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                        if (varMee == null)
            //                                            gonderimEsleme.Add(new GonderimEsleme()
            //                                            {
            //                                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                                ITEMNO = (Int16)satir.LineItemNo,
            //                                                FTYPE = (Int16)bunker.CONTYPE,
            //                                                ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                                PARTNO = satir.ComponentPartNo
            //                                            });
            //                                    }
            //                                    else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                    {
            //                                        GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + CimentoSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                        if (varMee == null)
            //                                            gonderimEsleme.Add(new GonderimEsleme()
            //                                            {
            //                                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                                ITEMNO = (Int16)satir.LineItemNo,
            //                                                FTYPE = (Int16)bunker.CONTYPE,
            //                                                ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                                PARTNO = satir.ComponentPartNo
            //                                            });
            //                                    }
            //                                }
            //                            }
            //                            else if (aktifMalzeme.PART_NO.StartsWith("06"))
            //                            {
            //                                int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
            //                                if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
            //                                    aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
            //                                    KulSiloNo > 0)
            //                                {
            //                                    List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                        x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
            //                                    if (cimentoEsleme.Count == 0)
            //                                    {
            //                                        GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + KulSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                        if (varMee == null)
            //                                            gonderimEsleme.Add(new GonderimEsleme()
            //                                            {
            //                                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                                ITEMNO = (Int16)satir.LineItemNo,
            //                                                FTYPE = (Int16)bunker.CONTYPE,
            //                                                ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                                PARTNO = satir.ComponentPartNo
            //                                            });
            //                                    }
            //                                    else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                    {
            //                                        GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + KulSiloNo.ToString() + "_IST")).FirstOrDefault();
            //                                        if (varMee == null)
            //                                            gonderimEsleme.Add(new GonderimEsleme()
            //                                            {
            //                                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                                NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                                NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                                ITEMNO = (Int16)satir.LineItemNo,
            //                                                FTYPE = (Int16)bunker.CONTYPE,
            //                                                ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                                PARTNO = satir.ComponentPartNo
            //                                            });
            //                                    }
            //                                }
            //                            }
            //                            break;
            //                        case enumBunkerIcerikTipi.Su1:
            //                            double RecycleAmount = 0, NeedAmount = 0;
            //                            try
            //                            {
            //                                RecycleAmount = (Amount.ToDbl() * (Convert.ToDouble(satir.RecycleUsageFactor.ToDecimal()) / 100));
            //                            }
            //                            catch
            //                            {
            //                                RecycleAmount = 0;
            //                            }

            //                            NeedAmount = Amount.ToDbl() - RecycleAmount;
            //                            List<GonderimEsleme> suEsleme = gonderimEsleme.Where(x => x.FTYPE == 3 &&
            //                                                        x.NAMEIST.ToUpper().Equals("SU1_IST")).ToList();
            //                            if (suEsleme.Count == 0)
            //                            {
            //                                gonderimEsleme.Add(new GonderimEsleme()
            //                                {
            //                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                    NAMEIST = "SU1_IST",
            //                                    AMOUNT = Math.Round(NeedAmount.ToDbl(), 0),
            //                                    NAMEITEM = "SU1_ITEMNO",
            //                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                    ITMPARTNO = "SU1_PARTNO",
            //                                    PARTNO = satir.ComponentPartNo
            //                                });
            //                                gonderimEsleme.Add(new GonderimEsleme()
            //                                {
            //                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                    NAMEIST = "SU2_IST",
            //                                    AMOUNT = Math.Round(RecycleAmount.ToDbl(), 0),
            //                                    NAMEITEM = "SU2_ITEMNO",
            //                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                    ITMPARTNO = "SU2_PARTNO",
            //                                    PARTNO = satir.ComponentPartNo
            //                                });
            //                            }
            //                            break;
            //                        case enumBunkerIcerikTipi.Su2:
            //                            break;
            //                        case enumBunkerIcerikTipi.Katkı:
            //                            Amount = Math.Round(satir.QtyPerAssembly.ToDecimal(), 2);
            //                            if (aktifMalzeme.PART_NO.StartsWith("06"))
            //                            {
            //                                int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
            //                                if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
            //                                    aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
            //                                    KulSiloNo > 0)
            //                                {
            //                                    List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                        x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
            //                                    if (cimentoEsleme.Count == 0)
            //                                    {
            //                                        gonderimEsleme.Add(new GonderimEsleme()
            //                                        {
            //                                            REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                            NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                            AMOUNT = Math.Round(Amount.ToDbl(), 2),
            //                                            NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                            ITEMNO = (Int16)satir.LineItemNo,
            //                                            FTYPE = (Int16)bunker.CONTYPE,
            //                                            ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                            PARTNO = satir.ComponentPartNo
            //                                        });
            //                                    }
            //                                    else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                    {
            //                                        gonderimEsleme.Add(new GonderimEsleme()
            //                                        {
            //                                            REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                            NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                            AMOUNT = Math.Round(Amount.ToDbl(), 2),
            //                                            NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                            ITEMNO = (Int16)satir.LineItemNo,
            //                                            FTYPE = (Int16)bunker.CONTYPE,
            //                                            ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                            PARTNO = satir.ComponentPartNo
            //                                        });
            //                                    }
            //                                }
            //                            }
            //                            else
            //                            {
            //                                gonderimEsleme.Add(new GonderimEsleme()
            //                                {
            //                                    REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                    NAMEIST = bunker.NAMEIST.ToString(),
            //                                    AMOUNT = Math.Round(Amount.ToDbl(), 2),
            //                                    NAMEITEM = bunker.NAMEITM.ToString(),
            //                                    ITEMNO = (Int16)satir.LineItemNo,
            //                                    FTYPE = (Int16)bunker.CONTYPE,
            //                                    ITMPARTNO = bunker.ITMPARTNO.ToString(),
            //                                    PARTNO = satir.ComponentPartNo
            //                                });
            //                            }
            //                            break;
            //                        default:
            //                            break;
            //                    }

            //                }
            //                string ss = gonderimEsleme.Count.ToString();
            //            }
            //            if (satir.OrderCode.ToUpper().Equals("F"))
            //            {

            //            }

            //        }
            //        if (tekilSiparisIcerik != null && tekilSiparisIcerik.Count > 0 && gonderimEsleme.Count > 0)
            //        {
            //            string FieldNames = "", FieldValues = "", UpdateQuery = "";
            //            FieldNames = "KOD,ORDER_NO,RELEASE_NO,SEQUENCE_NO,ORDER_CODE,RECETE_PART_NO, RECETE_PART_DESC,MIKTAR,MIXING_TIME,MIKSER_NO,CUSTOMER_ID, CUSTOMER_NAME, ADRESS_ID, ADRESS_DESC";
            //            FieldValues = "(SELECT ISNULL(MAX(KOD),0)+1 FROM IFSPLAN)"
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderNo, 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].ReleaseNo, 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].SequenceNo, 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderCode, 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].PartNo, 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].PartDesc), 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].RevisedQtyDue.ToString().Replace(',', '.'), 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].MixingTime.ToString().Replace(',', '.'), 1)
            //                + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].Mikser.ToString().Replace(',', '.'), 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerId), 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerName), 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressId), 1)
            //                + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressDesc), 1)
            //                //+ "," + EOFGlobalMethods.FieldReturnValues(KUMAA.ToString().Replace(',', '.'), 2)
            //                //+ "," + EOFGlobalMethods.FieldReturnValues(KUMBB.ToString().Replace(',', '.'), 2)
            //                //+ "," + EOFGlobalMethods.FieldReturnValues(KUMCC.ToString().Replace(',', '.'), 2)
            //                //+ "," + EOFGlobalMethods.FieldReturnValues(KUMDD.ToString().Replace(',', '.'), 2)
            //                ;

            //            File.WriteAllText("OrnekKAyit.txt", JsonConvert.SerializeObject(gonderimEsleme));
            //            foreach (var esleme in gonderimEsleme)
            //            {
            //                bool hasDouble = false;
            //                int ValueInt = 0;
            //                double ValueDouble = 0.00;

            //                if (esleme.FTYPE == 5)
            //                {
            //                    hasDouble = true;
            //                    ValueDouble = Convert.ToDouble(Math.Round(Convert.ToDouble(esleme.AMOUNT), 2));
            //                }
            //                else
            //                {
            //                    hasDouble = false;
            //                    ValueInt = Convert.ToInt32(Math.Round(Convert.ToDouble(esleme.AMOUNT), 0));
            //                }

            //                FieldNames = FieldNames + "," + esleme.NAMEIST.ToString() + "," + esleme.NAMEITEM.ToString()
            //                                        + "," + esleme.ITMPARTNO.ToString();
            //                FieldValues = FieldValues + "," + BN.FieldReturnValues((hasDouble == false ? ValueInt.ToString() : ValueDouble.ToString()).Replace(',', '.'), 1)
            //                                        + "," + BN.FieldReturnValues(esleme.ITEMNO.ToString().Replace(',', '.'), 1)
            //                                        + "," + BN.FieldReturnValues(esleme.PARTNO.ToString().Replace(',', '.'), 1);
            //            }
            //            UpdateQuery = "INSERT INTO dbo.IFSPLAN (" + FieldNames + ") VALUES (" + FieldValues + ")";
            //            File.WriteAllText("OrnekSorgu" + tekilSiparisIcerik[0].OrderNo + ".txt", UpdateQuery);
            //            int affected = ctx.Database.ExecuteSqlCommand(UpdateQuery);
            //            if (affected <= 0)
            //            {
            //                MessageBox.Show(tekilSiparisIcerik[0].OrderNo + " nolu İş Emri eklenemedi.");
            //            }
            //        }
            //    }

            //    _logger.LogInfo("İş emri senkronizasyonu tamamlandı");
            //    return orders;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("İş emri senkronizasyonu hatası", ex);
            //    return null;
            //}

        }


        /// <summary>
        /// MALZEME listesini çek ve MALZEME tablosuna kaydet
        /// </summary>
        public async Task<bool> SyncMaterialsAsync(string contract = null)
        {
            try
            {
                _logger.LogInfo("MALZEME senkronizasyonu başlatılıyor...");

                var materials = await _apiService.GetMaterialListAsync(contract);

                if (materials == null || materials.Count == 0)
                {
                    _logger.LogWarning("MALZEME bulunamadı");
                    return false;
                }

                _logger.LogInfo($"{materials.Count} adet malzeme bulundu");

                using var ctx = new TesisContext();
                foreach (MALZEME item in materials)
                {
                    MALZEME aktifMalzeme = await ctx.MALZEMEs.Where(x => x.PART_NO.Equals(item.PART_NO)).FirstOrDefaultAsync();
                    if (aktifMalzeme != null)
                    {
                        item.KOD = aktifMalzeme.KOD;
                        item.AD = aktifMalzeme.AD;
                        item.STOK_MIKTARI = aktifMalzeme.STOK_MIKTARI;
                        item.STOK_KONTROL = aktifMalzeme.STOK_KONTROL;
                        item.ACIKLAMA = aktifMalzeme.ACIKLAMA;
                        item.MINIMUM_MIKTAR = aktifMalzeme.MINIMUM_MIKTAR;
                        item.SILO_KAPASITE = aktifMalzeme.SILO_KAPASITE;
                    }
                    item.PROD_FAMILY = BN.KarakterDuzelt(item.PROD_FAMILY);
                    item.CIMENTO = "false";
                    item.AGREGA = "false";
                    item.KATKI = "false";
                    item.KUL = "false";
                    item.KUM = "false";
                    item.SU = "false";
                    item.TOZ = "false";
                    switch (item.PART_FAMILY_CODE)
                    {
                        case "01":
                            //item.KATKI = "true";
                            break;
                        case "02":
                            item.AGREGA = "true";
                            break;
                        case "03":
                            item.KUM = "true";
                            break;
                        case "04":
                            item.CIMENTO = "true";
                            break;
                        case "05":
                            item.KATKI = "true";
                            break;
                        case "06":
                            item.KUL = "true";
                            break;
                        case "07":
                            //item.KATKI = "true";
                            break;
                        case "08":
                            //item.KATKI = "true";
                            break;
                        case "13":
                            item.SU = "true";
                            break;
                        case "10":
                            item.KATKI = "true";
                            break;
                        default:
                            item.CIMENTO = "NULL";
                            item.AGREGA = "NULL";
                            item.KATKI = "NULL";
                            item.KUL = "NULL";
                            item.KUM = "NULL";
                            item.SU = "NULL";
                            item.TOZ = "NULL";
                            break;
                    }

                    if (item.KOD == 0)
                    {
                        ctx.MALZEMEs.Add(item);
                        _logger.LogInfo($"Yeni malzeme eklendi: {item.PART_NO} - {item.AD}");
                    }
                    else
                    {
                        ctx.MALZEMEs.AddOrUpdate(item);
                        _logger.LogInfo($"MALZEME güncellendi: {item.PART_NO} - {item.AD}");
                    }
                    await ctx.SaveChangesAsync();
                }

                _logger.LogInfo("MALZEME senkronizasyonu tamamlandı");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("MALZEME senkronizasyonu hatası", ex);
                return false;
            }
        }


        /// <summary>
        /// MALZEME listesini çek
        /// </summary>
        public async Task<List<MALZEME>> GetMaterialsAsync(string contract = null)
        {
            //try
            //{
            //    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "malzeme.json");

            //    if (!File.Exists(filePath))
            //    {
            //        _logger.LogWarning($"malzeme.json dosyası bulunamadı: {filePath}");
            //        return new List<MALZEME>();
            //    }

            //    var jsonContent = File.ReadAllText(filePath);
            //    var innerJson = JsonConvert.DeserializeObject<string>(jsonContent);
            //    var materials = JsonConvert.DeserializeObject<ODataResponse<MALZEME>>(innerJson);

            //    //var materials = await _apiService.GetMaterialListAsync(contract);
            //    return materials.Value ?? new List<MALZEME>();
            //}
            //catch (JsonException ex)
            //{
            //    _logger.LogError("JSON deserialize hatası", ex);
            //    throw;
            //}
            try
            {
                _logger.LogInfo("MALZEME senkronizasyonu başlatılıyor...");

                var materials = await _apiService.GetMaterialListAsync(contract);
                //var jsonContent = File.ReadAllText("malzeme.json");
                //var materials = JsonConvert.DeserializeObject<List<MALZEME>>(jsonContent);

                if (materials == null || materials.Count == 0)
                {
                    _logger.LogWarning("MALZEME bulunamadı");
                    return null;
                }

                _logger.LogInfo($"{materials.Count} adet malzeme bulundu");

                //foreach (var material in materials)
                //{
                //    var existing = await _malzemeRepository.GetByPartNoAsync(material.PART_NO);
                //    if (existing == null)
                //    {
                //        await _malzemeRepository.InsertAsync(material);
                //        _logger.LogInfo($"Yeni malzeme eklendi: {material.PART_NO} - {material.AD}");
                //    }
                //    else
                //    {
                //        material.KOD = existing.KOD;
                //        await _malzemeRepository.UpdateAsync(material);
                //        _logger.LogInfo($"MALZEME güncellendi: {material.PART_NO} - {material.AD}");
                //    }
                //}

                _logger.LogInfo("MALZEME senkronizasyonu tamamlandı");
                return materials;
            }
            catch (Exception ex)
            {
                _logger.LogError("MALZEME senkronizasyonu hatası", ex);
                return null;
            }

        }


        /// <summary>
        /// Raporlanmamış sevkiyatları IFS'e gönder
        /// </summary>
        public async Task<bool> SendPeriodDetailAsync(string systemId, string messageType, string messageText)
        {
            try
            {
                _logger.LogInfo($"{systemId} referanslı IFS üretim gönderiliyor.");


                var success = await _apiService.ReportWorkOrderAsync(
                    systemId,
                    messageType,
                    messageText
                );

                if (success)
                    _logger.LogInfo($"{systemId} referanslı IFS üretim gönderim işlemi tamamlandı.");
                else
                    _logger.LogInfo($"{systemId} referanslı IFS üretim gönderim işleminde hata alındı.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Periyot Detay gönderim hatası", ex);
                return false;
            }
        }


        /// <summary>
        /// Raporlanmamış sevkiyatları IFS'e gönder
        /// </summary>
        public async Task<bool> ReportUnreportedShipmentsAsync()
        {
            try
            {
                _logger.LogInfo("Raporlanmamış sevkiyatlar kontrol ediliyor...");

                var unreported = await _sevkiyatRepository.GetUnreportedAsync();

                if (unreported == null || unreported.Count == 0)
                {
                    _logger.LogInfo("Raporlanmamış sevkiyat bulunamadı");
                    return true;
                }

                _logger.LogInfo($"{unreported.Count} adet raporlanmamış sevkiyat bulundu");

                foreach (var sevkiyat in unreported)
                {
                    if (string.IsNullOrEmpty(sevkiyat.ORDER_NO))
                        continue;

                    var messageText = BuildWorkOrderReportMessage(sevkiyat);
                    var systemId = sevkiyat.ORDER_NO; // veya başka bir sistem ID

                    var success = await _apiService.ReportWorkOrderAsync(
                        systemId,
                        "URETIM",
                        messageText
                    );

                    if (success)
                    {
                        sevkiyat.DURUM = "REPORTED";
                        await _sevkiyatRepository.UpdateAsync(sevkiyat);
                        _logger.LogInfo($"Sevkiyat raporlandı: ORDER_NO={sevkiyat.ORDER_NO}");
                    }
                    else
                    {
                        _logger.LogWarning($"Sevkiyat raporlanamadı: ORDER_NO={sevkiyat.ORDER_NO}");
                    }
                }

                _logger.LogInfo("Sevkiyat raporlama işlemi tamamlandı");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Sevkiyat raporlama hatası", ex);
                return false;
            }
        }


        /// <summary>
        /// Work order report mesajını oluştur
        /// Postman collection'daki format'a göre
        /// </summary>
        private string BuildWorkOrderReportMessage(SEVKIYAT sevkiyat)
        {
            // Postman collection'daki format:
            // ORDER_NO^298~RELEASE_NO^*~SEQUENCE_NO^*~START_TIME^24.09.2025 11:20:48~FINISH_TIME^24.09.2025 11:22:31~...

            var message = new System.Text.StringBuilder();
            message.Append($"ORDER_NO^{sevkiyat.ORDER_NO ?? ""}~");
            message.Append($"RELEASE_NO^{sevkiyat.RELEASE_NO ?? "*"}~");
            message.Append($"SEQUENCE_NO^{sevkiyat.SEQUENCE_NO ?? "*"}~");
            message.Append($"START_TIME^{sevkiyat.START_TIME ?? ""}~");
            message.Append($"FINISH_TIME^{sevkiyat.FINISH_TIME ?? ""}~");
            message.Append($"PERIOD_VALUE^2~");
            message.Append($"MIXING_TIME^10~");
            message.Append($"RECYCLE_WATER_QTY^0~");
            message.Append($"RESET_INFO^{sevkiyat.RESET_INFO ?? ""}~");
            message.Append($"CLEANING_INFO^~");
            message.Append($"QTY_COMPLETED^1~");

            // Line item'lar (örnek format)
            message.Append($"LINE_ITEM_NO_1^1~");
            message.Append($"ISSUE_QTY_1^{sevkiyat.AGREGA1_VER ?? "0"}~");
            message.Append($"RECYCLE_DENSITY_1^0.00~");
            message.Append($"RECYCLE_ADJUST_QTY_1^0~");
            message.Append($"EXTRA_WATER_QTY_1^0~");
            message.Append($"HUMIDITY_1^0.00~");
            message.Append($"REVISED_HUMIDITY_1^0~");

            message.Append($"END_FLAG^TRUE~");

            return message.ToString();
        }
    
    }
}

