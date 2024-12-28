using haircaredeneme.Models;
using System.ComponentModel.DataAnnotations;

namespace haircaredeneme.Models
{

    public class Islem
    {
        [Key] public int IslemsId { get; set; }
        public string IslemAd { get; set; }

        public string IslemTur { get; set; }

        public string IslemSuresi { get; set; }

        public decimal Fiyat { get; set; }
        public int CalısanId { get; set; }
        public virtual Calisan CalisanNavigation { get; set; }

        public List<Randevu>? Randevular { get; set; } = new List<Randevu>();





    }

}