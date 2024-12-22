using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace haircare.Models
{
    
        public class Calisan
        {
            [Key] // Birincil anahtar olduğunu belirtir
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Otomatik artırılır
             public int Id { get; set; }
            public string CalısanAd { get; set; }
            public string CalısanSoyad { get; set; }
            public string UzmanlikAlani { get; set; }

            public string UygunlukSaati { get; set; }

            public List<Randevu> Randevular { get; set; } = new List<Randevu>();


    }
}

