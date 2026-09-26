using System.Net.Http.Json;

namespace vuluYonetimSistemi
{
    public record KullaniciOzetDto(int Id, string KullaniciAdi, string Rol, bool AktifMi);

    public class KullaniciListesiControl : UserControl
    {
        private DataGridView dgv = null!;
        private List<KullaniciOzetDto> _kullanicilar = new();

        public KullaniciListesiControl()
        {
            OlusturUI();
            _ = ListeyiYukle();
        }

        private void OlusturUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Tema.ArkaPlan;

            var ustPanel = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Tema.ArkaPlan };

            var lblBaslik = new Label
            {
                Text = "Kullanıcılar",
                Font = Tema.BaslikFontu,
                ForeColor = Tema.MetinRengi,
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var btnYeni = new Button
            {
                Text = "Yeni Kullanıcı",
                Location = new Point(20, 55),
                Size = new Size(130, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.VurguRengi,
                ForeColor = Color.White
            };
            btnYeni.Click += async (s, e) =>
            {
                using var form = new KullaniciEkleFormu();
                if (form.ShowDialog() == DialogResult.OK) await ListeyiYukle();
            };

            var btnDuzenle = new Button
            {
                Text = "Düzenle",
                Location = new Point(160, 55),
                Size = new Size(100, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.YuzeyRengi,
                ForeColor = Tema.MetinRengi
            };
            btnDuzenle.Click += async (s, e) =>
            {
                if (dgv.CurrentRow?.Cells["Id"]?.Value is not int id) return;
                var secili = _kullanicilar.FirstOrDefault(k => k.Id == id);
                if (secili is null) return;

                using var form = new KullaniciEkleFormu(secili);
                if (form.ShowDialog() == DialogResult.OK) await ListeyiYukle();
            };

            var btnSil = new Button
            {
                Text = "Seçileni Sil",
                Location = new Point(270, 55),
                Size = new Size(110, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.HataRengi,
                ForeColor = Color.White
            };
            btnSil.Click += async (s, e) =>
            {
                if (dgv.CurrentRow?.Cells["Id"]?.Value is not int id) return;
                var secili = _kullanicilar.FirstOrDefault(k => k.Id == id);
                if (secili is null) return;

                if (secili.KullaniciAdi == ApiServisi.KullaniciAdi)
                {
                    MessageBox.Show("Kendi hesabınızı silemezsiniz.");
                    return;
                }

                if (MessageBox.Show($"'{secili.KullaniciAdi}' silinsin mi?", "Onay",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                var yanit = await ApiServisi.Client.DeleteAsync($"api/Kullanici/{secili.Id}");
                if (yanit.IsSuccessStatusCode) await ListeyiYukle();
                else MessageBox.Show("Silme işlemi başarısız oldu (teknisyene bağlı olabilir).");
            };

            var btnYenile = new Button
            {
                Text = "Yenile",
                Location = new Point(390, 55),
                Size = new Size(100, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.YuzeyRengi,
                ForeColor = Tema.MetinRengi
            };
            btnYenile.Click += async (s, e) => await ListeyiYukle();

            ustPanel.Controls.Add(lblBaslik);
            ustPanel.Controls.Add(btnYeni);
            ustPanel.Controls.Add(btnDuzenle);
            ustPanel.Controls.Add(btnSil);
            ustPanel.Controls.Add(btnYenile);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Tema.ArkaPlan,
                ForeColor = Tema.MetinRengi,
                GridColor = Tema.YuzeyRengi,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 35
            };
            dgv.DefaultCellStyle.BackColor = Tema.YuzeyRengi;
            dgv.DefaultCellStyle.ForeColor = Tema.MetinRengi;
            dgv.DefaultCellStyle.SelectionBackColor = Tema.VurguRengi;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Tema.ArkaPlan;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Tema.MetinRengi;

            var gridKapsayici = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 0, 20, 20) };
            gridKapsayici.Controls.Add(dgv);

            this.Controls.Add(gridKapsayici);
            this.Controls.Add(ustPanel);
        }

        private async Task ListeyiYukle()
        {
            try
            {
                var kullanicilar = await ApiServisi.Client.GetFromJsonAsync<List<KullaniciOzetDto>>("api/Kullanici");
                _kullanicilar = kullanicilar ?? new List<KullaniciOzetDto>();

                dgv.DataSource = null;
                dgv.DataSource = _kullanicilar;

                if (dgv.Columns["Id"] != null) dgv.Columns["Id"]!.HeaderText = "ID";
                if (dgv.Columns["KullaniciAdi"] != null) dgv.Columns["KullaniciAdi"]!.HeaderText = "Kullanıcı Adı";
                if (dgv.Columns["Rol"] != null) dgv.Columns["Rol"]!.HeaderText = "Rol";
                if (dgv.Columns["AktifMi"] != null) dgv.Columns["AktifMi"]!.HeaderText = "Aktif";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullanıcılar yüklenemedi: " + ex.Message);
            }
        }
    }
}