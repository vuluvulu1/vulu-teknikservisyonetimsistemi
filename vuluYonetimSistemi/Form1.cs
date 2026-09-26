namespace vuluYonetimSistemi
{
    public partial class Form1 : TemaliForm
    {
        public Form1()
        {
            InitializeComponent();
            pnlNavbar.BackColor = Tema.YuzeyRengi;

            if (ApiServisi.Rol == "Admin")
            {
                var btnKullanicilar = new Button
                {
                    Text = "Kullanıcılar",
                    Location = new Point(770, 10),
                    Size = new Size(110, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Tema.VurguRengi,
                    ForeColor = Color.White
                };
                btnKullanicilar.Click += (s, e) => IcerigiDegistir(new KullaniciListesiControl());
                pnlNavbar.Controls.Add(btnKullanicilar);
            }
        }

        private void IcerigiDegistir(UserControl yeniIcerik)
        {
            pnlIcerik.Controls.Clear();
            yeniIcerik.Dock = DockStyle.Fill;
            pnlIcerik.Controls.Add(yeniIcerik);
        }

        private void btnMusteriler_Click(object sender, EventArgs e)
        {
            IcerigiDegistir(new MusteriListesiControl());
        }

        private void btnTeknisyenler_Click(object sender, EventArgs e)
        {
            IcerigiDegistir(new TeknisyenListesiControl());
        }

        private void btnServisKayitlari_Click(object sender, EventArgs e)
        {
        }

        private void btnRandevular_Click(object sender, EventArgs e)
        {
        }

        private void btnFaturalar_Click(object sender, EventArgs e)
        {
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            ApiServisi.Token = null;
            var girisFormu = new GirisFormu();
            girisFormu.Show();
            this.Hide();
        }
    }
}