using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENOYAEntegrasyonV2.Models.Entities
{
    public class BunkerLineConfig
    {
        public int LineNo { get; set; }

        // PERDETAY içindeki alan isimleri (ör: "AG1PVER", "CM1PVER"...)
        public List<string> IssueQuantityFields { get; } = new();
        public List<string> HumidityFields { get; } = new();
        public List<string> RevisedHumidityFields { get; } = new();
        public List<string> RecycleDensityFields { get; } = new();
        public List<string> RecycleAdjustQuantityFields { get; } = new();

        // Su satırı mı (SU1_ITEMNO’ya bağlı satır)
        public bool IsWaterLine { get; set; }
    }
}
