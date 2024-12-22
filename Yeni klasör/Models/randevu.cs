namespace haircare.Models
{
    public class Randevu
    {
        public int randevuId { get; set; }
      
        public DateTime Tarih { get; set; }
        public int CalisanId { get; set; }
        public int musteriId { get; set; }
        public int islemId { get; set; }

        public Calisan Calisan { get; set; }
        public Musteri musteri { get; set; }
        public Islem Islem { get; set; }

        // Yeni alan: Durum
        public string Durum { get; set; } = "Beklemede"; // Default değer
    }
    
    
}

