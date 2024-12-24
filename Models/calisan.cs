using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace haircare.Models
{

	public class Calisan
	{
		[Key] // Birincil anahtar olduğunu belirtir
		public int Id { get; set; }
		public string CalısanAd { get; set; }
		public string CalısanSoyad { get; set; }
		public string UygunlukSaati { get; set; }
		public int? UzmanlikAlaniId { get; set; } // Uzmanlık (Nullable Foreign Key)
    	public Islem UzmanlikAlani { get; set; }
		// Navigation property

		public List<Randevu> Randevular { get; set; } = new List<Randevu>();



	}
}