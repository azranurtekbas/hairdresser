using System.ComponentModel.DataAnnotations;

namespace haircaredeneme.Models
{
    public class Musteri
    {
        public int musteriId { get; set; }
        public string kullanıcıAd { get; set; }
       
        [Phone]
        public string telefonNo { get; set; }

        [Required]
        [EmailAddress]
        public string mail { get; set; }

        public string sifre { get; set; }

        public List<Randevu> Randevular { get; set; } = new List<Randevu>();

    }
}
