namespace vulu_yonetimsistemi_api.Models
{
    public class Teknisyen
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? Uzmanlik { get; set; }

        public int? KullaniciId { get; set; }
        public Kullanici? Kullanici { get; set; }

        public ICollection<ServisKaydi> ServisKayitlari { get; set; } = new List<ServisKaydi>();
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}
