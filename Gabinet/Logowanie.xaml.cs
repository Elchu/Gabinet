using System.Security.Cryptography;
using Gabinet.Database;
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
using Gabinet.lib;

namespace Gabinet
{
    /// <summary>
    /// Logika interakcji dla klasy Logowanie.xaml
    /// </summary>
    public partial class Logowanie : Window
    {
        public Logowanie()
        {
            InitializeComponent();
        }

        private void BtnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new GabinetEntities())
            {
                var uzytkownik =
                    db.uzytkownik.FirstOrDefault(u => u.login.Equals(TxtLogin.Text) && u.haslo.Equals(TxtPassword.Password));
                if (uzytkownik != null)
                {
                    var okno = new MainWindow(uzytkownik);
                    okno.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Błąd logowania", "Logowanie", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
       
    }
}
