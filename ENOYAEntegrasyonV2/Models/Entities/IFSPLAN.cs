using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ENOYAEntegrasyonV2.Models.Entities
{
    /// <summary>
    /// IFSPLAN tablosu entity modeli
    /// ENOYAMODELLEME.sql dosyasına göre oluşturuldu
    /// REST API'den gelen plan verilerini temsil eder
    /// </summary>
    public class IFSPLAN
    {
        [Key]
        public decimal KOD { get; set; }

        public string? ORDER_NO { get; set; }
        public string? RELEASE_NO { get; set; }
        public string? SEQUENCE_NO { get; set; }
        public string? ORDER_CODE { get; set; }
        public string? RECETE_PART_NO { get; set; }
        public string? RECETE_PART_DESC { get; set; }
        public string? MIKTAR { get; set; }
        public string? AG1_IST { get; set; }
        public string? AG2_IST { get; set; }
        public string? AG3_IST { get; set; }
        public string? AG4_IST { get; set; }
        public string? AG5_IST { get; set; }
        public string? AG6_IST { get; set; }
        public string? CM1_IST { get; set; }
        public string? CM2_IST { get; set; }
        public string? CM3_IST { get; set; }
        public string? CM4_IST { get; set; }
        public string? SU1_IST { get; set; }
        public string? SU2_IST { get; set; }
        public string? KT1_IST { get; set; }
        public string? KT2_IST { get; set; }
        public string? KT3_IST { get; set; }
        public string? KT4_IST { get; set; }
        public string? DURUM { get; set; }
        public string? MIXING_TIME { get; set; }
        public string? AG1_ITEMNO { get; set; }
        public string? AG2_ITEMNO { get; set; }
        public string? AG3_ITEMNO { get; set; }
        public string? AG4_ITEMNO { get; set; }
        public string? AG5_ITEMNO { get; set; }
        public string? AG6_ITEMNO { get; set; }
        public string? CM1_ITEMNO { get; set; }
        public string? CM2_ITEMNO { get; set; }
        public string? CM3_ITEMNO { get; set; }
        public string? CM4_ITEMNO { get; set; }
        public string? SU1_ITEMNO { get; set; }
        public string? SU2_ITEMNO { get; set; }
        public string? KT1_ITEMNO { get; set; }
        public string? KT2_ITEMNO { get; set; }
        public string? KT3_ITEMNO { get; set; }
        public string? KT4_ITEMNO { get; set; }
        public string? MIKSER_NO { get; set; }
        public string? AG1_PARTNO { get; set; }
        public string? AG2_PARTNO { get; set; }
        public string? AG3_PARTNO { get; set; }
        public string? AG4_PARTNO { get; set; }
        public string? AG5_PARTNO { get; set; }
        public string? AG6_PARTNO { get; set; }
        public string? CM1_PARTNO { get; set; }
        public string? CM2_PARTNO { get; set; }
        public string? CM3_PARTNO { get; set; }
        public string? CM4_PARTNO { get; set; }
        public string? SU1_PARTNO { get; set; }
        public string? SU2_PARTNO { get; set; }
        public string? KT1_PARTNO { get; set; }
        public string? KT2_PARTNO { get; set; }
        public string? KT3_PARTNO { get; set; }
        public string? KT4_PARTNO { get; set; }
        public string? CUSTOMER_ID { get; set; }
        public string? CUSTOMER_NAME { get; set; }
        public string? ADRESS_ID { get; set; }
        public string? ADRESS_DESC { get; set; }

    }
}

