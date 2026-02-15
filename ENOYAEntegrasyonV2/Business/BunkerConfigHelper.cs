using ENOYAEntegrasyonV2.Models.Entities;
using System.Collections.Generic;
using System.Reflection;

namespace ENOYAEntegrasyonV2.Business
{
    public static class BunkerConfigHelper
    {
        public static Dictionary<int, BunkerLineConfig> BuildLineConfigs(IFSPLAN plan, List<SiloAdlari> silos)
        {
            var result = new Dictionary<int, BunkerLineConfig>();

            // Her silo için: IFSPLAN.AG1_ITEMNO, CM1_ITEMNO vs üzerinden hangi line'a bağlı olduğunu bul
            foreach (var silo in silos)
            {
                if (silo.NAMEITM == null)
                    continue;

                var itemPropName = silo.NAMEITM.Value.ToString(); // Ör: "AG1_ITEMNO"
                var itemProp = typeof(IFSPLAN).GetProperty(itemPropName);
                if (itemProp == null)
                    continue;

                var itemValStr = itemProp.GetValue(plan) as string;
                if (!int.TryParse(itemValStr, out var lineNo) || lineNo == 0)
                    continue;

                if (!result.TryGetValue(lineNo, out var cfg))
                {
                    cfg = new BunkerLineConfig { LineNo = lineNo };
                    result[lineNo] = cfg;
                }

                // ISSUE_QUANTITY alanı
                if (silo.ISSUEQUANTITY.HasValue)
                    cfg.IssueQuantityFields.Add(silo.ISSUEQUANTITY.Value.ToString());

                // Nem hesabı var ise ilgili alanlar
                if (silo.IFSDATA == enumBunkerIfsData.Nem_HesabıVar)
                {
                    if (silo.HUMIDITY.HasValue)
                        cfg.HumidityFields.Add(silo.HUMIDITY.Value.ToString());
                    if (silo.REVISEDHUMIDITY.HasValue)
                        cfg.RevisedHumidityFields.Add(silo.REVISEDHUMIDITY.Value.ToString());
                    if (silo.RECYCLEDENSITY.HasValue)
                        cfg.RecycleDensityFields.Add(silo.RECYCLEDENSITY.Value.ToString());
                    if (silo.RADJUSTQUANTITY.HasValue)
                        cfg.RecycleAdjustQuantityFields.Add(silo.RADJUSTQUANTITY.Value.ToString());
                }

                // SU1_ITEMNO ise bu line "su satırı" olarak işaretleniyor
                if (silo.NAMEITM == enumBunkerItemAlanAdi.SU1_ITEMNO)
                    cfg.IsWaterLine = true;
            }

            return result;
        }

        private static double GetDouble(object? value)
        {
            if (value == null)
                return 0;
            if (value is double d)
                return d;
            if (value is float f)
                return f;
            if (value is decimal dec)
                return (double)dec;
            double.TryParse(value.ToString()?.Replace('.', ','), out var res);
            return res;
        }

        public static double SumFields<T>(T entity, IEnumerable<string> fieldNames)
        {
            var type = typeof(T);
            double sum = 0;

            foreach (var name in fieldNames)
            {
                var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) continue;

                var val = prop.GetValue(entity);
                sum += GetDouble(val);
            }

            return sum;
        }
    }
}
