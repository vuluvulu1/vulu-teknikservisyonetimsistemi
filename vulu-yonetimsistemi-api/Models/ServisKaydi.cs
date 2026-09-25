namespace vulu_yonetimsistemi_api.Models
{
    public enum ServisDurumu
    {
        Beklemede,
        Devam,
        Tamamlandi,
        IptalEdildi
    }

    public class ServisKaydi
    {
        public int Id { get; set; }

        public int MusteriId { get; set; }
        public Musteri Musteri { get; set; } = null!;

        public int? TeknisyenId { get; set; }
        public Teknisyen? Teknisyen { get; set; }

        public string CihazBilgisi { get; set; } = string.Empty;
        public string Sorun { get; set; } = string.Empty;
        public ServisDurumu Durum { get; set; } = ServisDurumu.Beklemede;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
        public DateTime? TamamlanmaTarihi { get; set; }

        public Fatura? Fatura { get; set; }
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}
