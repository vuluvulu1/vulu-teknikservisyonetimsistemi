namespace vuluYonetimSistemi
{
    public static class Tema
    {
        public static Color ArkaPlan { get; set; } = Color.FromArgb(24, 24, 27);
        public static Color YuzeyRengi { get; set; } = Color.FromArgb(39, 39, 42);
        public static Color MetinRengi { get; set; } = Color.WhiteSmoke;
        public static Color VurguRengi { get; set; } = Color.FromArgb(99, 102, 241); // buton, aktif eleman
        public static Color HataRengi { get; set; } = Color.FromArgb(239, 68, 68);
        public static Color BasariRengi { get; set; } = Color.FromArgb(34, 197, 94);

        public static Font BaslikFontu { get; set; } = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static Font NormalFont { get; set; } = new Font("Segoe UI", 10F);

        public static void FormaUygula(Form form)
        {
            form.BackColor = ArkaPlan;
            form.ForeColor = MetinRengi;
            form.Font = NormalFont;

            foreach (Control kontrol in TumKontroller(form))
            {
                if (kontrol is Button buton)
                {
                    buton.BackColor = VurguRengi;
                    buton.ForeColor = Color.White;
                    buton.FlatStyle = FlatStyle.Flat;
                    buton.FlatAppearance.BorderSize = 0;
                }
                else if (kontrol is TextBox || kontrol is ComboBox)
                {
                    kontrol.BackColor = YuzeyRengi;
                    kontrol.ForeColor = MetinRengi;
                }
                else if (kontrol is Label etiket && etiket.Tag as string == "baslik")
                {
                    etiket.Font = BaslikFontu;
                }
            }
        }

        private static IEnumerable<Control> TumKontroller(Control kok)
        {
            foreach (Control kontrol in kok.Controls)
            {
                yield return kontrol;
                foreach (var alt in TumKontroller(kontrol))
                    yield return alt;
            }
        }
    }
}