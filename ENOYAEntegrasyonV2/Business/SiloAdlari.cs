using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ENOYAEntegrasyonV2.Business
{
    public class SiloAdlari
    {
        public Int32 ID { get; set; }// Bunker tanımındaki değer
        public enumBunkerTanimi? DEFINITION_ { get; set; }// Bunker tanımındaki görünen açıklama
        public enumBunkerKolonAdi? IDCOLUMNNAME { get; set; } //Bunker tanımının ENOYA tablolarındaki kolon adları
        public enumBunkerIstenenAlanAdi? NAMEIST { get; set; }// Bunker tanımının ENOYA tablolarındaki ISTENEN kolon adları
        public enumBunkerItemAlanAdi? NAMEITM { get; set; } //Bunker tanımının ENOYA ile IFS arasındaki eşleşmesi için kullanılacak ITEMNO eşleşmesi
        public enumBunkerIcerikTipi? CONTYPE { get; set; }
        public enumBunkerIfsData? IFSDATA { get; set; }
        public enumBunkerIssueQuantity? ISSUEQUANTITY { get; set; }// Bunker tanımının ENOYA tarafındaki VERILEN miktarların tutulduğu kolon isimleri
        public enumBunkerHumidity? HUMIDITY { get; set; } // Humidity verilerinin alınacağı kolon bilgisi
        public enumBunkerRecycleDensity? RECYCLEDENSITY { get; set; }
        public enumBunkerRevisedHumidity? REVISEDHUMIDITY { get; set; }
        public enumBunkerRAdjustQuantity? RADJUSTQUANTITY { get; set; }
        public Int16? CALCDENSITY { get; set; }
        public Int16? CALCADJUSTQUANTITY { get; set; }
        public Int16? CALCHUMIDITY { get; set; }
        public enumBunkerPercentField? PERCENTFIELD { get; set; }
        public enumBunkerItmPartNo? ITMPARTNO { get; set; }
        public enumBunkerTRField? TRFIELD { get; set; }
        public string? IFSPARTNO { get; set; } = "-1";

        public static List<SiloAdlari> listSiloAdlari()
        {
            List<SiloAdlari> lstNSiloAdlar = new List<SiloAdlari>();
            #region AGREGA1
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 1,
                DEFINITION_ = enumBunkerTanimi.Agrega_Bunkeri_1,
                IDCOLUMNNAME = enumBunkerKolonAdi.AGREGA1,
                NAMEIST = enumBunkerIstenenAlanAdi.AG1_IST,
                NAMEITM = enumBunkerItemAlanAdi.AG1_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Agrega,
                IFSDATA = enumBunkerIfsData.Nem_HesabıVar,
                ISSUEQUANTITY = enumBunkerIssueQuantity.AG1PVER,
                HUMIDITY = enumBunkerHumidity.AG1PNEMORT,
                RECYCLEDENSITY = enumBunkerRecycleDensity.AG1PGKORT,
                REVISEDHUMIDITY = enumBunkerRevisedHumidity.AG1PNEMSUTOP,
                RADJUSTQUANTITY = enumBunkerRAdjustQuantity.AG1PGKSUTOP,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = enumBunkerPercentField.AG1BLMYZDE,
                ITMPARTNO = enumBunkerItmPartNo.AG1_PARTNO,
                TRFIELD = enumBunkerTRField.AGREGA1_VER
            });
            #endregion

            #region AGREGA2
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 2,
                DEFINITION_ = enumBunkerTanimi.Agrega_Bunkeri_2,
                IDCOLUMNNAME = enumBunkerKolonAdi.AGREGA2,
                NAMEIST = enumBunkerIstenenAlanAdi.AG2_IST,
                NAMEITM = enumBunkerItemAlanAdi.AG2_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Agrega,
                IFSDATA = enumBunkerIfsData.Nem_HesabıVar,
                ISSUEQUANTITY = enumBunkerIssueQuantity.AG2PVER,
                HUMIDITY = enumBunkerHumidity.AG2PNEMORT,
                RECYCLEDENSITY = enumBunkerRecycleDensity.AG2PGKORT,
                REVISEDHUMIDITY = enumBunkerRevisedHumidity.AG2PNEMSUTOP,
                RADJUSTQUANTITY = enumBunkerRAdjustQuantity.AG2PGKSUTOP,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = enumBunkerPercentField.AG2BLMYZDE,
                ITMPARTNO = enumBunkerItmPartNo.AG2_PARTNO,
                TRFIELD = enumBunkerTRField.AGREGA2_VER
            });
            #endregion

            #region AGREGA3
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 3,
                DEFINITION_ = enumBunkerTanimi.Agrega_Bunkeri_3,
                IDCOLUMNNAME = enumBunkerKolonAdi.AGREGA3,
                NAMEIST = enumBunkerIstenenAlanAdi.AG3_IST,
                NAMEITM = enumBunkerItemAlanAdi.AG3_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Agrega,
                IFSDATA = enumBunkerIfsData.Nem_HesabıVar,
                ISSUEQUANTITY = enumBunkerIssueQuantity.AG3PVER,
                HUMIDITY = enumBunkerHumidity.AG3PNEMORT,
                RECYCLEDENSITY = enumBunkerRecycleDensity.AG3PGKORT,
                REVISEDHUMIDITY = enumBunkerRevisedHumidity.AG3PNEMSUTOP,
                RADJUSTQUANTITY = enumBunkerRAdjustQuantity.AG3PGKSUTOP,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = enumBunkerPercentField.AG3BLMYZDE,
                ITMPARTNO = enumBunkerItmPartNo.AG3_PARTNO,
                TRFIELD = enumBunkerTRField.AGREGA3_VER
            });
            #endregion

            #region AGREGA4
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 4,
                DEFINITION_ = enumBunkerTanimi.Agrega_Bunkeri_4,
                IDCOLUMNNAME = enumBunkerKolonAdi.AGREGA4,
                NAMEIST = enumBunkerIstenenAlanAdi.AG4_IST,
                NAMEITM = enumBunkerItemAlanAdi.AG4_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Agrega,
                IFSDATA = enumBunkerIfsData.Nem_HesabıVar,
                ISSUEQUANTITY = enumBunkerIssueQuantity.AG4PVER,
                HUMIDITY = enumBunkerHumidity.AG4PNEMORT,
                RECYCLEDENSITY = enumBunkerRecycleDensity.AG4PGKORT,
                REVISEDHUMIDITY = enumBunkerRevisedHumidity.AG4PNEMSUTOP,
                RADJUSTQUANTITY = enumBunkerRAdjustQuantity.AG4PGKSUTOP,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = enumBunkerPercentField.AG4BLMYZDE,
                ITMPARTNO = enumBunkerItmPartNo.AG4_PARTNO,
                TRFIELD = enumBunkerTRField.AGREGA4_VER
            });
            #endregion

            #region AGREGA5
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 5,
                DEFINITION_ = enumBunkerTanimi.Agrega_Bunkeri_5,
                IDCOLUMNNAME = enumBunkerKolonAdi.AGREGA5,
                NAMEIST = enumBunkerIstenenAlanAdi.AG5_IST,
                NAMEITM = enumBunkerItemAlanAdi.AG5_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Agrega,
                IFSDATA = enumBunkerIfsData.Nem_HesabıVar,
                ISSUEQUANTITY = enumBunkerIssueQuantity.AG5PVER,
                HUMIDITY = enumBunkerHumidity.AG5PNEMORT,
                RECYCLEDENSITY = enumBunkerRecycleDensity.AG5PGKORT,
                REVISEDHUMIDITY = enumBunkerRevisedHumidity.AG5PNEMSUTOP,
                RADJUSTQUANTITY = enumBunkerRAdjustQuantity.AG5PGKSUTOP,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = enumBunkerPercentField.AG5BLMYZDE,
                ITMPARTNO = enumBunkerItmPartNo.AG5_PARTNO,
                TRFIELD = enumBunkerTRField.AGREGA5_VER
            });
            #endregion

            #region AGREGA6
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 6,
                DEFINITION_ = enumBunkerTanimi.Agrega_Bunkeri_6,
                IDCOLUMNNAME = enumBunkerKolonAdi.AGREGA6,
                NAMEIST = enumBunkerIstenenAlanAdi.AG6_IST,
                NAMEITM = enumBunkerItemAlanAdi.AG6_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Agrega,
                IFSDATA = enumBunkerIfsData.Nem_HesabıVar,
                ISSUEQUANTITY = enumBunkerIssueQuantity.AG6PVER,
                HUMIDITY = enumBunkerHumidity.AG6PNEMORT,
                RECYCLEDENSITY = enumBunkerRecycleDensity.AG6PGKORT,
                REVISEDHUMIDITY = enumBunkerRevisedHumidity.AG6PNEMSUTOP,
                RADJUSTQUANTITY = enumBunkerRAdjustQuantity.AG6PGKSUTOP,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = enumBunkerPercentField.AG6BLMYZDE,
                ITMPARTNO = enumBunkerItmPartNo.AG6_PARTNO,
                TRFIELD = enumBunkerTRField.AGREGA6_VER
            });
            #endregion

            #region CIMENTO1
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 7,
                DEFINITION_ = enumBunkerTanimi.Çimento_Silo_1,
                IDCOLUMNNAME = enumBunkerKolonAdi.CIMENTO1,
                NAMEIST = enumBunkerIstenenAlanAdi.CM1_IST,
                NAMEITM = enumBunkerItemAlanAdi.CM1_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Çimento_Kül,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.CM1PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.CM1_PARTNO,
                TRFIELD = enumBunkerTRField.CIMENTO1_VER
            });
            #endregion

            #region CIMENTO2
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 8,
                DEFINITION_ = enumBunkerTanimi.Çimento_Silo_2,
                IDCOLUMNNAME = enumBunkerKolonAdi.CIMENTO2,
                NAMEIST = enumBunkerIstenenAlanAdi.CM2_IST,
                NAMEITM = enumBunkerItemAlanAdi.CM2_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Çimento_Kül,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.CM2PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.CM2_PARTNO,
                TRFIELD = enumBunkerTRField.CIMENTO2_VER
            });
            #endregion

            #region CIMENTO3
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 9,
                DEFINITION_ = enumBunkerTanimi.Çimento_Silo_3,
                IDCOLUMNNAME = enumBunkerKolonAdi.CIMENTO3,
                NAMEIST = enumBunkerIstenenAlanAdi.CM3_IST,
                NAMEITM = enumBunkerItemAlanAdi.CM3_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Çimento_Kül,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.CM3PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.CM3_PARTNO,
                TRFIELD = enumBunkerTRField.CIMENTO3_VER
            });
            #endregion

            #region CIMENTO4
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 10,
                DEFINITION_ = enumBunkerTanimi.Çimento_Silo_4,
                IDCOLUMNNAME = enumBunkerKolonAdi.CIMENTO4,
                NAMEIST = enumBunkerIstenenAlanAdi.CM4_IST,
                NAMEITM = enumBunkerItemAlanAdi.CM4_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Çimento_Kül,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.CM4PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.CM4_PARTNO,
                TRFIELD = enumBunkerTRField.CIMENTO4_VER
            });
            #endregion

            #region SU1
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 11,
                DEFINITION_ = enumBunkerTanimi.Su_1,
                IDCOLUMNNAME = enumBunkerKolonAdi.SU1,
                NAMEIST = enumBunkerIstenenAlanAdi.SU1_IST,
                NAMEITM = enumBunkerItemAlanAdi.SU2_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Su1,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.SU1PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 1,
                CALCADJUSTQUANTITY = 1,
                CALCHUMIDITY = 1,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.SU1_PARTNO,
                TRFIELD = enumBunkerTRField.SU1_VER
            });
            #endregion

            #region SU2
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 12,
                DEFINITION_ = enumBunkerTanimi.Su_2,
                IDCOLUMNNAME = enumBunkerKolonAdi.SU2,
                NAMEIST = enumBunkerIstenenAlanAdi.SU2_IST,
                NAMEITM = enumBunkerItemAlanAdi.SU2_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Su2,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.SU2PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 1,
                CALCADJUSTQUANTITY = 1,
                CALCHUMIDITY = 1,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.SU2_PARTNO,
                TRFIELD = enumBunkerTRField.SU2_VER
            });
            #endregion

            #region KATKI1
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 13,
                DEFINITION_ = enumBunkerTanimi.Katkı_Bunkeri_1,
                IDCOLUMNNAME = enumBunkerKolonAdi.KATKI1,
                NAMEIST = enumBunkerIstenenAlanAdi.KT1_IST,
                NAMEITM = enumBunkerItemAlanAdi.KT1_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Katkı,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.KT1PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.KT1_PARTNO,
                TRFIELD = enumBunkerTRField.KATKI1_VER
            });
            #endregion

            #region KATKI2
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 14,
                DEFINITION_ = enumBunkerTanimi.Katkı_Bunkeri_2,
                IDCOLUMNNAME = enumBunkerKolonAdi.KATKI2,
                NAMEIST = enumBunkerIstenenAlanAdi.KT2_IST,
                NAMEITM = enumBunkerItemAlanAdi.KT2_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Katkı,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.KT2PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.KT2_PARTNO,
                TRFIELD = enumBunkerTRField.KATKI2_VER
            });
            #endregion

            #region KATKI3
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 15,
                DEFINITION_ = enumBunkerTanimi.Katkı_Bunkeri_3,
                IDCOLUMNNAME = enumBunkerKolonAdi.KATKI3,
                NAMEIST = enumBunkerIstenenAlanAdi.KT3_IST,
                NAMEITM = enumBunkerItemAlanAdi.KT3_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Katkı,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.KT3PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.KT3_PARTNO,
                TRFIELD = enumBunkerTRField.KATKI3_VER
            });
            #endregion

            #region KATKI4
            lstNSiloAdlar.Add(new SiloAdlari()
            {
                ID = 16,
                DEFINITION_ = enumBunkerTanimi.Katkı_Bunkeri_4,
                IDCOLUMNNAME = enumBunkerKolonAdi.KATKI4,
                NAMEIST = enumBunkerIstenenAlanAdi.KT4_IST,
                NAMEITM = enumBunkerItemAlanAdi.KT4_ITEMNO,
                CONTYPE = enumBunkerIcerikTipi.Katkı,
                IFSDATA = enumBunkerIfsData.Nem_Hesabı_Yok,
                ISSUEQUANTITY = enumBunkerIssueQuantity.KT4PVER,
                HUMIDITY = 0,
                RECYCLEDENSITY = 0,
                REVISEDHUMIDITY = 0,
                RADJUSTQUANTITY = 0,
                CALCDENSITY = 0,
                CALCADJUSTQUANTITY = 0,
                CALCHUMIDITY = 0,
                PERCENTFIELD = 0,
                ITMPARTNO = enumBunkerItmPartNo.KT4_PARTNO,
                TRFIELD = enumBunkerTRField.KATKI4_VER
            });
            #endregion

            return lstNSiloAdlar;
        }

        public static List<SiloAdlari> siloTanimlari()
        {
            if (File.Exists("SiloTanimlar.json"))
                return JsonConvert.DeserializeObject<List<SiloAdlari>>(File.ReadAllText("SiloTanimlar.json"));
            else
                return null;
        }
    }
}
