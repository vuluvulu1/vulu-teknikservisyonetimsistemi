namespace vuluYonetimSistemi
{
    public partial class GirisFormu : TemaliForm
    {
        public GirisFormu()
        {
            InitializeComponent();
        }

        private async void btnGiris_Click(object sender, EventArgs e)
        {
            lblHata.Text = "";
            btnGiris.Enabled = false;

            var (basarili, mesaj) = await ApiServisi.GirisYap(txtKullaniciAdi.Text, txtSifre.Text);

            if (basarili)
            {
                var anaForm = new Form1(); // ana formunun gerçek adı neyse onu yaz
                anaForm.Show();
                this.Hide();
            }
            else
            {
                lblHata.Text = mesaj;
            }

            btnGiris.Enabled = true;
        }
    }
}