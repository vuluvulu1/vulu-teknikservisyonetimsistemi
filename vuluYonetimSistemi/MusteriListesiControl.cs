using System.Net.Http.Json;

namespace vuluYonetimSistemi
{
    public class Musteri
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = "";
        public string? Telefon { get; set; }
        public string? Email { get; set; }
        public string? Adres { get; set; }
    }

    public class MusteriListesiControl : UserControl
    {
        private DataGridView dgvMusteriler = null!;
        private Button btnYenile = null!;
        private Button btnYeniMusteri = null!;
        private Button btnSil = null!;
        private Label lblBaslik = null!;
        private Button btnDuzenle = null!;

        public MusteriListesiControl()
        {
            OlusturUI();
            _ = ListeyiYukle();
        }

        private void OlusturUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Tema.ArkaPlan;

            var ustPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Tema.ArkaPlan,
                Padding = new Padding(20, 15, 20, 0)
            };

            lblBaslik = new Label
            {
                Text = "Müşteriler",
                Font = Tema.BaslikFontu,
                ForeColor = Tema.MetinRengi,
                AutoSize = true,
                Location = new Point(20, 0)
            };

            btnYeniMusteri = new Button
            {
                Text = "Yeni Müşteri",
                Location = new Point(20, 45),
                Size = new Size(120, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.VurguRengi,
                ForeColor = Color.White
            };
            btnYeniMusteri.Click += BtnYeniMusteri_Click;

            btnSil = new Button
            {
                Text = "Seçileni Sil",
                Location = new Point(150, 45),
                Size = new Size(120, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.HataRengi,
                ForeColor = Color.White
            };
            btnSil.Click += BtnSil_Click;

            btnDuzenle = new Button
            {
                Text = "Düzenle",
                Location = new Point(280, 45),
                Size = new Size(100, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.YuzeyRengi,
                ForeColor = Tema.MetinRengi
            };
            btnDuzenle.Click += BtnDuzenle_Click;

            btnYenile = new Button
            {
                Text = "Yenile",
                Location = new Point(390, 45),
                Size = new Size(100, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.YuzeyRengi,
                ForeColor = Tema.MetinRengi
            };
            btnYenile.Click += async (s, e) => await ListeyiYukle();

            ustPanel.Controls.Add(lblBaslik);
            ustPanel.Controls.Add(btnYeniMusteri);
            ustPanel.Controls.Add(btnSil);
            ustPanel.Controls.Add(btnYenile);
            ustPanel.Controls.Add(btnDuzenle);

            dgvMusteriler = new DataGridView
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
                RowHeadersVisible = false
            };
            dgvMusteriler.DefaultCellStyle.BackColor = Tema.YuzeyRengi;
            dgvMusteriler.DefaultCellStyle.ForeColor = Tema.MetinRengi;
            dgvMusteriler.DefaultCellStyle.SelectionBackColor = Tema.VurguRengi;
            dgvMusteriler.ColumnHeadersDefaultCellStyle.BackColor = Tema.ArkaPlan;
            dgvMusteriler.ColumnHeadersDefaultCellStyle.ForeColor = Tema.MetinRengi;
            dgvMusteriler.EnableHeadersVisualStyles = false;
            dgvMusteriler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvMusteriler.ColumnHeadersHeight = 35;

            var gridKapsayici = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 20, 20)
            };
            gridKapsayici.Controls.Add(dgvMusteriler);

            this.Controls.Add(gridKapsayici);
            this.Controls.Add(ustPanel);
        }

        private async Task ListeyiYukle()
        {
            try
            {
                var musteriler = await ApiServisi.Client.GetFromJsonAsync<List<Musteri>>("api/Musteri");

                dgvMusteriler.DataSource = null;
                dgvMusteriler.DataSource = musteriler;

                if (dgvMusteriler.Columns["Id"] != null)
                    dgvMusteriler.Columns["Id"]!.HeaderText = "ID";
                if (dgvMusteriler.Columns["AdSoyad"] != null)
                    dgvMusteriler.Columns["AdSoyad"]!.HeaderText = "Ad Soyad";
                if (dgvMusteriler.Columns["Telefon"] != null)
                    dgvMusteriler.Columns["Telefon"]!.HeaderText = "Telefon";
                if (dgvMusteriler.Columns["Email"] != null)
                    dgvMusteriler.Columns["Email"]!.HeaderText = "E-posta";
                if (dgvMusteriler.Columns["Adres"] != null)
                    dgvMusteriler.Columns["Adres"]!.HeaderText = "Adres";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteriler yüklenemedi: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnYeniMusteri_Click(object? sender, EventArgs e)
        {
            using var form = new MusteriEkleFormu();
            if (form.ShowDialog() == DialogResult.OK)
            {
                await ListeyiYukle();
            }
        }

        private async void BtnSil_Click(object? sender, EventArgs e)
        {
            if (dgvMusteriler.CurrentRow?.DataBoundItem is not Musteri secili)
            {
                MessageBox.Show("Silmek için bir müşteri seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sonuc = MessageBox.Show($"'{secili.AdSoyad}' silinsin mi?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc != DialogResult.Yes) return;

            var yanit = await ApiServisi.Client.DeleteAsync($"api/Musteri/{secili.Id}");
            if (yanit.IsSuccessStatusCode)
                await ListeyiYukle();
            else
                MessageBox.Show("Silme işlemi başarısız oldu.", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtnDuzenle_Click(object? sender, EventArgs e)
        {
            if (dgvMusteriler.CurrentRow?.DataBoundItem is not Musteri secili)
            {
                MessageBox.Show("Düzenlemek için bir müşteri seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new MusteriEkleFormu(secili);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = ListeyiYukle();
            }
        }
    }
}