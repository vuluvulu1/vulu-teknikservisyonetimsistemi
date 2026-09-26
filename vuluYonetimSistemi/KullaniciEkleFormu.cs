using System.Net.Http.Json;

namespace vuluYonetimSistemi
{
    public class KullaniciEkleFormu : TemaliForm
    {
        private readonly KullaniciOzetDto? _duzenlenecek;

        private TextBox txtKullaniciAdi = null!;
        private TextBox txtSifre = null!;
        private ComboBox cmbRol = null!;
        private CheckBox chkAktif = null!;
        private Button btnKaydet = null!;

        public KullaniciEkleFormu(KullaniciOzetDto? duzenlenecek = null)
        {
            _duzenlenecek = duzenlenecek;

            this.Text = duzenlenecek is null ? "Yeni Kullanıcı" : "Kullanıcı Düzenle";
            this.Size = new Size(360, 380);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lbl1 = new Label { Text = "Kullanıcı Adı", Location = new Point(20, 20), AutoSize = true };
            txtKullaniciAdi = new TextBox { Location = new Point(20, 40), Size = new Size(300, 25) };

            var lbl2 = new Label
            {
                Text = duzenlenecek is null ? "Şifre" : "Yeni Şifre (boş bırakılırsa değişmez)",
                Location = new Point(20, 80),
                AutoSize = true
            };
            txtSifre = new TextBox { Location = new Point(20, 100), Size = new Size(300, 25), PasswordChar = '*' };

            var lbl3 = new Label { Text = "Rol", Location = new Point(20, 140), AutoSize = true };
            cmbRol = new ComboBox { Location = new Point(20, 160), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRol.Items.Add("Admin");
            cmbRol.Items.Add("Teknisyen");
            cmbRol.SelectedIndex = 1;

            chkAktif = new CheckBox
            {
                Text = "Aktif",
                Location = new Point(20, 200),
                AutoSize = true,
                Checked = true,
                ForeColor = Tema.MetinRengi
            };

            btnKaydet = new Button
            {
                Text = duzenlenecek is null ? "Kaydet" : "Güncelle",
                Location = new Point(20, 250),
                Size = new Size(300, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.VurguRengi,
                ForeColor = Color.White
            };
            btnKaydet.Click += BtnKaydet_Click;

            this.Controls.AddRange(new Control[] { lbl1, txtKullaniciAdi, lbl2, txtSifre, lbl3, cmbRol, chkAktif, btnKaydet });

            if (_duzenlenecek is not null)
            {
                txtKullaniciAdi.Text = _duzenlenecek.KullaniciAdi;
                cmbRol.SelectedItem = _duzenlenecek.Rol;
                chkAktif.Checked = _duzenlenecek.AktifMi;
            }
        }

        private async void BtnKaydet_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKullaniciAdi.Text))
            {
                MessageBox.Show("Kullanıcı adı boş olamaz.");
                return;
            }

            if (_duzenlenecek is null && string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                MessageBox.Show("Şifre boş olamaz.");
                return;
            }

            HttpResponseMessage yanit;

            if (_duzenlenecek is null)
            {
                var yeniKullanici = new
                {
                    KullaniciAdi = txtKullaniciAdi.Text.Trim(),
                    Sifre = txtSifre.Text,
                    Rol = cmbRol.SelectedItem!.ToString()
                };
                yanit = await ApiServisi.Client.PostAsJsonAsync("api/Kullanici/olustur", yeniKullanici);
            }
            else
            {
                var guncelKullanici = new
                {
                    KullaniciAdi = txtKullaniciAdi.Text.Trim(),
                    Rol = cmbRol.SelectedItem!.ToString(),
                    AktifMi = chkAktif.Checked,
                    YeniSifre = string.IsNullOrWhiteSpace(txtSifre.Text) ? null : txtSifre.Text
                };
                yanit = await ApiServisi.Client.PutAsJsonAsync($"api/Kullanici/{_duzenlenecek.Id}", guncelKullanici);
            }

            if (yanit.IsSuccessStatusCode)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                var hataMetni = await yanit.Content.ReadAsStringAsync();
                try
                {
                    var hataJson = System.Text.Json.JsonDocument.Parse(hataMetni);
                    MessageBox.Show(hataJson.RootElement.GetProperty("hata").GetString() ?? "İşlem başarısız oldu.");
                }
                catch
                {
                    MessageBox.Show("İşlem başarısız oldu.");
                }
            }
        }
    }
}