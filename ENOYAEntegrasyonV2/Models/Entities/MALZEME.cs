using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ENOYAEntegrasyonV2.Models.Entities
{
    /// <summary>
    /// MALZEME tablosu entity modeli
    /// ENOYAMODELLEME.sql dosyasına göre oluşturuldu
    /// </summary>
    public class MALZEME
    {
        [Key]
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Önemli
        public decimal KOD { get; set; }

        [DataMember]
        [JsonProperty("Description")]
        public string? AD { get; set; }
        [DataMember]
        public string? STOK_MIKTARI { get; set; }
        [DataMember]
        public string? STOK_KONTROL { get; set; }
        [DataMember]
        
        public string? ACIKLAMA { get; set; }
        [DataMember]
        public string? MINIMUM_MIKTAR { get; set; }
        [DataMember]
        public string? SILO_KAPASITE { get; set; }
        [DataMember]
        public string? AGREGA { get; set; }
        [DataMember]
        public string? CIMENTO { get; set; }
        [DataMember]
        public string? KUL { get; set; }
        [DataMember]
        public string? TOZ { get; set; }
        [DataMember]
        public string? KUM { get; set; }
        [DataMember]
        public string? SU { get; set; }
        [DataMember]
        public string? KATKI { get; set; }
        [JsonProperty("EksiNemLimit")]
        [DataMember]
        public string? EKSINEMLIMIT { get; set; }
        [JsonProperty("PartNo")]
        [DataMember]
        public string? PART_NO { get; set; }
        [JsonProperty("PartProductFamily")]
        [DataMember]
        public string? PROD_FAMILY { get; set; }
        [JsonProperty("GksYogunluk")]
        [DataMember]
        public string? GKS_YOG { get; set; }
        //[JsonProperty("GksYogunluk2")]
        [DataMember]
        [JsonProperty("GksInceMlz")]
        public string? GKS_INCE_YOG { get; set; }
        [JsonProperty("PartFamilyCode")]
        [NotMapped]
        public string? PART_FAMILY_CODE { get; set; }
        [JsonProperty("KDegeri")]
        [NotMapped]
        public string? KDEGERI { get; set; }
        [JsonProperty("KimyasalKatkiSuOrani")]
        [NotMapped]
        public string? KIMYASALKATKISUORANI { get; set; }
        [JsonProperty("BaglamaOrani")]
        [NotMapped]
        public string? BAGLAMAORANI { get; set; }
        //[JsonProperty("GksYogunluk2")]
        //[NotMapped]
        //public string? GKS_YOGUNLUK2 { get; set; }
        

    }
}

