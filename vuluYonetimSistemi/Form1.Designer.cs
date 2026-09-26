namespace vuluYonetimSistemi
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlNavbar = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.btnMusteriler = new System.Windows.Forms.Button();
            this.btnTeknisyenler = new System.Windows.Forms.Button();
            this.btnServisKayitlari = new System.Windows.Forms.Button();
            this.btnRandevular = new System.Windows.Forms.Button();
            this.btnFaturalar = new System.Windows.Forms.Button();

            this.btnCikis = new System.Windows.Forms.Button();
            this.pnlIcerik = new System.Windows.Forms.Panel();
            this.pnlNavbar.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlNavbar
            //
            this.pnlNavbar.Controls.Add(this.btnCikis);
            this.pnlNavbar.Controls.Add(this.btnFaturalar);
            this.pnlNavbar.Controls.Add(this.btnRandevular);
            this.pnlNavbar.Controls.Add(this.btnServisKayitlari);
            this.pnlNavbar.Controls.Add(this.btnTeknisyenler);
            this.pnlNavbar.Controls.Add(this.btnMusteriler);
            this.pnlNavbar.Controls.Add(this.lblLogo);
            this.pnlNavbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNavbar.Location = new System.Drawing.Point(0, 0);
            this.pnlNavbar.Name = "pnlNavbar";
            this.pnlNavbar.Size = new System.Drawing.Size(1000, 55);
            this.pnlNavbar.TabIndex = 0;
            //
            // lblLogo
            //
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLogo.Location = new System.Drawing.Point(20, 16);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(58, 21);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "TSMS";
            //
            // btnMusteriler
            //
            this.btnMusteriler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMusteriler.Location = new System.Drawing.Point(140, 10);
            this.btnMusteriler.Name = "btnMusteriler";
            this.btnMusteriler.Size = new System.Drawing.Size(110, 35);
            this.btnMusteriler.TabIndex = 1;
            this.btnMusteriler.Text = "Müşteriler";
            this.btnMusteriler.Click += new System.EventHandler(this.btnMusteriler_Click);
            //
            // btnTeknisyenler
            //
            this.btnTeknisyenler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeknisyenler.Location = new System.Drawing.Point(260, 10);
            this.btnTeknisyenler.Name = "btnTeknisyenler";
            this.btnTeknisyenler.Size = new System.Drawing.Size(110, 35);
            this.btnTeknisyenler.TabIndex = 2;
            this.btnTeknisyenler.Text = "Teknisyenler";
            this.btnTeknisyenler.Click += new System.EventHandler(this.btnTeknisyenler_Click);
            //
            // btnServisKayitlari
            //
            this.btnServisKayitlari.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServisKayitlari.Location = new System.Drawing.Point(380, 10);
            this.btnServisKayitlari.Name = "btnServisKayitlari";
            this.btnServisKayitlari.Size = new System.Drawing.Size(130, 35);
            this.btnServisKayitlari.TabIndex = 3;
            this.btnServisKayitlari.Text = "Servis Kayıtları";
            this.btnServisKayitlari.Click += new System.EventHandler(this.btnServisKayitlari_Click);
            //
            // btnRandevular
            //
            this.btnRandevular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRandevular.Location = new System.Drawing.Point(520, 10);
            this.btnRandevular.Name = "btnRandevular";
            this.btnRandevular.Size = new System.Drawing.Size(110, 35);
            this.btnRandevular.TabIndex = 4;
            this.btnRandevular.Text = "Randevular";
            this.btnRandevular.Click += new System.EventHandler(this.btnRandevular_Click);
            //
            // btnFaturalar
            //
            this.btnFaturalar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFaturalar.Location = new System.Drawing.Point(640, 10);
            this.btnFaturalar.Name = "btnFaturalar";
            this.btnFaturalar.Size = new System.Drawing.Size(110, 35);
            this.btnFaturalar.TabIndex = 5;
            this.btnFaturalar.Text = "Faturalar";
            this.btnFaturalar.Click += new System.EventHandler(this.btnFaturalar_Click);
            //
            // btnCikis
            //
            this.btnCikis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCikis.Location = new System.Drawing.Point(880, 10);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(100, 35);
            this.btnCikis.TabIndex = 6;
            this.btnCikis.Text = "Çıkış Yap";
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            //
            // pnlIcerik
            //
            this.pnlIcerik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlIcerik.Location = new System.Drawing.Point(0, 55);
            this.pnlIcerik.Name = "pnlIcerik";
            this.pnlIcerik.Size = new System.Drawing.Size(1000, 545);
            this.pnlIcerik.TabIndex = 1;
            //
            // Form1
            //
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.pnlIcerik);
            this.Controls.Add(this.pnlNavbar);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TSMS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlNavbar.ResumeLayout(false);
            this.pnlNavbar.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlNavbar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Button btnMusteriler;
        private System.Windows.Forms.Button btnTeknisyenler;
        private System.Windows.Forms.Button btnServisKayitlari;
        private System.Windows.Forms.Button btnRandevular;
        private System.Windows.Forms.Button btnFaturalar;
        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.Panel pnlIcerik;
    }
}