namespace vulu_yonetimsistemi_api.Models
{
    public enum KullaniciRol
    {
        Admin,
        Teknisyen
    }

    public class Kullanici
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; } = string.Empty;
        public string SifreHash { get; set; } = string.Empty;
        public KullaniciRol Rol { get; set; } = KullaniciRol.Teknisyen;
        public bool AktifMi { get; set; } = true;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
    }
}
