using haircaredeneme.Models;
using Microsoft.EntityFrameworkCore;
namespace haircaredeneme.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Islem> Islemler { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Islem>()
            .HasOne(i => i.CalisanNavigation)
            .WithMany(c => c.Islemler)
            .HasForeignKey(i => i.CalısanId);

            base.OnModelCreating(modelBuilder);
 
         
        }

       




    }







}
