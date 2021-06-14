using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using static WpfApp1.MainWindow;
using System.Text.RegularExpressions;


namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public Studenci student = new Studenci();
        public StudentWindow(Studenci student = null)
        {
            InitializeComponent();
            if (student != null)
            {
                tbI.Text = student.Imie;
                tbN.Text = student.Nazwisko;
                tbNr.Text = student.NrIndeksu.ToString();
                tbW.Text = student.Wydzial;

            }
            this.student = student ?? new Studenci();

        }
        public StudentWindow()
        {
            InitializeComponent();
        }
        private void btnOk_Click (object sender, RoutedEventArgs e)
        {
             if (!Regex.IsMatch(tbI.Text, @"^\p{L}{1,12}$") ||
                !Regex.IsMatch(tbN.Text, @"^\p{L}{1,12}$") ||
                !Regex.IsMatch(tbW.Text, @"^\p{L}{1,12}$") ||
                !Regex.IsMatch(tbNr.Text, @"^[0-9]{4,10}$"))

            {
                MessageBox.Show("Błędne dane");
                  return;
              }

            student.Imie = tbI.Text;
            student.Nazwisko = tbN.Text;
            student.NrIndeksu = int.Parse(tbNr.Text);
            student.Wydzial = tbW.Text;
            this.DialogResult = true;
        }
    }
}


















