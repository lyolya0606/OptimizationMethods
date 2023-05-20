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
using System.Configuration;

namespace OptimizationMethods {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        private readonly string _login;
        private readonly string _password;
        public LoginWindow() {
            InitializeComponent();
            _login = ConfigurationManager.AppSettings["Login"];
            _password = ConfigurationManager.AppSettings["Password"];
            
        }

        private void UserButton_MouseDown(object sender, MouseButtonEventArgs e) {
            //Hide();
            //new MainWindow().Show();
            //Close();
        }

        private void UserButton_Click(object sender, RoutedEventArgs e) {
            new MainWindow().Show();
            Close();
        }
        
        private void AdminButton_Click(object sender, RoutedEventArgs e) {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;
            if (login == _login && password == _password) {
                new AdminWindow().Show();
                Close();
            } else {
                MessageBox.Show("Неккоректный логин или пароль");
            }
        }
    }
}
