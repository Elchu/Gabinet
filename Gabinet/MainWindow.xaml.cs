using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gabinet.Database;
using System.Data.Entity;
using Gabinet.ViewModel;
using DataGrid = System.Windows.Controls.DataGrid;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace Gabinet
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private GabinetEntities db = new GabinetEntities();
        private readonly int _uzytkownikId; //id uzytkownika zalogowanego
        private int _klientId; // wybranie klienta
        private int _uslugaId; // wybranie uslugi
        private int _pracownikId; //wybranie pracownika
        private int _wizytaId; //wybranie zarezerwowanej wizyty
        private string _dataW; // wybranie daty
        private string _godzinaW; //wybranie godziny

        public MainWindow(uzytkownik uzytkownikId)
        {
            _uzytkownikId = uzytkownikId.uzytkownik_id;
            InitializeComponent();
        }
        #region tabHaslo

        private void TxtHstare_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SprawdzDane();
        }

        private void TxtHnowe1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SprawdzDane();
        }

        private void TxtHnowe2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SprawdzDane();
        }

        private void BtnHzmienHaslo_Click(object sender, RoutedEventArgs e)
        {
            var uzytkownik = db.uzytkownik.FirstOrDefault(u => u.uzytkownik_id.Equals(_uzytkownikId));

            if (uzytkownik != null)
            {
                if (SprawdzHaslo(uzytkownik.haslo))
                {
                    uzytkownik.haslo = TxtHnowe1.Password;
                    db.Entry(uzytkownik).State = EntityState.Modified;
                    db.SaveChanges();

                    MessageBox.Show("Hasło zostało zmienione", "Potwierdzenie", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    WyczyscHasla();
                }
                else
                    MessageBox.Show("Wprowadz poprawne hasło", "Hasło", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Błąd serwera, spróbuj za chwilę", "Informacja", MessageBoxButton.OK,
                    MessageBoxImage.Information);
        }

        #region Funcke pomocnicze

        /// <summary>
        /// funcka sprawdza czy zostały wpisane dane i czy są takie same, jeśli tak to ustawia przycisk na aktywny
        /// </summary>
        private void SprawdzDane()
        {
            if (TxtHstare.Password != "" && TxtHnowe1.Password != "" && TxtHnowe2.Password != "" &&
                TxtHnowe1.Password.Equals(TxtHnowe2.Password))
                BtnHzmienHaslo.IsEnabled = true;
            else
                BtnHzmienHaslo.IsEnabled = false;
        }

        /// <summary>
        /// funcka sprawdza czy hasla sa identyczne
        /// </summary>
        /// <param name="haslo">podajemy haslo uzytkownika z bazy danych</param>
        /// <returns>zwraca true jesli hasla sa takie same</returns>
        private bool SprawdzHaslo(string haslo)
        {
            if (TxtHstare.Password.Equals(haslo))
                return true;

            return false;
        }

        /// <summary>
        /// funcka czyści wpiasne hasla
        /// </summary>
        private void WyczyscHasla()
        {
            TxtHstare.Password = "";
            TxtHnowe1.Password = "";
            TxtHnowe2.Password = "";
        }
        #endregion Funcke pomocnicze
        #endregion tabHaslo
        #region tabUzytkownicy

        #region Funkcje pomocnicze
        private void PokazSiatkeUzytkownikow()
        {
            DgUzytkownicy.ItemsSource = db.uzytkownik.ToList();
            DgUzytkownicy.Columns[0].Visibility = Visibility.Hidden;
            DgUzytkownicy.Columns[6].Visibility = Visibility.Hidden;
        }

        private bool SprawdzFormularz(bool sprawdzLogin = true)
        {
            if (TxtPimie.Text.Length >= 3 && TxtPnazwisko.Text.Length >= 4 && TxtPlogin.Text.Length >= 4)
            {
                if (sprawdzLogin)
                {
                    var login = db.uzytkownik.FirstOrDefault(u => u.login.Equals(TxtPlogin.Text));

                    //sprawdzamy czy podany login jest uzywany, jesli nie to rejestrujemy uzytkownika
                    if (login == null)
                        return true;

                    MessageBox.Show("Podany login jest już wykorzystany, podaj inny", "Infomacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }

                return true;
            }

            MessageBox.Show("Uzupełnij poprawnie dane", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            return false;

        }

        private void CzyscDaneFormularza()
        {
            TxtPimie.Text = "";
            TxtPnazwisko.Text = "";
            TxtPlogin.Text = "";
            CbPpracownik.IsChecked = false;
        }

        private void CzyscGodziny()
        {
            TxtPg1P.Text = "";
            TxtPg1K.Text = "";
            TxtPg2P.Text = "";
            TxtPg2K.Text = "";
            TxtPg3P.Text = "";
            TxtPg3K.Text = "";
            TxtPg4P.Text = "";
            TxtPg4K.Text = "";
            TxtPg5P.Text = "";
            TxtPg5K.Text = "";
            TxtPg6P.Text = "";
            TxtPg6K.Text = "";
            TxtPg7P.Text = "";
            TxtPg7K.Text = "";
        }

        /// <summary>
        /// funcka wypelnia textboxy danymi
        /// </summary>
        /// <param name="start">podaj czas poczatkowy pracy</param>
        /// <param name="koniec">podaj czas konca pracy</param>
        private void WypelnijGodziny(string start, string koniec)
        {
            TextBox[] czasStart = new TextBox[] { TxtPg1P, TxtPg2P, TxtPg3P, TxtPg4P, TxtPg5P, TxtPg6P, TxtPg7P };
            TextBox[] czasKoniec = new TextBox[] { TxtPg1K, TxtPg2K, TxtPg3K, TxtPg4K, TxtPg5K, TxtPg6K, TxtPg7K };

            for (int i = 0; i < czasStart.Length; i++)
            {
                czasStart[i].Text = start;
                czasKoniec[i].Text = koniec;
            }
        }

        private void WypelnijGodziny7_15()
        {
            WypelnijGodziny("7:00", "15:00");
        }

        private void WypelnijGodziny8_16()
        {
            WypelnijGodziny("8:00", "16:00");
        }

        private void WypelnijGodziny9_17()
        {
            WypelnijGodziny("9:00", "17:00");
        }

        private void WypelnijGodziny10_18()
        {
            WypelnijGodziny("10:00", "18:00");
        }
        #endregion //Funkcje pomocnicze

        private void BtnPSzukaj_Click(object sender, RoutedEventArgs e)
        {
            SzukajPracownikow(TxtPSzukaj, DgUzytkownicy);

            DgUzytkownicy.Columns[0].Visibility = Visibility.Hidden;
            DgUzytkownicy.Columns[6].Visibility = Visibility.Hidden;
        }
        private void DgUzytkownicy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var zaznaczonyUzytkownik = (uzytkownik)DgUzytkownicy.SelectedItem;

            if (zaznaczonyUzytkownik != null)
            {
                TbId.Text = zaznaczonyUzytkownik.uzytkownik_id.ToString();
                TxtPimie.Text = zaznaczonyUzytkownik.imie;
                TxtPnazwisko.Text = zaznaczonyUzytkownik.nazwisko;
                TxtPlogin.Text = zaznaczonyUzytkownik.login;
                CbPpracownik.IsChecked = zaznaczonyUzytkownik.pracownik;

                BtnPUsun.IsEnabled = true;
                BtnPuaktualnij.IsEnabled = true;

                if (CbPpracownik.IsChecked == true)
                {
                    var godziny = db.godziny.FirstOrDefault(g => g.uzytkownik_id == zaznaczonyUzytkownik.uzytkownik_id);

                    if (godziny != null)
                    {
                        TxtPg1P.Text = godziny.pon_od.ToString();
                        TxtPg1K.Text = godziny.pon_do.ToString();
                        TxtPg2P.Text = godziny.wt_od.ToString();
                        TxtPg2K.Text = godziny.wt_do.ToString();
                        TxtPg3P.Text = godziny.sr_od.ToString();
                        TxtPg3K.Text = godziny.sr_do.ToString();
                        TxtPg4P.Text = godziny.czw_od.ToString();
                        TxtPg4K.Text = godziny.czw_do.ToString();
                        TxtPg5P.Text = godziny.pt_od.ToString();
                        TxtPg5K.Text = godziny.pt_do.ToString();
                        TxtPg6P.Text = godziny.sb_od.ToString();
                        TxtPg6K.Text = godziny.sb_do.ToString();
                        TxtPg7P.Text = godziny.nd_od.ToString();
                        TxtPg7K.Text = godziny.nd_do.ToString();
                    }
                }
                else
                    CzyscGodziny();
            }
        }
        private void BtnPDodaj_Click(object sender, RoutedEventArgs e)
        {

            if (SprawdzFormularz())
            {
                var newUzytkownik = new uzytkownik()
                {
                    imie = TxtPimie.Text,
                    nazwisko = TxtPnazwisko.Text,
                    login = TxtPlogin.Text,
                    pracownik = CbPpracownik.IsChecked
                };

                db.uzytkownik.Add(newUzytkownik);
                try
                {
                    db.SaveChanges();

                    if (CbPpracownik.IsChecked == true)
                    {
                        var newGodziny = new godziny();
                        newGodziny.uzytkownik_id = newUzytkownik.uzytkownik_id;
                        newGodziny.pon_od = Convert.ToDateTime(TxtPg1P.Text).TimeOfDay;
                        newGodziny.pon_do = Convert.ToDateTime(TxtPg1K.Text).TimeOfDay;
                        newGodziny.wt_od = Convert.ToDateTime(TxtPg2P.Text).TimeOfDay;
                        newGodziny.wt_do = Convert.ToDateTime(TxtPg2K.Text).TimeOfDay;
                        newGodziny.sr_od = Convert.ToDateTime(TxtPg3P.Text).TimeOfDay;
                        newGodziny.sr_do = Convert.ToDateTime(TxtPg3K.Text).TimeOfDay;
                        newGodziny.czw_od = Convert.ToDateTime(TxtPg4P.Text).TimeOfDay;
                        newGodziny.czw_do = Convert.ToDateTime(TxtPg4K.Text).TimeOfDay;
                        newGodziny.pt_od = Convert.ToDateTime(TxtPg5P.Text).TimeOfDay;
                        newGodziny.pt_do = Convert.ToDateTime(TxtPg5K.Text).TimeOfDay;
                        newGodziny.sb_od = Convert.ToDateTime(TxtPg6P.Text).TimeOfDay;
                        newGodziny.sb_do = Convert.ToDateTime(TxtPg6K.Text).TimeOfDay;
                        newGodziny.nd_od = Convert.ToDateTime(TxtPg7P.Text).TimeOfDay;
                        newGodziny.nd_do = Convert.ToDateTime(TxtPg7K.Text).TimeOfDay;

                        db.godziny.Add(newGodziny);

                        db.SaveChanges();
                    }

                    PokazSiatkeUzytkownikow();
                    CzyscDaneFormularza();
                    MessageBox.Show("Użytkownik został dodany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Wystąpił bład podczas zapisywania.\nSpróbuj ponownie później.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void BtnPuaktualnij_Click(object sender, RoutedEventArgs e)
        {
            if (SprawdzFormularz(false))
            {
                int id = Convert.ToInt32(TbId.Text);
                var edytowanyUzytkownik = db.uzytkownik.FirstOrDefault(u => u.uzytkownik_id == id);
                edytowanyUzytkownik.imie = TxtPimie.Text;
                edytowanyUzytkownik.nazwisko = TxtPnazwisko.Text;
                edytowanyUzytkownik.login = TxtPlogin.Text;
                edytowanyUzytkownik.pracownik = CbPpracownik.IsChecked;

                db.Entry(edytowanyUzytkownik).State = EntityState.Modified;

                if (CbPpracownik.IsChecked == true)
                {

                    var edytujGodz = db.godziny.FirstOrDefault(g => g.uzytkownik_id == id);

                    if (edytujGodz != null)
                    {
                        edytujGodz.pon_od = Convert.ToDateTime(TxtPg1P.Text).TimeOfDay;
                        edytujGodz.pon_do = Convert.ToDateTime(TxtPg1K.Text).TimeOfDay;
                        edytujGodz.wt_od = Convert.ToDateTime(TxtPg2P.Text).TimeOfDay;
                        edytujGodz.wt_do = Convert.ToDateTime(TxtPg2K.Text).TimeOfDay;
                        edytujGodz.sr_od = Convert.ToDateTime(TxtPg3P.Text).TimeOfDay;
                        edytujGodz.sr_do = Convert.ToDateTime(TxtPg3K.Text).TimeOfDay;
                        edytujGodz.czw_od = Convert.ToDateTime(TxtPg4P.Text).TimeOfDay;
                        edytujGodz.czw_do = Convert.ToDateTime(TxtPg4K.Text).TimeOfDay;
                        edytujGodz.pt_od = Convert.ToDateTime(TxtPg5P.Text).TimeOfDay;
                        edytujGodz.pt_do = Convert.ToDateTime(TxtPg5K.Text).TimeOfDay;
                        edytujGodz.sb_od = Convert.ToDateTime(TxtPg6P.Text).TimeOfDay;
                        edytujGodz.sb_do = Convert.ToDateTime(TxtPg6K.Text).TimeOfDay;
                        edytujGodz.nd_od = Convert.ToDateTime(TxtPg7P.Text).TimeOfDay;
                        edytujGodz.nd_do = Convert.ToDateTime(TxtPg7K.Text).TimeOfDay;

                        db.Entry(edytujGodz).State = EntityState.Modified;
                    }
                    else
                    {
                        var newGodziny = new godziny();
                        newGodziny.uzytkownik_id = id;
                        newGodziny.pon_od = Convert.ToDateTime(TxtPg1P.Text).TimeOfDay;
                        newGodziny.pon_do = Convert.ToDateTime(TxtPg1K.Text).TimeOfDay;
                        newGodziny.wt_od = Convert.ToDateTime(TxtPg2P.Text).TimeOfDay;
                        newGodziny.wt_do = Convert.ToDateTime(TxtPg2K.Text).TimeOfDay;
                        newGodziny.sr_od = Convert.ToDateTime(TxtPg3P.Text).TimeOfDay;
                        newGodziny.sr_do = Convert.ToDateTime(TxtPg3K.Text).TimeOfDay;
                        newGodziny.czw_od = Convert.ToDateTime(TxtPg4P.Text).TimeOfDay;
                        newGodziny.czw_do = Convert.ToDateTime(TxtPg4K.Text).TimeOfDay;
                        newGodziny.pt_od = Convert.ToDateTime(TxtPg5P.Text).TimeOfDay;
                        newGodziny.pt_do = Convert.ToDateTime(TxtPg5K.Text).TimeOfDay;
                        newGodziny.sb_od = Convert.ToDateTime(TxtPg6P.Text).TimeOfDay;
                        newGodziny.sb_do = Convert.ToDateTime(TxtPg6K.Text).TimeOfDay;
                        newGodziny.nd_od = Convert.ToDateTime(TxtPg7P.Text).TimeOfDay;
                        newGodziny.nd_do = Convert.ToDateTime(TxtPg7K.Text).TimeOfDay;

                        db.godziny.Add(newGodziny);
                    }

                    db.SaveChanges();
                }

                try
                {
                    db.SaveChanges();

                    CzyscDaneFormularza();
                    PokazSiatkeUzytkownikow();
                    MessageBox.Show("Dane zostały zaktualizowane", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Wystąpił błąd podczas aktualizowana. \nSpróbuj później", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void BtnPWyczysc_Click(object sender, RoutedEventArgs e)
        {
            CzyscDaneFormularza();
        }
        private void BtnPUsun_Click(object sender, RoutedEventArgs e)
        {
            if (SprawdzFormularz(false))
            {
                int id = Convert.ToInt32(TbId.Text);

                if (id != 1)
                {
                    var usunUzytkownika = db.uzytkownik.FirstOrDefault(u => u.uzytkownik_id == id);
                    if (usunUzytkownika != null)
                    {

                        if (MessageBox.Show("Czy jesteś pewien, że chcesz usunąć użytkownika?", "Informacja", MessageBoxButton.YesNoCancel, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            var godzUzyt = db.godziny.FirstOrDefault(g => g.uzytkownik_id == id);

                            if (godzUzyt != null)
                                db.godziny.Remove(godzUzyt);

                            IQueryable<uzytkownik_usluga> uu = db.uzytkownik_usluga.Where(u => u.uzytkownik_id == id);

                            if (uu.Any())
                                db.uzytkownik_usluga.RemoveRange(uu);

                            IQueryable<wizyty> wizyty = db.wizyty.Where(w => w.uzytkownik_id == id);

                            if (wizyty.Any())
                                db.wizyty.RemoveRange(wizyty);

                            db.uzytkownik.Remove(usunUzytkownika);

                            try
                            {
                                db.SaveChanges();
                                MessageBox.Show("Użytkownik został usunięty.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                                CzyscDaneFormularza();
                                PokazSiatkeUzytkownikow();
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Wystąpił błąd podczas usuwania. \nSpróbuj później.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                        MessageBox.Show("Użytkownik nie został odnaleziony. \nSpróbuj ponownie później", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Nie można usunąć administratora.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CbPpracownik_Checked(object sender, RoutedEventArgs e)
        {
            GbPGodziny.Visibility = Visibility.Visible;
            CzyscGodziny();
        }
        private void CbPpracownik_Unchecked(object sender, RoutedEventArgs e)
        {
            GbPGodziny.Visibility = Visibility.Hidden;
            CzyscGodziny();
        }

        private void BtnWypelnijGodziny715_Click(object sender, RoutedEventArgs e)
        {
            WypelnijGodziny7_15();
        }

        private void BtnWypelnijGodziny816_Click(object sender, RoutedEventArgs e)
        {
            WypelnijGodziny8_16();
        }

        private void BtnWypelnijGodziny917_Click(object sender, RoutedEventArgs e)
        {
            WypelnijGodziny9_17();
        }

        private void BtnWypelnijGodziny1018_Click(object sender, RoutedEventArgs e)
        {
            WypelnijGodziny10_18();
        }

        #endregion
        #region tabUslugi
        private void BtnUSzukaj_Click(object sender, RoutedEventArgs e)
        {
            SzukajUslugi(TxtUSzukaj, DgUslugi);

            DgUslugi.Columns[0].Visibility = Visibility.Hidden;
        }
        private void BtnUdodaj_Click(object sender, RoutedEventArgs e)
        {
            var usluga = new uslugi()
            {
                nazwa = TxtUnazwa.Text,
                cena = Convert.ToDecimal(TxtUcena.Text),
                czas = TimeSpan.Parse(TxtUczas.Text),
                opis = TxtUopis.Text
            };

            db.uslugi.Add(usluga);

            try
            {
                db.SaveChanges();
                MessageBox.Show("Usługa została dodana", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                WyczyscUslugi();
                ZaladujListeUslug();
            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił błąd. Spróbuj później.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnUmodyfikuj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbUid.Text))
            {
                int idUslugi = Convert.ToInt32(TbUid.Text);
                var editUsluga = db.uslugi.FirstOrDefault(u => u.usluga_id == idUslugi);

                if (editUsluga != null)
                {
                    editUsluga.nazwa = TxtUnazwa.Text;
                    editUsluga.cena = Convert.ToDecimal(TxtUcena.Text.Replace('.', ','));
                    editUsluga.czas = Convert.ToDateTime(TxtUczas.Text).TimeOfDay;
                    editUsluga.opis = TxtUopis.Text;

                    db.Entry(editUsluga).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                        ZaladujListeUslug();
                        WyczyscUslugi();
                        MessageBox.Show("Usługa została zaktualizowana.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Błąd podczas aktualizacji.\nSpróbuj później", "Informacja", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                    MessageBox.Show("Usługa nie została odnaleziona.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
                MessageBox.Show("Wybierz usługę do modyfikacji.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void BtnUusun_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy jesteś pewien, że chcesz usunąć usługę?", "Informacja", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                int idUslugi = Convert.ToInt32(TbUid.Text);

                var usluga = db.uslugi.FirstOrDefault(u => u.usluga_id == idUslugi);

                if (usluga != null)
                    db.uslugi.Remove(usluga);
                else
                    MessageBox.Show("Usluga nie została odnaleziona.\nSpróbuj później", "Informacja", MessageBoxButton.OK, MessageBoxImage.Error);

                try
                {
                    db.SaveChanges();
                    ZaladujListeUslug();
                    WyczyscUslugi();
                    MessageBox.Show("Usługa została usunięta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Usługa nie została usunięta.\nSpróbuj później", "Informacja", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void DgUslugi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var usluga = (uslugi)DgUslugi.SelectedItem;
            if (usluga != null)
            {
                TbUid.Text = usluga.usluga_id.ToString();
                TxtUnazwa.Text = usluga.nazwa;
                TxtUcena.Text = usluga.cena.ToString();
                TxtUczas.Text = usluga.czas.ToString();
                TxtUopis.Text = usluga.opis;

                BtnUusun.IsEnabled = true;
                BtnUmodyfikuj.IsEnabled = true;
            }


        }
        private void BtnUwyczysc_Click(object sender, RoutedEventArgs e)
        {
            WyczyscUslugi();
        }
        #region Funkcje pomocnicze

        private void ZaladujListeUslug()
        {
            DgUslugi.ItemsSource = db.uslugi.ToList();

            DgUslugi.Columns[0].Visibility = Visibility.Hidden;
        }

        private void WyczyscUslugi()
        {
            TbUid.Text = "";
            TxtUnazwa.Text = "";
            TxtUcena.Text = "";
            TxtUczas.Text = "";
            TxtUopis.Text = "";

            BtnUusun.IsEnabled = false;
            BtnUmodyfikuj.IsEnabled = false;
        }
        #endregion //Funkcje pomocnicze
        #endregion
        #region tabUslugiPracownicy
        //wyszukuje pracownikow
        private void BtnUPSzukaj_Click(object sender, RoutedEventArgs e)
        {
            SzukajPracownikow(TxtUPSzukaj, DgUPuzytkownicy, true);

            DgUPuzytkownicy.Columns[0].Visibility = Visibility.Hidden;
            DgUPuzytkownicy.Columns[6].Visibility = Visibility.Hidden;

            DgUPuslugi.UnselectAllCells();
            DgUPuslugi.ItemsSource = db.uslugi.ToList();
            DgUPuslugi.Columns[0].Visibility = Visibility.Hidden;
            DgUPuslugi.Columns[5].Visibility = Visibility.Hidden;
        }
        //wybiera pracownika i uslugi
        private void DgUPuzytkownicy_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            uzytkownik uzytkownikZaznaczony = (uzytkownik)DgUPuzytkownicy.SelectedItem;

            if (uzytkownikZaznaczony != null)
            {
                TxtUPlogin.Text = uzytkownikZaznaczony.login;
                TxtUPimie.Text = uzytkownikZaznaczony.imie;
                TxtUPnazwisko.Text = uzytkownikZaznaczony.nazwisko;
                TbUPidPracownika.Text = uzytkownikZaznaczony.uzytkownik_id.ToString();

                SzukajUslugiPracownika(uzytkownikZaznaczony.uzytkownik_id, DgUPuslugiPracownika);
                DgUPuslugiPracownika.Columns[0].Visibility = Visibility.Hidden;
                DgUPuslugiPracownika.Columns[1].Visibility = Visibility.Hidden;
                DgUPuslugiPracownika.Columns[2].Visibility = Visibility.Hidden;    
            }
        }
        //Dodaje usluge do pracownika
        private void DgUPuslugi_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DgUPuslugi.SelectedIndex >= 0)
            {//jezeli wiersz zostal zaznaczony
                if (TbUPidPracownika.Text != "")
                {//sprawdzamy czy pracownik zostal wybrany
                    if (MessageBox.Show("Czy na pewno chcesz dodać usługę do pracownika?", "Informacja", MessageBoxButton.YesNoCancel, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    { //jezeli chcemy dodac usluge do pracownika
                        var zaznaczonaUsluga = (uslugi)DgUPuslugi.SelectedItem;

                        int idZaznaczonegoUzytkownika = Convert.ToInt32(TbUPidPracownika.Text);
                        var sprawdzUslugePracownika =
                            db.uzytkownik_usluga.FirstOrDefault(
                                u =>
                                    u.usluga_id == zaznaczonaUsluga.usluga_id &&
                                    u.uzytkownik_id == idZaznaczonegoUzytkownika);

                        if (sprawdzUslugePracownika == null)
                        {//jezeli pracownik jeszcze nie posiada uslugi
                            var uslugiUzytkownika = new uzytkownik_usluga
                            {
                                uzytkownik_id = idZaznaczonegoUzytkownika,
                                usluga_id = zaznaczonaUsluga.usluga_id
                            };

                            db.uzytkownik_usluga.Add(uslugiUzytkownika);
                            try
                            {
                                db.SaveChanges();
                                MessageBox.Show("Usługa została dodana do użytkownika", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                                DgUPuslugi.UnselectAllCells();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Wystąpił błąd podczas dodawania usługi." + ex.Message, "Informacja",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                            MessageBox.Show("Pracownik posiada już zaznaczoną usługę", "Informacja", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                    }

                    DgUPuslugi.UnselectAllCells();
                }

            }

        }
        //wyswietla uslugi jakie wykonuje pracownik
        private void DgUPuslugiPracownika_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DgUPuslugiPracownika.SelectedIndex >= 0)
            {//jezeli wiersz jest zaznaczony
                if (TbUPidPracownika.Text != "")
                {//sprawdzamy czy pracownik zostal wybrany
                    if (MessageBox.Show("Czy na pewno chcesz usunąć usługę pracownikowi?", "Informacja", MessageBoxButton.YesNoCancel, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {//jezeli chcemy usunac usluge
                        var zaznaczonaUslugaPracownika = (PracownikUsluga)DgUPuslugiPracownika.SelectedItem;

                        var uslugaDoUsuniecia =
                            db.uzytkownik_usluga.FirstOrDefault(
                                u =>
                                    u.uzytkownik_id == zaznaczonaUslugaPracownika.uzytkownik_id &&
                                    u.usluga_id == zaznaczonaUslugaPracownika.usluga_id);

                        if (uslugaDoUsuniecia != null)
                        {//jezeli usluga zostala znaleziona
                            db.uzytkownik_usluga.Remove(uslugaDoUsuniecia);

                            try
                            {
                                db.SaveChanges();

                                SzukajUslugiPracownika(zaznaczonaUslugaPracownika.uzytkownik_id, DgUPuslugiPracownika);
                                DgUPuslugiPracownika.Columns[0].Visibility = Visibility.Hidden;
                                DgUPuslugiPracownika.Columns[1].Visibility = Visibility.Hidden;
                                DgUPuslugiPracownika.Columns[2].Visibility = Visibility.Hidden;

                                MessageBox.Show("Usługa została usunięta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

                                DgUPuslugiPracownika.UnselectAllCells();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Wystąpił błąd podczas usuwania." + ex.Message, "Informacja",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            }

                        }
                        else
                            MessageBox.Show("Usługa nie została odnaleziona. Spróbuj później", "Informacja",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    DgUPuslugiPracownika.UnselectAllCells();
                }
            }
        }
        #endregion
        #region Funkcje pomocnicze globalne
        /// <summary>
        /// funkcja wyszukuje pracownikow i umieszcza liste pracownikow w datagrid
        /// </summary>
        /// <param name="pole">podaj nazwe textbox z wynikiem wyszukiwania</param>
        /// <param name="siatka">podaj nazwe datagrid gdzie dane maja zostac umieszczone</param>
        /// /// <param name="uzytkownikPracownik">true wysukuje pracownikow, false uzytkownikow</param>
        private void SzukajPracownikow(TextBox pole, DataGrid siatka, bool uzytkownikPracownik)
        {
            var szukaniUzytkownicy = (from u in db.uzytkownik
                                      where (u.imie.Contains(pole.Text) ||
                                      u.nazwisko.Contains(pole.Text) ||
                                      u.login.Contains(pole.Text)) &&
                                      u.pracownik == uzytkownikPracownik
                                      select u).ToList();

            siatka.ItemsSource = szukaniUzytkownicy;
        }
        /// <summary>
        /// funkcja wyszukuje pracownikow i umieszcza liste pracownikow w datagrid
        /// </summary>
        /// <param name="pole">podaj nazwe textbox z wynikiem wyszukiwania</param>
        /// <param name="siatka">podaj nazwe datagrid gdzie dane maja zostac umieszczone</param>
        private void SzukajPracownikow(TextBox pole, DataGrid siatka)
        {
            var szukaniUzytkownicy = (from u in db.uzytkownik
                                      where u.imie.Contains(pole.Text) ||
                                      u.nazwisko.Contains(pole.Text) ||
                                      u.login.Contains(pole.Text)
                                      select u).ToList();

            siatka.ItemsSource = szukaniUzytkownicy;

        }
        /// <summary>
        /// funkcja wyszukuje uslugi i umieszcza liste uslug w datagrid
        /// </summary>
        /// <param name="pole">podaj nazwe textbox z wynikiem wyszukiwania</param>
        /// <param name="siatka">podaj nazwe datagrid gdzie dane maja zostac umieszczone</param>
        private void SzukajUslugi(TextBox pole, DataGrid siatka)
        {
            var szukaneUslugi = (from u in db.uslugi
                                 where u.nazwa.Contains(pole.Text)
                                 select u).ToList();

            siatka.ItemsSource = szukaneUslugi;
        }
        /// <summary>
        /// funkcja wyszukuje uslugi pracownika i umieszcza liste uslug w datagrid
        /// </summary>
        /// <param name="idPracownika">podaj id pracownika dla ktorego maja byc odszukane uslugi</param>
        /// <param name="siatka">podaj nazwe datagrid gdzie dane maja zostac umieszczone</param>
        private void SzukajUslugiPracownika(int idPracownika, DataGrid siatka)
        {
            var uslugiPracownika = (from uu in db.uzytkownik_usluga
                                    join u in db.uslugi on uu.usluga_id equals u.usluga_id
                                    where uu.uzytkownik_id == idPracownika
                                    select new PracownikUsluga
                                    {
                                        uzytkownik_sluga_id = uu.uzytkownik_usluga_id,
                                        uzytkownik_id = uu.uzytkownik_id,
                                        usluga_id = uu.usluga_id,
                                        opis = u.opis,
                                        nazwa = u.nazwa,
                                        czas = u.czas,
                                        cena = u.cena
                                    }
                            );

            siatka.ItemsSource = uslugiPracownika.ToList();
        }
        /// <summary>
        /// funkcja wyszukuje klientow i umieszcza liste pracownikow w datagrid
        /// </summary>
        /// <param name="pole">podaj nazwe textbox z wynikiem wyszukiwania</param>
        /// <param name="siatka">podaj nazwe datagrid gdzie dane maja zostac umieszczone</param>
        private void SzukajKlientow(TextBox pole, DataGrid siatka)
        {
            var szukaniKlienci = (from u in db.klienci
                                  where ((u.nazwisko + " " + u.imie).Contains(pole.Text)) || ((u.imie + " " + u.nazwisko).Contains(pole.Text))
                                  select u).ToList();

            siatka.ItemsSource = szukaniKlienci;
        }
        #endregion
        #region tabKlienci
        #region FunckjePomocnicze

        private void WyczyscKlientow()
        {
            TxtKemail.Text = "";
            TxtKimie.Text = "";
            TxtKkodPocztowy.Text = "";
            TxtKmiejscowosc.Text = "";
            TxtKnazwisko.Text = "";
            TxtKNrDomu.Text = "";
            TxtKSzukaj.Text = "";
            TxtKtelefon.Text = "";
            TxtKulica.Text = "";
        }
        private void ZaladujListeKlientow()
        {
            DgKklienci.ItemsSource = db.klienci.ToList();
            DgKklienci.Columns[0].Visibility = Visibility.Hidden;
            DgKklienci.Columns[9].Visibility = Visibility.Hidden;
        }
        #endregion

        private void BtnKSzukaj_Click(object sender, RoutedEventArgs e)
        {
            SzukajKlientow(TxtKSzukaj, DgKklienci);
            DgKklienci.Columns[0].Visibility = Visibility.Hidden;
            DgKklienci.Columns[9].Visibility = Visibility.Hidden;
        }

        private void BtnKdodaj_Click(object sender, RoutedEventArgs e)
        {
            var nowyKlient = new klienci()
            {
                imie = TxtKimie.Text,
                email = TxtKemail.Text,
                kodPocztowy = TxtKkodPocztowy.Text,
                miejscowosc = TxtKmiejscowosc.Text,
                modyfikacja = DateTime.Now,
                nazwisko = TxtKnazwisko.Text,
                numerDomu = TxtKNrDomu.Text,
                telefon = TxtKtelefon.Text,
                ulica = TxtKulica.Text
            };

            db.klienci.Add(nowyKlient);

            try
            {
                db.SaveChanges();
                ZaladujListeKlientow();
                WyczyscKlientow();
                MessageBox.Show("Klient został dodany.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd podczas dodawania klienta. Spróbuj później.\n" + ex.Message, "Informacja", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgKklienci_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DgKklienci.SelectedIndex >= 0)
            {
                var zaznaczonyKlient = (klienci)DgKklienci.SelectedItem;

                TbKid.Text = zaznaczonyKlient.klienci_id.ToString();
                TxtKemail.Text = zaznaczonyKlient.email;
                TxtKimie.Text = zaznaczonyKlient.imie;
                TxtKkodPocztowy.Text = zaznaczonyKlient.kodPocztowy;
                TxtKmiejscowosc.Text = zaznaczonyKlient.miejscowosc;
                TxtKnazwisko.Text = zaznaczonyKlient.nazwisko;
                TxtKNrDomu.Text = zaznaczonyKlient.numerDomu;
                TxtKtelefon.Text = zaznaczonyKlient.telefon;
                TxtKulica.Text = zaznaczonyKlient.ulica;

                BtnKmodyfikuj.IsEnabled = true;
                BtnKusun.IsEnabled = true;
            }
        }

        private void BtnKmodyfikuj_Click(object sender, RoutedEventArgs e)
        {
            if (DgKklienci.SelectedIndex >= 0)
            {//sprawdzamy czy klient zostal zaznaczony
                if (string.IsNullOrEmpty(TbKid.Text))
                {//jezeli id klienta jest puste
                    MessageBox.Show("Wybierz poprawnie klienta do edycji.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {//klient zostal wybrany
                    int idklienta = Convert.ToInt32(TbKid.Text);
                    var zaznaczonyKlient = db.klienci.FirstOrDefault(k => k.klienci_id == idklienta);

                    if (zaznaczonyKlient != null)
                    {//sprawdzamy czy klient zostal odnaleziony w bazie
                        zaznaczonyKlient.email = TxtKemail.Text;
                        zaznaczonyKlient.imie = TxtKimie.Text;
                        zaznaczonyKlient.kodPocztowy = TxtKkodPocztowy.Text;
                        zaznaczonyKlient.miejscowosc = TxtKmiejscowosc.Text;
                        zaznaczonyKlient.modyfikacja = DateTime.Now;
                        zaznaczonyKlient.nazwisko = TxtKnazwisko.Text;
                        zaznaczonyKlient.numerDomu = TxtKNrDomu.Text;
                        zaznaczonyKlient.telefon = TxtKtelefon.Text;
                        zaznaczonyKlient.ulica = TxtKulica.Text;

                        db.Entry(zaznaczonyKlient).State = EntityState.Modified;

                        try
                        {
                            db.SaveChanges();
                            ZaladujListeKlientow();
                            WyczyscKlientow();
                            MessageBox.Show("Dane zostały zaktualizowane.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                            BtnKmodyfikuj.IsEnabled = false;
                            BtnKusun.IsEnabled = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wystąpił błąd podczas aktualizacji. Spróbuj później.\n" + ex.Message, "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nie odnaleziono klienta. Spróbuj później.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void BtnKusun_Click(object sender, RoutedEventArgs e)
        {
            if (DgKklienci.SelectedIndex >= 0)
            {
                if (string.IsNullOrEmpty(TbKid.Text))
                {
                    MessageBox.Show("Wybierz poprawnie klienta do usunięcia.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    int idklienta = Convert.ToInt32(TbKid.Text);
                    var zaznaczonyKlient = db.klienci.FirstOrDefault(k => k.klienci_id == idklienta);

                    if (zaznaczonyKlient != null)
                    {
                        db.klienci.Remove(zaznaczonyKlient);

                        try
                        {
                            db.SaveChanges();
                            ZaladujListeKlientow();
                            WyczyscKlientow();
                            MessageBox.Show("Klient został usunięty.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                            BtnKmodyfikuj.IsEnabled = false;
                            BtnKusun.IsEnabled = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wystąpił błąd podczas usuwania. Spróbuj później.\n" + ex.Message, "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nie odnaleziono klienta. Spróbuj później.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        #endregion
        #region Wizyty
        //wyszukuje uslugi dla karty rezerwacja
        private void BtnRSzukajU_Click(object sender, RoutedEventArgs e)
        {
            SzukajUslugi(TxtRSzukajU, DgRuslugi);
            DgRuslugi.Columns[0].Visibility = Visibility.Hidden;
        }

        //wyszukuje klientow dla karty rezerwacja
        private void BtnRSzukajK_Click(object sender, RoutedEventArgs e)
        {
            SzukajKlientow(TxtRSzukajK, DgRklienci);
        }

        //wyszukuje pracownikow dla karty rezerwacja
        private void BtnRSzukajP_Click(object sender, RoutedEventArgs e)
        {
            SzukajPracownikow(TxtRSzukajP, DgRpracownicy, true);
        }

        //wypisuje do textbox wybrana usluge z datagird
        private void DgRuslugi_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //Rezerwacja wybranie uslugi
            if (DgRuslugi.SelectedIndex >= 0)
            {
                var zaznaczonaUsluga = (uslugi)DgRuslugi.SelectedItem;

                TxtRUslugaW.Text = zaznaczonaUsluga.nazwa;
                _uslugaId = Convert.ToInt32(zaznaczonaUsluga.usluga_id);

                var listaPracownikow = (from uz in db.uzytkownik
                                        join uu in db.uzytkownik_usluga on uz.uzytkownik_id equals uu.uzytkownik_id
                                        join us in db.uslugi on uu.usluga_id equals us.usluga_id
                                        where us.usluga_id == _uslugaId
                                        select uz
                 ).ToList();
                //usuwamy zaznaczone pole zeby zaladowac nowa liste
                DgRuslugi.UnselectAll();
                DgRpracownicy.ItemsSource = listaPracownikow;
            }
        }

        //wypisuje do textbox wybranego klienta z siatki datagrid
        private void DgRklienci_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //Rezerwacja wybranie klienta
            if (DgRklienci.SelectedIndex >= 0)
            {
                var zaznaczonyKlient = (klienci)DgRklienci.SelectedItem;

                TxtRKlientW.Text = zaznaczonyKlient.imie + " " + zaznaczonyKlient.nazwisko;
                _klientId = Convert.ToInt32(zaznaczonyKlient.klienci_id);

            }
        }

        //wypisuje godziny pracy pracownika dla zaznaczonego pracownika w datagrid
        private void DgRpracownicy_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            CalendarR.IsEnabled = true;
            PokazGodzinyPracyPracownika();
        }

        //wypisuje zaznaczona date z kalendarza do textbox
        private void CalendarR_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            //sprawdzamy czy zostala wybrana data na kalendarzu
            if (CalendarR.SelectedDate.HasValue)
            {
                //obcinamy wybrana date z czasem do postaci samej daty
                _dataW = (CalendarR.SelectedDate.Value).ToShortDateString();
                TxtRTerminW.Text = _dataW;

                //odswieza godziny dla wybranego pracownika
                PokazGodzinyPracyPracownika();
            }
        }

        //dodajemy wizyte
        private void BtnRDodaj_Click(object sender, RoutedEventArgs e)
        {
            //sprawdzamy czy dane do wizyty zostaly poprawnie wybrane
            if (_klientId <= 0 || _pracownikId <= 0 || _uslugaId <= 0 || TxtRTerminW.Text.Length < 14)
                MessageBox.Show("Uzupełnij poprawnie dane", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                uzytkownik uzytkownik = db.uzytkownik.Single(u => u.uzytkownik_id == _pracownikId);
                uslugi usluga = db.uslugi.Single(u => u.usluga_id == _uslugaId);
                klienci klient = db.klienci.Single(k => k.klienci_id == _klientId);

                string daneUzytkownika = uzytkownik.imie + " " + uzytkownik.nazwisko;
                string daneUslugi = usluga.nazwa;
                string daneKlienta = klient.imie + " " + klient.nazwisko;

                string wiadomosc =
                    string.Format("Czy chcesz dodać wizytę dla:\n\nUżytkownik: {0}\nKlient: {1}\nUsługa: {2} \nTermin: {3} - {4}", daneUzytkownika, daneKlienta, daneUslugi, Convert.ToDateTime(TxtRTerminW.Text), Convert.ToDateTime(TxtRTerminW.Text));

                MessageBoxResult result = MessageBox.Show(wiadomosc, "Podsumowanie wizyty", MessageBoxButton.YesNo, MessageBoxImage.Information);

                //sprawdzamy czy chcemy dodac usluge
                if (MessageBoxResult.Yes == result)
                {
                    wizyty wizyta = new wizyty()
                    {
                        klienci_id = _klientId,
                        uslugi_id = _uslugaId,
                        uzytkownik_id = _pracownikId,
                        rezerwacja_od = Convert.ToDateTime(TxtRTerminW.Text),
                        rezerwacja_do = Convert.ToDateTime(TxtRTerminW.Text),
                        status_id = 1,
                        modyfikacja = DateTime.Now
                    };

                    db.wizyty.Add(wizyta);
                    db.SaveChanges();

                    PokazGodzinyPracyPracownika();
                    MessageBox.Show("Rezerwacja została dodana", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        //Funkcja wypisuje godziny pracy pracownika
        private void PokazGodzinyPracyPracownika()
        {
            //zmienna bedzie przechowywala date zaznaczona z kalendarza i czas co 30 min np. 2012-04-30 12:30:00
            string czas;

            //zmienna bedzie przechowywala czas pracy pracownika np. od godz. 8
            int praca_od = 0;

            //zmienna bedzie przechowywala czas pacy pracownika np. do godz. 15
            int praca_do = 0;

            //ustawiamy od nowa pole tekstowe na zaznaczona date
            TxtRTerminW.Text = _dataW;

            //konwertujemy wybrana date z kalendarza do postaci daty i czasu
            DateTime dzienGodzina = Convert.ToDateTime(_dataW);

            //wyciagamy z daty i czasu jaki to dzien tygodnia
            DayOfWeek dzien = dzienGodzina.DayOfWeek;

            WlaczWylaczPrzyciski(false);

            //jezeli datagrid z pracownikami zostal zaznaczonz
            if (DgRpracownicy.SelectedIndex >= 0)
            {
                uzytkownik zaznaczonyPracownik = (uzytkownik)DgRpracownicy.SelectedItem;
                _pracownikId = zaznaczonyPracownik.uzytkownik_id;

                //lista uslug wykonywanych przez pracownika
                var listaUslug = (from us in db.uslugi
                                  join uu in db.uzytkownik_usluga on us.usluga_id equals uu.usluga_id
                                  where uu.uzytkownik_id == _pracownikId
                                  select us
                                 ).ToList();

                //usuwamy zaznaczone pole zeby zaladowac nowa liste
                //DgRpracownicy.UnselectAll();
                DgRuslugi.ItemsSource = listaUslug;

                //wyszukujemy godziny pracy dla zaznaczonego pracownika
                godziny godzinyPracownika = db.godziny.Single(p => p.uzytkownik_id.Equals(_pracownikId));

                switch (dzien)
                {//wybieramy odpowiedni dzien tygodnia dla zaznaczonego pracownika i przypisujemy godziny pracy od-do
                    case DayOfWeek.Monday:
                        //sprawdzamy czy przypadkiem pole nie jest null jesli tak to dajemy wpisujemy godzine na 0 w przeciwnym wypadku odczytujemy godziny
                        praca_od = godzinyPracownika.pon_od != null ? godzinyPracownika.pon_od.Value.Hours : 0;
                        praca_do = godzinyPracownika.pon_do != null ? godzinyPracownika.pon_do.Value.Hours : 0;
                        break;
                    case DayOfWeek.Tuesday:
                        praca_od = godzinyPracownika.wt_od != null ? godzinyPracownika.wt_od.Value.Hours : 0;
                        praca_do = godzinyPracownika.wt_do != null ? godzinyPracownika.wt_do.Value.Hours : 0;
                        break;
                    case DayOfWeek.Wednesday:
                        praca_od = godzinyPracownika.sr_od != null ? godzinyPracownika.sr_od.Value.Hours : 0;
                        praca_do = godzinyPracownika.sr_do != null ? godzinyPracownika.sr_do.Value.Hours : 0;
                        break;
                    case DayOfWeek.Thursday:
                        praca_od = godzinyPracownika.czw_od != null ? godzinyPracownika.czw_od.Value.Hours : 0;
                        praca_do = godzinyPracownika.czw_do != null ? godzinyPracownika.czw_do.Value.Hours : 0;
                        break;
                    case DayOfWeek.Friday:
                        praca_od = godzinyPracownika.pt_od != null ? godzinyPracownika.pt_od.Value.Hours : 0;
                        praca_do = godzinyPracownika.pt_do != null ? godzinyPracownika.pt_do.Value.Hours : 0;
                        break;
                    case DayOfWeek.Saturday:
                        praca_od = godzinyPracownika.sb_od != null ? godzinyPracownika.sb_od.Value.Hours : 0;
                        praca_do = godzinyPracownika.sb_do != null ? godzinyPracownika.sb_do.Value.Hours : 0;
                        break;
                    case DayOfWeek.Sunday:
                        praca_od = godzinyPracownika.nd_od != null ? godzinyPracownika.nd_od.Value.Hours : 0;
                        praca_do = godzinyPracownika.nd_do != null ? godzinyPracownika.nd_do.Value.Hours : 0;
                        break;
                }
            }

            //usuwamy pola ze stackpanelu
            GbRgodziny.Children.Clear();

            //Rysujemy TextBox dla godzin pracy pracownika
            for (int g = praca_od; g < praca_do; g++)
                for (int m = 0; m < 60; m += 30)
                {
                    // do daty zaznaczonej w kalendarzu _dataW dopisujemy czas pracy co 30 min
                    czas = _dataW + " " + g + ":" + m + ":00";

                    //czas konwertujemy do postaci data i czas
                    DateTime godzina_r = Convert.ToDateTime(czas);

                    //tworzymy nowego textboxa
                    TextBox poleGodziny = new TextBox();

                    //dodajemy textboxa do stackpanelu
                    GbRgodziny.Children.Add(poleGodziny);

                    //ustawiamy szerowkosc dla textboxa
                    poleGodziny.Width = 180;

                    var informacjeOterminach = (
                        from w in db.wizyty
                        join uz in db.uzytkownik on w.uzytkownik_id equals uz.uzytkownik_id
                        join u in db.uslugi on w.uslugi_id equals u.usluga_id
                        join k in db.klienci on w.klienci_id equals k.klienci_id
                        where uz.uzytkownik_id == _pracownikId && w.rezerwacja_od == godzina_r
                        select new
                        {
                            idWizyty = w.wizyty_id,
                            idUzytkownika = uz.uzytkownik_id,
                            idUslugi = u.usluga_id,
                            daneUzytkownika = k.imie + " " + k.nazwisko,
                            usluga = u.nazwa
                        }
                        ).FirstOrDefault();

                    poleGodziny.Background = Brushes.White;

                    //konwertujemy uzyskana powyzej godzine i czas do stringa
                    poleGodziny.Text = string.Format(godzina_r.ToShortTimeString());

                    if (informacjeOterminach != null)
                    {
                        poleGodziny.Background = Brushes.Red;
                        poleGodziny.Text += " " + informacjeOterminach.daneUzytkownika + " " + informacjeOterminach.usluga;
                        poleGodziny.Tag = informacjeOterminach.idWizyty;
                    }

                    //ustawiam margines gorny
                    poleGodziny.Margin = new Thickness(0, 5, 0, 0);

                    //ustawiamy pozycje textbox + wysokosc x kolejna wysokosc textboxa
                    poleGodziny.PointToScreen(new Point(10, 20 + 25 * g));

                    //rejestrujemy zdarzenie do przycisku, kiedy uzytkownik zaznaczy textbox 
                    //zostaje wywolana funkcja i pobieramy informacje z zaznaczonego pola
                    poleGodziny.SelectionChanged += TxtRTerminW_SelectionChanged;

                    //rejestrujemy zdarzenie do przycisku modyfikacji
                    poleGodziny.MouseDoubleClick += TxtRTerminWModyfikacja_MouseDoubleClick;
                }
        }

        //wypisuje zaznaczona godzine pracownika z textbox i przepisuje do textbox z terminem wizyty 
        private void TxtRTerminW_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //konwertuje obiekt do textboxa aby wyciagnac informacje o kliknietym przycisku
            TextBox pole = (TextBox)sender;
            _godzinaW = pole.Text;
            //laczymy date z godzina i wpisujemy do textbox
            TxtRTerminW.Text = _dataW + " " + _godzinaW;

            _wizytaId = Convert.ToInt32(pole.Tag);

            WlaczWylaczPrzyciski(false);
        }

        //odczytuje informacje o wizycie do modyfikacji
        private void TxtRTerminWModyfikacja_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox wizytaModyfikacja = (TextBox)sender;

            //aktywacja przyciskow modyfikuj i usun
            if (wizytaModyfikacja.Background.Equals(Brushes.Red))
                WlaczWylaczPrzyciski(true);


            try
            {
                //odczytuje idWizyty
                int idWizytyMod = Convert.ToInt32(wizytaModyfikacja.Tag);

                var wizytaMod = (from w in db.wizyty
                                 join u in db.uslugi on w.uslugi_id equals u.usluga_id
                                 join uz in db.uzytkownik on w.uzytkownik_id equals uz.uzytkownik_id
                                 join k in db.klienci on w.klienci_id equals k.klienci_id
                                 where w.wizyty_id == idWizytyMod
                                 select new
                                 {
                                     uzytkownikId = w.uzytkownik_id,
                                     uslugaId = w.uslugi_id,
                                     klientId = w.klienci_id,
                                     nazwaUslugi = u.nazwa,
                                     daneKlienta = k.imie + " " + k.nazwisko,
                                     dataWizyty = w.rezerwacja_od
                                 }
                    ).FirstOrDefault();

                //sprawdzamy czy wizyta istnieje
                if (wizytaMod != null)
                {
                    _klientId = wizytaMod.klientId.Value;
                    _uslugaId = wizytaMod.uslugaId.Value;
                    _klientId = wizytaMod.klientId.Value;
                    TxtRTerminW.Text = wizytaMod.dataWizyty.ToString();
                    TxtRKlientW.Text = wizytaMod.daneKlienta;
                    TxtRUslugaW.Text = wizytaMod.nazwaUslugi;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił błąd podczas odczytu wizyty. Spróbuj później", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        //modyfikuje wizyte
        private void BtnRModyfikuj_Click(object sender, RoutedEventArgs e)
        {
            wizyty wizyta = db.wizyty.FirstOrDefault(w => w.wizyty_id == _wizytaId);

            if (wizyta != null)
            {
                wizyta.klienci_id = _klientId;
                wizyta.uslugi_id = _uslugaId;
                wizyta.uzytkownik_id = _pracownikId;
                wizyta.modyfikacja = DateTime.Now;

                db.Entry(wizyta).State = EntityState.Modified;
                db.SaveChanges();

                PokazGodzinyPracyPracownika();
                MessageBox.Show("Wizyta została zmodyfikowana", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Nie można teraz zmodyfikować wizyty. Spróbuj później", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        //usuwa zaznaczona wizyte
        private void BtnRUsuń_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć wizytę?", "Informacja", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (MessageBoxResult.Yes == result)
            {
                var wizyta = db.wizyty.FirstOrDefault(w => w.wizyty_id == _wizytaId);

                try
                {
                    db.wizyty.Remove(wizyta);
                    db.SaveChanges();

                    MessageBox.Show("Wizyta została usunięta", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);

                    PokazGodzinyPracyPracownika();
                }
                catch (Exception)
                {
                    MessageBox.Show("Wystąpił błąd podczas usuwania. Spróbuj poźniej", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Funkcja wlacza i wylacza przyciski do modyfikacji i usuwania pol w wizytach
        /// </summary>
        /// <param name="wlWy">true - wlacza, false - wylacza</param>
        private void WlaczWylaczPrzyciski(bool wlWy)
        {
            BtnRModyfikuj.IsEnabled = wlWy;
            BtnRUsuń.IsEnabled = wlWy;
        }
        #endregion //Wizyta
        #region Menu
        private void BtnMenuKlienci_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 1;
        }
        private void BtnMenuPracownicy_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 4;
        }

        private void BtnMenuRezerwacja_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 0;
        }

        private void BtnMenuUslugi_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 3;
        }

        private void BtnMenuUslugiPracownicy_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 2;
        }

        private void BtnMenuUstawienia_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 5;
        }
        #endregion //Menu
    }
}
