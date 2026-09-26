namespace vuluYonetimSistemi
{
    public class TemaliForm : Form
    {
        public TemaliForm()
        {
            this.Load += (s, e) => Tema.FormaUygula(this);
        }
    }
}