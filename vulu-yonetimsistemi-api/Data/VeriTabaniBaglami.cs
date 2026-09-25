using Microsoft.EntityFrameworkCore;
using vulu_yonetimsistemi_api.Models;

namespace vulu_yonetimsistemi_api.Data
{
    public class VeriTabaniBaglami : DbContext
    {
        public VeriTabaniBaglami(DbContextOptions<VeriTabaniBaglami> options) : base(options) { }

        public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();
        public DbSet<Musteri> Musteriler => Set<Musteri>();
        public DbSet<Teknisyen> Teknisyenler => Set<Teknisyen>();
        public DbSet<ServisKaydi> ServisKayitlari => Set<ServisKaydi>();
        public DbSet<Randevu> Randevular => Set<Randevu>();
        public DbSet<Fatura> Faturalar => Set<Fatura>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kullanici>()
                .HasIndex(k => k.KullaniciAdi)
                .IsUnique();

            modelBuilder.Entity<Fatura>()
                .Property(f => f.Tutar)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<ServisKaydi>()
                .HasOne(s => s.Fatura)
                .WithOne(f => f.ServisKaydi)
                .HasForeignKey<Fatura>(f => f.ServisKaydiId);
        }
    }
}
