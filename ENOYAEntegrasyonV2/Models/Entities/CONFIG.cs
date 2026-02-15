using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENOYAEntegrasyonV2.Models.Entities
{
    [Table("CONFIG")]
    public class CONFIG
    {
        [Key]
        public string? HAKSAYI { get; set; }
        public string? SECILIDIL { get; set; }
        public string? KULLANICIKOD { get; set; }
        public string? RECETEKOD { get; set; }
        public string? MUSTERIKOD { get; set; }
        public string? SANTIYEKOD { get; set; }
        public string? KAMYONKOD { get; set; }
        public string? SURUCUKOD { get; set; }
        public string? HIZMET1KOD { get; set; }
        public string? HIZMET2KOD { get; set; }
        public string? MIKTAR { get; set; }
        public string? HAZIRMIKSEVKKOD { get; set; }
        public string? HAZIRMIKMIKTAR { get; set; }
        public string? MIXCAPHELPER { get; set; }
        public string? TOTALPERYOT { get; set; }
        public string? ILAVESU { get; set; }
        public string? SIPARISKOD { get; set; }
        public string? BASSAAT { get; set; }
        public string? MODBUS { get; set; }
        public string? URTDURUM { get; set; }
        public string? PLANKOD { get; set; }
        public string? PLANNEXTKOD { get; set; }
        public string? PLANPREVKOD { get; set; }
        public string? KAMREVSEVKKOD { get; set; }
        public string? GKMANDEG { get; set; }
        public string? GKOTODEG { get; set; }
        public string? GKUSTSNR { get; set; }
        public string? OZGAGR { get; set; }
        public string? AG1ANEM { get; set; }
        public string? AG2ANEM { get; set; }
        public string? AG3ANEM { get; set; }
        public string? AG4ANEM { get; set; }
        public string? AG5ANEM { get; set; }
        public string? AG6ANEM { get; set; }
        public string? AG1ENEM { get; set; }
        public string? AG2ENEM { get; set; }
        public string? AG3ENEM { get; set; }
        public string? AG4ENEM { get; set; }
        public string? AG5ENEM { get; set; }
        public string? AG6ENEM { get; set; }
        public string? IFSPLANKOD { get; set; }
        public string? AG1BLMYZDE { get; set; }
        public string? AG2BLMYZDE { get; set; }
        public string? AG3BLMYZDE { get; set; }
        public string? AG4BLMYZDE { get; set; }
        public string? AG5BLMYZDE { get; set; }
        public string? AG6BLMYZDE { get; set; }
        public string? CIMENTOSILO { get; set; }
        public string? BNKGUNCEL { get; set; }
        public string? AGZERO { get; set; }
        public string? AGCARP { get; set; }
        public string? AGBOL { get; set; }
        public string? AG2ZERO { get; set; }
        public string? AG2CARP { get; set; }
        public string? AG2BOL { get; set; }
        public string? CMZERO { get; set; }
        public string? CMCARP { get; set; }
        public string? CMBOL { get; set; }
        public string? SUZERO { get; set; }
        public string? SUCARP { get; set; }
        public string? SUBOL { get; set; }
        public string? KTZERO { get; set; }
        public string? KTCARP { get; set; }
        public string? KTBOL { get; set; }
        public string? AGDARALMT { get; set; }
        public string? AG2DARALMT { get; set; }
        public string? CMDARALMT { get; set; }
        public string? SUDARALMT { get; set; }
        public string? KTDARALMT { get; set; }
        public string? KULSILO { get; set; }
        public string? ILAVEKT1 { get; set; }
        public string? ILAVEKT2 { get; set; }
        public string? ILAVEKT3 { get; set; }
        public string? ILAVEKT4 { get; set; }
    }
}
