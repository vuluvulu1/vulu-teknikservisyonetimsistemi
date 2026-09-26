using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace vuluYonetimSistemi
{
    public class MusteriEkleFormu : TemaliForm
    {
        private readonly Musteri? _duzenlenecek;

        private TextBox txtAdSoyad = null!;
        private TextBox txtTelefon = null!;
        private TextBox txtEmail = null!;
        private TextBox txtAdres = null!;
        private Button btnKaydet = null!;

        public MusteriEkleFormu(Musteri? duzenlenecek = null)
        {
            _duzenlenecek = duzenlenecek;

            this.Text = duzenlenecek is null ? "Yeni Müşteri" : "Müşteri Düzenle";
            this.Size = new Size(360, 460);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lbl1 = new Label { Text = "Ad Soyad", Location = new Point(20, 20), AutoSize = true };
            txtAdSoyad = new TextBox { Location = new Point(20, 40), Size = new Size(300, 25) };
            txtAdSoyad.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;
                if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                    e.Handled = true;
            };

            var lbl2 = new Label { Text = "Telefon", Location = new Point(20, 80), AutoSize = true };
            txtTelefon = new TextBox { Location = new Point(20, 100), Size = new Size(300, 25), MaxLength = 11 };
            txtTelefon.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;
                if (!char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            var lbl3 = new Label { Text = "E-posta", Location = new Point(20, 140), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(20, 160), Size = new Size(300, 25) };

            var lbl4 = new Label { Text = "Adres", Location = new Point(20, 200), AutoSize = true };
            txtAdres = new TextBox
            {
                Location = new Point(20, 220),
                Size = new Size(300, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                AcceptsReturn = true
            };


            btnKaydet = new Button
            {
                Text = duzenlenecek is null ? "Kaydet" : "Güncelle",
                Location = new Point(20, 340),
                Size = new Size(300, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.VurguRengi,
                ForeColor = Color.White
            };
            btnKaydet.Click += BtnKaydet_Click;

            this.Controls.AddRange(new Control[] { lbl1, txtAdSoyad, lbl2, txtTelefon, lbl3, txtEmail, lbl4, txtAdres, btnKaydet });

            if (_duzenlenecek is not null)
            {
                txtAdSoyad.Text = _duzenlenecek.AdSoyad;
                txtTelefon.Text = _duzenlenecek.Telefon;
                txtEmail.Text = _duzenlenecek.Email;
                txtAdres.Text = _duzenlenecek.Adres;
            }
        }

        private static readonly Regex EmailDeseni = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        private async void BtnKaydet_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAdSoyad.Text))
            {
                MessageBox.Show("Ad Soyad boş olamaz.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTelefon.Text))
            {
                MessageBox.Show("Telefon boş olamaz.");
                return;
            }

            if (txtTelefon.Text.Trim().Length != 11)
            {
                MessageBox.Show("Telefon numarası 11 haneli olmalı (örn: 05551234567).");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAdres.Text))
            {
                MessageBox.Show("Adres boş olamaz.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !EmailDeseni.IsMatch(txtEmail.Text))
            {
                MessageBox.Show("Geçerli bir e-posta adresi girin.");
                return;
            }

            var musteriVerisi = new
            {
                AdSoyad = txtAdSoyad.Text.Trim(),
                Telefon = txtTelefon.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Adres = txtAdres.Text.Trim()
            };

            HttpResponseMessage yanit;

            if (_duzenlenecek is null)
            {
                yanit = await ApiServisi.Client.PostAsJsonAsync("api/Musteri", musteriVerisi);
            }
            else
            {
                yanit = await ApiServisi.Client.PutAsJsonAsync($"api/Musteri/{_duzenlenecek.Id}", musteriVerisi);
            }

            if (yanit.IsSuccessStatusCode)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("İşlem başarısız oldu.");
            }
        }
    }
}