using ENOYAEntegrasyonV2.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ENOYAEntegrasyonV2.Models.Entities
{
    [Table("PERDETAY")] 
    public class PERDETAY
    {
        [Key]
        [Column] public decimal URTKOD { get; set; }
        [Column] public string? AG1PIST { get; set; }
        [Column] public string? AG2PIST { get; set; }
        [Column] public string? AG3PIST { get; set; }
        [Column] public string? AG4PIST { get; set; }
        [Column] public string? AG5PIST { get; set; }
        [Column] public string? AG6PIST { get; set; }
        [Column] public string? CM1PIST { get; set; }
        [Column] public string? CM2PIST { get; set; }
        [Column] public string? CM3PIST { get; set; }
        [Column] public string? CM4PIST { get; set; }
        [Column] public string? SU1PIST { get; set; }
        [Column] public string? SU2PIST { get; set; }
        [Column] public string? KT1PIST { get; set; }
        [Column] public string? KT2PIST { get; set; }
        [Column] public string? KT3PIST { get; set; }
        [Column] public string? KT4PIST { get; set; }
        [Column] public string? AG1PNIST { get; set; }
        [Column] public string? AG2PNIST { get; set; }
        [Column] public string? AG3PNIST { get; set; }
        [Column] public string? AG4PNIST { get; set; }
        [Column] public string? AG5PNIST { get; set; }
        [Column] public string? AG6PNIST { get; set; }
        [Column] public string? SU1PNIST { get; set; }
        [Column] public string? SU2PNIST { get; set; }
        [Column] public string? AG1PVER { get; set; }
        [Column] public string? AG2PVER { get; set; }
        [Column] public string? AG3PVER { get; set; }
        [Column] public string? AG4PVER { get; set; }
        [Column] public string? AG5PVER { get; set; }
        [Column] public string? AG6PVER { get; set; }
        [Column] public string? CM1PVER { get; set; }
        [Column] public string? CM2PVER { get; set; }
        [Column] public string? CM3PVER { get; set; }
        [Column] public string? CM4PVER { get; set; }
        [Column] public string? SU1PVER { get; set; }
        [Column] public string? SU2PVER { get; set; }
        [Column] public string? KT1PVER { get; set; }
        [Column] public string? KT2PVER { get; set; }
        [Column] public string? KT3PVER { get; set; }
        [Column] public string? KT4PVER { get; set; }
        [Column] public string? AG1PNEMORT { get; set; }
        [Column] public string? AG2PNEMORT { get; set; }
        [Column] public string? AG3PNEMORT { get; set; }
        [Column] public string? AG4PNEMORT { get; set; }
        [Column] public string? AG5PNEMORT { get; set; }
        [Column] public string? AG6PNEMORT { get; set; }
        [Column] public string? AG1PGKORT { get; set; }
        [Column] public string? AG2PGKORT { get; set; }
        [Column] public string? AG3PGKORT { get; set; }
        [Column] public string? AG4PGKORT { get; set; }
        [Column] public string? AG5PGKORT { get; set; }
        [Column] public string? AG6PGKORT { get; set; }
        [Column] public string? AG1PNEMSUTOP { get; set; }
        [Column] public string? AG2PNEMSUTOP { get; set; }
        [Column] public string? AG3PNEMSUTOP { get; set; }
        [Column] public string? AG4PNEMSUTOP { get; set; }
        [Column] public string? AG5PNEMSUTOP { get; set; }
        [Column] public string? AG6PNEMSUTOP { get; set; }
        [Column] public string? AG1PGKSUTOP { get; set; }
        [Column] public string? AG2PGKSUTOP { get; set; }
        [Column] public string? AG3PGKSUTOP { get; set; }
        [Column] public string? AG4PGKSUTOP { get; set; }
        [Column] public string? AG5PGKSUTOP { get; set; }
        [Column] public string? AG6PGKSUTOP { get; set; }
        [Column] public string? PMIXTIME { get; set; }
        [Column] public string? ILVSUA { get; set; }
        [Column] public string? ILVSUE { get; set; }
        decimal _PERKOD { get; set; }
        [Column] public decimal PERKOD {
            get
            {
                return _PERKOD;
            }
            set
            {
                _PERKOD = value.ToDecimal();
            }
        }
        [Column] public string? TARIHSAAT { get; set; }

    }

}
