namespace vulu_yonetimsistemi_api.Models
{
    public enum RandevuDurumu
    {
        Planlandi,
        TamamlandiRandevu,
        IptalEdildi
    }

    public class Randevu
    {
        public int Id { get; set; }

        public int MusteriId { get; set; }
        public Musteri Musteri { get; set; } = null!;

        public int? TeknisyenId { get; set; }
        public Teknisyen? Teknisyen { get; set; }

        public int? ServisKaydiId { get; set; }
        public ServisKaydi? ServisKaydi { get; set; }

        public DateTime RandevuTarihi { get; set; }
        public string? Aciklama { get; set; }
        public RandevuDurumu Durum { get; set; } = RandevuDurumu.Planlandi;
    }
}
