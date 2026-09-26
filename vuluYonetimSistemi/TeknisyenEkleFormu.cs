using System.Net.Http.Json;

namespace vuluYonetimSistemi
{
    public class TeknisyenEkleFormu : TemaliForm 
    {
        private readonly Teknisyen? _duzenlenecek;

        private TextBox txtAdSoyad = null!;
        private TextBox txtTelefon = null!;
        private TextBox txtUzmanlik = null!;
        private Button btnKaydet = null!;
        private ComboBox cmbKullanici = null!;

        record KullaniciSecenegi(int Value, string Text);
        
        public TeknisyenEkleFormu(Teknisyen? duzenlenecek = null) 
        {
            _duzenlenecek = duzenlenecek;

            this.Text = duzenlenecek is null ? "Yeni Teknisyen" : "Teknisyen Düzenle";
            this.Size = new Size(360, 380);
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

            var lbl3 = new Label { Text = "Uzmanlık", Location = new Point(20, 140), AutoSize = true };
            txtUzmanlik = new TextBox { Location = new Point(20, 160), Size = new Size(300, 25) };

            var lbl4 = new Label { Text = "Bağlı Kullanıcı Hesabı (opsiyonel)", Location = new Point(20, 195), AutoSize = true };
            cmbKullanici = new ComboBox
            {
                Location = new Point(20, 215),
                Size = new Size(300, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnKaydet = new Button
            {
                Text = duzenlenecek is null ? "Kaydet" : "Güncelle",
                Location = new Point(20, 260),
                Size = new Size(300, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.VurguRengi,
                ForeColor = Color.White
            };
            btnKaydet.Click += BtnKaydet_Click;

            this.Controls.AddRange(new Control[] { lbl1, txtAdSoyad, lbl2, txtTelefon, lbl3, txtUzmanlik, lbl4, cmbKullanici, btnKaydet });

            if (_duzenlenecek is not null)
            {
                txtAdSoyad.Text = _duzenlenecek.AdSoyad;
                txtTelefon.Text = _duzenlenecek.Telefon;
                txtUzmanlik.Text = _duzenlenecek.Uzmanlik;
            }

            _ = KullanicilariYukle();
        }

        private async Task KullanicilariYukle()
        {
            try
            {
                var kullanicilar = await ApiServisi.Client.GetFromJsonAsync<List<KullaniciOzetDto>>("api/Kullanici");

                cmbKullanici.Items.Clear();
                cmbKullanici.Items.Add(new KullaniciSatiri { Id = 0, Baslik = "— Yok —" });

                foreach (var k in kullanicilar ?? new List<KullaniciOzetDto>())
                    cmbKullanici.Items.Add(new KullaniciSatiri { Id = k.Id, Baslik = $"{k.KullaniciAdi} ({k.Rol})" });

                int hedefId = _duzenlenecek?.KullaniciId ?? 0;

                foreach (var item in cmbKullanici.Items)
                {
                    if (item is KullaniciSatiri satir && satir.Id == hedefId)
                    {
                        cmbKullanici.SelectedItem = satir;
                        return;
                    }
                }

                cmbKullanici.SelectedIndex = 0;
            }
            catch
            {
                // Kullanıcı listesi yüklenemezse sessizce boş bırak.
            }
        }

       
        private class KullaniciSatiri
        {
            public int Id;
            public string Baslik = "";
            public override string ToString() => Baslik;
        }

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

            var teknisyenVerisi = new
            {
                AdSoyad = txtAdSoyad.Text.Trim(),
                Telefon = txtTelefon.Text.Trim(),
                Uzmanlik = txtUzmanlik.Text.Trim(),
                KullaniciId = cmbKullanici.SelectedItem is KullaniciSatiri secili && secili.Id != 0 ? secili.Id : (int?)null
            };

            HttpResponseMessage yanit;

            if (_duzenlenecek is null)
                yanit = await ApiServisi.Client.PostAsJsonAsync("api/Teknisyen", teknisyenVerisi);
            else
                yanit = await ApiServisi.Client.PutAsJsonAsync($"api/Teknisyen/{_duzenlenecek.Id}", teknisyenVerisi);

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
                    var mesaj = hataJson.RootElement.GetProperty("hata").GetString();
                    MessageBox.Show(mesaj ?? "İşlem başarısız oldu.");
                }
                catch
                {
                    MessageBox.Show("İşlem başarısız oldu.");
                }
            }
        }
    }
}