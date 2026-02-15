using Newtonsoft.Json;

namespace ENOYAEntegrasyonV2.Models.Entities
{
    /// <summary>
    /// IFS API'den gelen plan satır verilerini temsil eder
    /// Bu model, API'den gelen ham JSON verisini parse etmek için kullanılır
    /// Daha sonra IFSPLAN entity'sine dönüştürülür
    /// </summary>
    public class IFSPLANLine
    {
        [JsonProperty("luname")]
        public string? Luname { get; set; }

        [JsonProperty("OrderNo")]
        public string? OrderNo { get; set; }

        [JsonProperty("ReleaseNo")]
        public string? ReleaseNo { get; set; }

        [JsonProperty("SequenceNo")]
        public string? SequenceNo { get; set; }

        [JsonProperty("OrderCode")]
        public string? OrderCode { get; set; }

        [JsonProperty("ProcessType")]
        public string? ProcessType { get; set; }

        [JsonProperty("DateEntered")]
        public string? DateEntered { get; set; }

        [JsonProperty("Contract")]
        public string? Contract { get; set; }

        [JsonProperty("PartNo")]
        public string? PartNo { get; set; }

        [JsonProperty("PartDesc")]
        public string? PartDesc { get; set; }

        [JsonProperty("RevisedQtyDue")]
        public decimal? RevisedQtyDue { get; set; }

        [JsonProperty("EngChgLevel")]
        public string? EngChgLevel { get; set; }

        [JsonProperty("StructureAlternative")]
        public string? StructureAlternative { get; set; }

        [JsonProperty("AlternativeDesc")]
        public string? AlternativeDesc { get; set; }

        [JsonProperty("LineItemNo")]
        public int? LineItemNo { get; set; }

        [JsonProperty("ComponentPartNo")]
        public string? ComponentPartNo { get; set; }

        [JsonProperty("CompPartDesc")]
        public string? CompPartDesc { get; set; }

        [JsonProperty("ProductFamily")]
        public string? ProductFamily { get; set; }

        [JsonProperty("QtyRequired")]
        public decimal? QtyRequired { get; set; }

        [JsonProperty("QtyPerAssembly")]
        public decimal? QtyPerAssembly { get; set; }

        [JsonProperty("Mikser")]
        public string? Mikser { get; set; }

        [JsonProperty("RecycleUsageFactor")]
        public decimal? RecycleUsageFactor { get; set; }

        [JsonProperty("RecycleUsageFactorV")]
        public decimal? RecycleUsageFactorV { get; set; }

        [JsonProperty("MixingTime")]
        public decimal? MixingTime { get; set; }

        [JsonProperty("RoutingAlternative")]
        public string? RoutingAlternative { get; set; }

        [JsonProperty("EarliestStartDate")]
        public string? EarliestStartDate { get; set; }

        [JsonProperty("CustomerId")]
        public string? CustomerId { get; set; }

        [JsonProperty("CustomerName")]
        public string? CustomerName { get; set; }

        [JsonProperty("A")]
        public string? A { get; set; }

        [JsonProperty("B")]
        public string? B { get; set; }

        [JsonProperty("C")]
        public string? C { get; set; }

        [JsonProperty("D")]
        public string? D { get; set; }

        [JsonProperty("AdressId")]
        public string? AdressId { get; set; }

        [JsonProperty("AdressDesc")]
        public string? AdressDesc { get; set; }
    }
}
