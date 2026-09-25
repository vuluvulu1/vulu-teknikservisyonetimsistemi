namespace vulu_yonetimsistemi_api.Models
{
    public class Musteri
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? Email { get; set; }
        public string? Adres { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        public ICollection<ServisKaydi> ServisKayitlari { get; set; } = new List<ServisKaydi>();
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}
