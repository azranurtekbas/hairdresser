using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using haircaredeneme.Models;
using System.Text.Json.Serialization;

namespace haircaredeneme.Models
{

    public class Calisan
    {
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("CalisanAd")] // JSON içinde doğru isimlendirme
        public string CalısanAd { get; set; }

        [JsonPropertyName("CalisanSoyad")]
        public string CalısanSoyad { get; set; }

        public string UzmanlikAlani { get; set; }
        public TimeSpan UygunlukSaati { get; set; }

        public ICollection<Islem> Islemler { get; set; }
        public List<Randevu> Randevular { get; set; } = new List<Randevu>();

    }
}

