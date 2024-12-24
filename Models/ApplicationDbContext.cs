using Microsoft.EntityFrameworkCore;

namespace haircare.Models
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		// DbSet tanımları
		public DbSet<Islem> Islemler { get; set; }
		public DbSet<Calisan> Calisanlar { get; set; }
		public DbSet<Randevu> Randevular { get; set; }
		public DbSet<Musteri> Musteriler { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Calisan - Islem ilişkisi
			modelBuilder.Entity<Calisan>()
				.HasOne(c => c.UzmanlikAlani)
				.WithMany()
				.HasForeignKey(c => c.UzmanlikAlaniId)
				.OnDelete(DeleteBehavior.Restrict);

			base.OnModelCreating(modelBuilder);
		}

	}
}