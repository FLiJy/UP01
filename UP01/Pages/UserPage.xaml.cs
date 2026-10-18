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
using UP01.Core;

namespace UP01.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();

            LoadUser();
            LoadClients();
        }

        private void LoadUser()
        {
            if (AuthService.CurrentUser == null)
                return;

            UserNameTextBlock.Text =
                AuthService.CurrentUser.first_name
                + " "
                + AuthService.CurrentUser.last_name;
        }

        private void LoadClients()
        {
            ClientsDataGrid.ItemsSource =
                Core.Core.Context.clients
                    .OrderBy(x => x.last_name)
                    .ToList();
        }


        // ==============================
        // ПОИСК
        // ==============================

        private void SearchTextBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string search =
                SearchTextBox.Text.Trim().ToLower();

            var clients =
                Core.Core.Context.clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                clients = clients.Where(x =>
                    x.first_name.ToLower().Contains(search) ||
                    x.last_name.ToLower().Contains(search) ||
                    x.phone.Contains(search));
            }

            // Сортировка
            if (SortComboBox.SelectedIndex == 1)
            {
                clients = clients
                    .OrderBy(x => x.first_name);
            }
            else if (SortComboBox.SelectedIndex == 2)
            {
                clients = clients
                    .OrderBy(x => x.phone);
            }
            else
            {
                clients = clients
                    .OrderBy(x => x.last_name);
            }

            ClientsDataGrid.ItemsSource =
                clients.ToList();
        }


        // ==============================
        // СОРТИРОВКА
        // ==============================

        private void SortComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (IsInitialized)
                ApplyFilter();
        }


        // ==============================
        // СБРОС
        // ==============================

        private void ResetButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SearchTextBox.Clear();

            SortComboBox.SelectedIndex = 0;

            LoadClients();
        }


        // ==============================
        // ВЫБОР КЛИЕНТА
        // ==============================

        private void ClientsDataGrid_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem == null)
                return;

            var client =
                ClientsDataGrid.SelectedItem as clients;

            if (client == null)
                return;

            // Позже здесь откроем страницу
            // автомобилей выбранного клиента.
        }


        // ==============================
        // СМЕНА ПАРОЛЯ
        // ==============================

        private void ChangePasswordButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new ChangePasswordPage());
        }


        // ==============================
        // ВЫХОД
        // ==============================

        private void LogoutButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show(
                    "Вы действительно хотите выйти?",
                    "Выход",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            AuthService.Logout();

            NavigationService.Navigate(
                new LoginPage());
        }
    }
}
