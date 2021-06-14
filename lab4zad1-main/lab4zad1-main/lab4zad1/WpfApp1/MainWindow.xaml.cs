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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.Reflection;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Studenci> ListaStudentow { set; get; }
        

        public MainWindow()

        {            
            ListaStudentow = new List<Studenci>()
            {
                new Studenci() { Imie = "Jan",Nazwisko="Kowalski",NrIndeksu=1234,Wydzial="KIS" },
                new Studenci() { Imie = "Anna",Nazwisko="Nowak",NrIndeksu=4321,Wydzial="KIS" },
                new Studenci() { Imie = "Michał",Nazwisko="Jacek",NrIndeksu=3421,Wydzial="KIS" }
            };
            InitializeComponent();
            
            DgStudenci.Columns.Add(new DataGridTextColumn() { Header = "Imie", Binding = new Binding("Imie") });
            DgStudenci.Columns.Add(new DataGridTextColumn() { Header = "Nazwisko", Binding = new Binding("Nazwisko") });
            DgStudenci.Columns.Add(new DataGridTextColumn() { Header = "NrIndeksu", Binding = new Binding("NrIndeksu") });
            DgStudenci.Columns.Add(new DataGridTextColumn() { Header = "Wydzial", Binding = new Binding("Wydzial") });

            DgStudenci.AutoGenerateColumns = false;
            DgStudenci.ItemsSource = ListaStudentow;

        }

        


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new StudentWindow();
            if (dialog.ShowDialog() == true)
            {

                ListaStudentow.Add(dialog.student);
                DgStudenci.Items.Refresh();
            }
        }
        private void btnSub_Click(object sender, RoutedEventArgs e)
        {
            if (DgStudenci.SelectedItem is Studenci)
            {
                ListaStudentow.Remove((Studenci)DgStudenci.SelectedItem);
                DgStudenci.Items.Refresh();
            }
        }
        private void DgStudenci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void Save<T>(T ob, StreamWriter sw)
        {
            Type t = ob.GetType();
            sw.WriteLine($"[[{t.FullName}]]");
            foreach (var p in t.GetRuntimeProperties())
            {
                sw.WriteLine($"[{p.Name}]");
                sw.WriteLine(p.GetValue(ob));
            }
            sw.WriteLine($"[[]]");
        }

        public T Load<T>(StreamReader sr) where T : new()
        {
            T ob = default(T);
            Type tob = null;
            PropertyInfo property = null;
            while (!sr.EndOfStream)
            {
                var ln = sr.ReadLine();
                if (ln == "[[]]")
                    return ob;
                else if (ln.StartsWith("[["))
                {
                    tob = Type.GetType(ln.Trim('[', ']'));
                    if (typeof(T).IsAssignableFrom(tob))
                        ob = (T)Activator.CreateInstance(tob);
                }
                else if (ln.StartsWith("[") && ob != null)
                    property = tob.GetProperty(ln.Trim('[', ']'));
                else if (ob != null && property != null)
                    property.SetValue(ob, Convert.ChangeType(ln, property.PropertyType));
            }
            return default(T);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog abc = new SaveFileDialog();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    Save<Studenci>(ListaStudentow[0], sw);
                    sw.Close();
                    MessageBox.Show("Pomyślnie zapisano plik txt");
                }
                else
                    MessageBox.Show("Nie udało się zapisać pliku txt");
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                ListaStudentow.Add(Load<Studenci>(sr));
                DgStudenci.Items.Refresh();
                fs.Close();
                MessageBox.Show("Pomyślnie otworzono plik txt");
            }
            else
                MessageBox.Show("Nie udało się otworzyć pliku txt");
        }

        private void btnSXML_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog abc = new SaveFileDialog();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                    XmlSerializer serializer = new XmlSerializer(typeof(Studenci));
                    
                    serializer.Serialize(fs, ListaStudentow[0]);
                    fs.Close();
                    MessageBox.Show("Pomyślnie zapisano plik xml");
                }
                else
                    MessageBox.Show("Nie udało się zapisać pliku xml");
            }
        }

        private void btnLXML_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xml files (*.xml)|*.xml";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                Studenci a;
                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(Studenci));
                a = (Studenci)serializer.Deserialize(fs);
                ListaStudentow.Add(a);
                DgStudenci.Items.Refresh();
                fs.Close();
                MessageBox.Show("Pomyślnie otworzono plik xml");
            }
            else
                MessageBox.Show("Nie udało się otworzyć pliku xml");
        }
    }
}