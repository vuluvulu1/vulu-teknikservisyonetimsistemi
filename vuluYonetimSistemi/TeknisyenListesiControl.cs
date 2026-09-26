using System.Net.Http.Json;
using static vuluYonetimSistemi.TeknisyenEkleFormu;
using System.Linq;

namespace vuluYonetimSistemi
{
    public class Teknisyen
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = "";
        public string? Telefon { get; set; }
        public string? Uzmanlik { get; set; }
        public int? KullaniciId { get; set; }

        
    }
    public class TeknisyenListesiControl : UserControl
    {
        private DataGridView dgvTeknisyenler = null!;
        private Button btnYenile = null!;
        private Button btnYeni = null!;
        private Button btnDuzenle = null!;
        private Button btnSil = null!;
        private Label lblBaslik = null!;
        private List<Teknisyen> _teknisyenler = new();

        public TeknisyenListesiControl()
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
                BackColor = Tema.ArkaPlan
            };

            lblBaslik = new Label
            {
                Text = "Teknisyenler",
                Font = Tema.BaslikFontu,
                ForeColor = Tema.MetinRengi,
                AutoSize = true,
                Location = new Point(20, 15)
            };

            btnYeni = new Button
            {
                Text = "Yeni Teknisyen",
                Location = new Point(20, 55),
                Size = new Size(130, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.VurguRengi,
                ForeColor = Color.White
            };
            btnYeni.Click += BtnYeni_Click;

            btnSil = new Button
            {
                Text = "Seçileni Sil",
                Location = new Point(160, 55),
                Size = new Size(120, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.HataRengi,
                ForeColor = Color.White
            };
            btnSil.Click += BtnSil_Click;

            btnDuzenle = new Button
            {
                Text = "Düzenle",
                Location = new Point(290, 55),
                Size = new Size(100, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.YuzeyRengi,
                ForeColor = Tema.MetinRengi
            };
            btnDuzenle.Click += BtnDuzenle_Click;

            btnYenile = new Button
            {
                Text = "Yenile",
                Location = new Point(400, 55),
                Size = new Size(100, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Tema.YuzeyRengi,
                ForeColor = Tema.MetinRengi
            };
            btnYenile.Click += async (s, e) => await ListeyiYukle();

            ustPanel.Controls.Add(lblBaslik);
            ustPanel.Controls.Add(btnYeni);
            ustPanel.Controls.Add(btnSil);
            ustPanel.Controls.Add(btnDuzenle);
            ustPanel.Controls.Add(btnYenile);

            dgvTeknisyenler = new DataGridView
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
            dgvTeknisyenler.DefaultCellStyle.BackColor = Tema.YuzeyRengi;
            dgvTeknisyenler.DefaultCellStyle.ForeColor = Tema.MetinRengi;
            dgvTeknisyenler.DefaultCellStyle.SelectionBackColor = Tema.VurguRengi;
            dgvTeknisyenler.ColumnHeadersDefaultCellStyle.BackColor = Tema.ArkaPlan;
            dgvTeknisyenler.ColumnHeadersDefaultCellStyle.ForeColor = Tema.MetinRengi;
            dgvTeknisyenler.EnableHeadersVisualStyles = false;
            dgvTeknisyenler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTeknisyenler.ColumnHeadersHeight = 35;

            var gridKapsayici = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 20, 20)
            };
            gridKapsayici.Controls.Add(dgvTeknisyenler);

            this.Controls.Add(gridKapsayici);
            this.Controls.Add(ustPanel);
        }

        private async Task ListeyiYukle()
        {
            try
            {
                var teknisyenler = await ApiServisi.Client.GetFromJsonAsync<List<Teknisyen>>("api/Teknisyen");
                _teknisyenler = teknisyenler ?? new List<Teknisyen>();
                var kullanicilar = await ApiServisi.Client.GetFromJsonAsync<List<KullaniciOzetDto>>("api/Kullanici");
                var kullaniciSozlugu = (kullanicilar ?? new List<KullaniciOzetDto>()).ToDictionary(k => k.Id, k => k.KullaniciAdi);

                var goruntulenecekVeri = (teknisyenler ?? new List<Teknisyen>()).Select(t => new
                {
                    t.Id,
                    t.AdSoyad,
                    t.Telefon,
                    t.Uzmanlik,
                    KullaniciHesabi = t.KullaniciId.HasValue && kullaniciSozlugu.ContainsKey(t.KullaniciId.Value)
                        ? kullaniciSozlugu[t.KullaniciId.Value]
                        : "-"
                }).ToList();

                dgvTeknisyenler.DataSource = null;
                dgvTeknisyenler.DataSource = goruntulenecekVeri;

                if (dgvTeknisyenler.Columns["Id"] != null) dgvTeknisyenler.Columns["Id"]!.HeaderText = "ID";
                if (dgvTeknisyenler.Columns["AdSoyad"] != null) dgvTeknisyenler.Columns["AdSoyad"]!.HeaderText = "Ad Soyad";
                if (dgvTeknisyenler.Columns["Telefon"] != null) dgvTeknisyenler.Columns["Telefon"]!.HeaderText = "Telefon";
                if (dgvTeknisyenler.Columns["Uzmanlik"] != null) dgvTeknisyenler.Columns["Uzmanlik"]!.HeaderText = "Uzmanlık";
                if (dgvTeknisyenler.Columns["KullaniciHesabi"] != null) dgvTeknisyenler.Columns["KullaniciHesabi"]!.HeaderText = "Kullanıcı Hesabı";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Teknisyenler yüklenemedi: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnYeni_Click(object? sender, EventArgs e)
        {
            using var form = new TeknisyenEkleFormu();
            if (form.ShowDialog() == DialogResult.OK)
                await ListeyiYukle();
        }

        private async void BtnDuzenle_Click(object? sender, EventArgs e)
        {
            if (dgvTeknisyenler.CurrentRow?.Cells["Id"]?.Value is not int secilenId)
            {
                MessageBox.Show("Düzenlemek için bir teknisyen seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var secili = _teknisyenler.FirstOrDefault(t => t.Id == secilenId);
            if (secili is null) return;

            using var form = new TeknisyenEkleFormu(secili);
            if (form.ShowDialog() == DialogResult.OK)
                await ListeyiYukle();
        }

        private async void BtnSil_Click(object? sender, EventArgs e)
        {
            if (dgvTeknisyenler.CurrentRow?.Cells["Id"]?.Value is not int secilenId)
            {
                MessageBox.Show("Silmek için bir teknisyen seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var secili = _teknisyenler.FirstOrDefault(t => t.Id == secilenId);
            if (secili is null) return;

            var sonuc = MessageBox.Show($"'{secili.AdSoyad}' silinsin mi?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc != DialogResult.Yes) return;

            var yanit = await ApiServisi.Client.DeleteAsync($"api/Teknisyen/{secili.Id}");
            if (yanit.IsSuccessStatusCode)
                await ListeyiYukle();
            else
                MessageBox.Show("Silme işlemi başarısız oldu.", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}