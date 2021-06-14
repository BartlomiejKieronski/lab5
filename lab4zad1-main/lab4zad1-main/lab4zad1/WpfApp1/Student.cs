namespace WpfApp1
{
    public partial class MainWindow
    {
        public class Studenci
        {
            public string Imie { get; set; }
            public string Nazwisko { get; set; }
            public int NrIndeksu { get; set; }
            public string Wydzial { get; set; }
            public Studenci(string Imie, string Nazwisko, int NrIndeksu, string Wydzial)
            {
                Studenci student = this;
                this.Imie = Imie;
                this.Nazwisko = Nazwisko;
                this.NrIndeksu = NrIndeksu;
                this.Wydzial = Wydzial;
            }
            public Studenci()
                : this("", "", 0, "")
            { }
        }
    }
}