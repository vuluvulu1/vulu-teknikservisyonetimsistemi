namespace vulu_yonetimsistemi_api.Models
{
    public class Fatura
    {
        public int Id { get; set; }

        public int ServisKaydiId { get; set; }
        public ServisKaydi ServisKaydi { get; set; } = null!;

        public decimal Tutar { get; set; }
        public bool OdendiMi { get; set; } = false;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
        public DateTime? OdemeTarihi { get; set; }
    }
}
