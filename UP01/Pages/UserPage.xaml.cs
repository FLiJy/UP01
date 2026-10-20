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

            SortComboBox.SelectedIndex = 0;

            LoadUser();
            LoadClients();
            LoadVehicles();
            LoadOrders();
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

        private void LoadVehicles()
        {
            VehiclesDataGrid.ItemsSource =
                Core.Core.Context.vehicles
                    .OrderBy(x => x.make)
                    .ToList();
        }

        private void LoadOrders()
        {
            OrdersDataGrid.ItemsSource =
                Core.Core.Context.work_orders
                    .OrderByDescending(x => x.start_date)
                    .ToList();
        }


        private void SearchTextBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            ApplySearch();
        }


        private void ApplySearch()
        {
            string search =
                SearchTextBox.Text.Trim().ToLower();

            if (DataTabControl.SelectedIndex == 0)
            {
                var clients =
                    Core.Core.Context.clients.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    clients = clients.Where(x =>
                        x.first_name.ToLower().Contains(search) ||
                        x.last_name.ToLower().Contains(search) ||
                        x.phone.ToLower().Contains(search) ||
                        x.email.ToLower().Contains(search));
                }

                if (SortComboBox.SelectedIndex == 1)
                {
                    clients = clients
                        .OrderBy(x => x.first_name);
                }
                else
                {
                    clients = clients
                        .OrderBy(x => x.last_name);
                }

                ClientsDataGrid.ItemsSource =
                    clients.ToList();
            }

            else if (DataTabControl.SelectedIndex == 1)
            {
                var vehicles =
                    Core.Core.Context.vehicles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    vehicles = vehicles.Where(x =>
                        x.vin.ToLower().Contains(search) ||
                        x.make.ToLower().Contains(search) ||
                        x.model.ToLower().Contains(search) ||
                        x.license_plate.ToLower().Contains(search));
                }

                if (SortComboBox.SelectedIndex == 1)
                {
                    vehicles = vehicles
                        .OrderBy(x => x.make);
                }
                else if (SortComboBox.SelectedIndex == 2)
                {
                    vehicles = vehicles
                        .OrderBy(x => x.model);
                }
                else
                {
                    vehicles = vehicles
                        .OrderBy(x => x.vehicle_id);
                }

                VehiclesDataGrid.ItemsSource =
                    vehicles.ToList();
            }

            else if (DataTabControl.SelectedIndex == 2)
            {
                var orders =
                    Core.Core.Context.work_orders.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    orders = orders.Where(x =>
                        x.status.ToLower().Contains(search));
                }

                if (SortComboBox.SelectedIndex == 1)
                {
                    orders = orders
                        .OrderBy(x => x.status);
                }
                else
                {
                    orders = orders
                        .OrderByDescending(x => x.start_date);
                }

                OrdersDataGrid.ItemsSource =
                    orders.ToList();
            }
        }

        private void SortComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (!IsInitialized)
                return;

            ApplySearch();
        }

        private void DataTabControl_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (!IsInitialized)
                return;

            SearchTextBox.Clear();

            SortComboBox.SelectedIndex = 0;
        }

        private void ResetButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SearchTextBox.Clear();

            SortComboBox.SelectedIndex = 0;

            LoadClients();
            LoadVehicles();
            LoadOrders();
        }

        private void ChangePasswordButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new ChangePasswordPage());
        }

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

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ToggleTheme();
        }
    }
}
