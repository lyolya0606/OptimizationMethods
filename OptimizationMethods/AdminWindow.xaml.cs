using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ChartDirector;

namespace OptimizationMethods {
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window {
        public AdminWindow() {
            InitializeComponent();
            FillTable();
        }
        
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            //string s = "\n";
            if (TextBoxVariant.Text == "") {
                MessageBox.Show("Введите номер варианта");
                return;
            }
            if (CheckFile(TextBoxVariant.Text)) {
                MessageBox.Show("Такой вариант уже есть");
                return;
            }
            StreamWriter sr = new StreamWriter(new FileStream("variants.txt", FileMode.Append));
            sr.WriteLine(TextBoxVariant.Text);
            sr.Close();
            TextBoxVariant.Text = "";
            FillTable();
        }

        private void FillTable() {
            DataTable dt = new DataTable();
            dt.Columns.Add("Вариант №");
            StreamReader sr = new StreamReader("variants.txt");
            string line = sr.ReadLine();
            while (line != null) {
                dt.Rows.Add("Вариант " + line);
                line = sr.ReadLine();
            }
            sr.Close();
            VariantTable.ItemsSource = dt.DefaultView;
        }

        private bool CheckFile(string x) {
            List<string> check = new();
            StreamReader sr = new StreamReader("variants.txt");
            string line = sr.ReadLine();
            while (line != null) {
                check.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
            return check.Contains(x);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            List<string> check = new();
            StreamReader sr = new StreamReader("variants.txt");
            string line = sr.ReadLine();
            while (line != null) {
                check.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();

            if (!check.Contains(TextBoxVariant.Text)) {
                MessageBox.Show("Такого варианта нет");
                return;
            }

            check.Remove(TextBoxVariant.Text);
            StreamWriter sw = new StreamWriter("variants.txt");
            foreach (string el in check) {
                sw.WriteLine(el);
            }
            sw.Close();
            TextBoxVariant.Text = "";
            FillTable();
            
        }
        
        private void Exit_Click(object sender, RoutedEventArgs e) {
            Hide();
            new LoginWindow().Show();
            Close();
        }
    }
}
