using System;

namespace ENOYAEntegrasyonV2.Models.Entities
{
    /// <summary>
    /// IRSALIYE tablosu entity modeli
    /// ENOYAMODELLEME.sql dosyasına göre oluşturuldu
    /// </summary>
    public class Irsaliye
    {
        public decimal KOD { get; set; }
        public string? IRS_SERI_NO { get; set; }
        public string? IRS_NO { get; set; }
        public string? TARIH { get; set; }
        public string? SAAT { get; set; }
        public string? TARIHSAAT { get; set; }
        public string? MIKTAR { get; set; }
        public string? OTOMATIK { get; set; }
        public string? KULLANICI { get; set; }
        public string? HAZIR_MIKTAR { get; set; }
        public string? GERI_DONEN { get; set; }
        public string? GERI_DONEN_KOD { get; set; }
        public string? RECETE_KOD { get; set; }
        public string? RECETE_INDEX { get; set; }
        public string? RECETE_AD { get; set; }
        public string? MUSTERI_KOD { get; set; }
        public string? MUSTERI_INDEX { get; set; }
        public string? MUSTERI_AD { get; set; }
        public string? SANTIYE_KOD { get; set; }
        public string? SANTIYE_AD { get; set; }
        public string? KAMYON_KOD { get; set; }
        public string? KAMYON_PLK { get; set; }
        public string? SURUCU_KOD { get; set; }
        public string? SURUCU_AD { get; set; }
        public string? HIZMET1_KOD { get; set; }
        public string? HIZMET1_AD { get; set; }
        public string? HIZMET2_KOD { get; set; }
        public string? HIZMET2_AD { get; set; }
        public string? ILAVE_SU { get; set; }

        // Agrega alanları
        public string? AGREGA1_IST { get; set; }
        public string? AGREGA1_TART { get; set; }
        public string? AGREGA1_VER { get; set; }
        public string? AGREGA2_IST { get; set; }
        public string? AGREGA2_TART { get; set; }
        public string? AGREGA2_VER { get; set; }
        public string? AGREGA3_IST { get; set; }
        public string? AGREGA3_TART { get; set; }
        public string? AGREGA3_VER { get; set; }
        public string? AGREGA4_IST { get; set; }
        public string? AGREGA4_TART { get; set; }
        public string? AGREGA4_VER { get; set; }
        public string? AGREGA5_IST { get; set; }
        public string? AGREGA5_TART { get; set; }
        public string? AGREGA5_VER { get; set; }
        public string? AGREGA6_IST { get; set; }
        public string? AGREGA6_TART { get; set; }
        public string? AGREGA6_VER { get; set; }

        // Çimento alanları
        public string? CIMENTO1_IST { get; set; }
        public string? CIMENTO1_VER { get; set; }
        public string? CIMENTO2_IST { get; set; }
        public string? CIMENTO2_VER { get; set; }
        public string? CIMENTO3_IST { get; set; }
        public string? CIMENTO3_VER { get; set; }
        public string? CIMENTO4_IST { get; set; }
        public string? CIMENTO4_VER { get; set; }

        // Su alanları
        public string? SU1_IST { get; set; }
        public string? SU1_TART { get; set; }
        public string? SU1_VER { get; set; }
        public string? SU2_IST { get; set; }
        public string? SU2_TART { get; set; }
        public string? SU2_VER { get; set; }

        // Katkı alanları
        public string? KATKI1_IST { get; set; }
        public string? KATKI1_VER { get; set; }
        public string? KATKI2_IST { get; set; }
        public string? KATKI2_VER { get; set; }
        public string? KATKI3_IST { get; set; }
        public string? KATKI3_VER { get; set; }
        public string? KATKI4_IST { get; set; }
        public string? KATKI4_VER { get; set; }

        // IFS Order bilgileri
        public string? IFSORDER_NO { get; set; }

        // Diğer alanlar (kısaltılmış, tam liste ENOYAMODELLEME.sql'de)
        public string? URTSUCIM { get; set; }
        public string? URTSUCIMKAT { get; set; }
        public string? ILAVE_KT1 { get; set; }
        public string? ILAVE_KT2 { get; set; }
        public string? ILAVE_KT3 { get; set; }
        public string? ILAVE_KT4 { get; set; }

    }
}

