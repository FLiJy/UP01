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
            LoadStatistics();
            ApplySearch();
        }

        private void LoadUser()
        {
            if (AuthService.CurrentUser == null)
            {
                UserNameTextBlock.Text = "Пользователь";
                return;
            }

            UserNameTextBlock.Text =
                AuthService.CurrentUser.first_name + " " +
                AuthService.CurrentUser.last_name;
        }

        private void LoadStatistics()
        {
            var vehicles = Core.Core.Context.vehicles.ToList();
            var orders = Core.Core.Context.work_orders.ToList();

            VehiclesCountTextBlock.Text =
                vehicles.Count.ToString();

            OrdersCountTextBlock.Text =
                orders.Count.ToString();

            CompletedOrdersCountTextBlock.Text =
                orders.Count(x =>
                    x.status != null &&
                    x.status.ToLower() == "завершён").ToString();
        }

        private void ApplySearch()
        {
            string search = SearchTextBox.Text.Trim().ToLower();
            int tabIndex = DataTabControl.SelectedIndex;
            int sortIndex = SortComboBox.SelectedIndex;

            if (tabIndex == 0)
            {
                var vehicles =
                    Core.Core.Context.vehicles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    vehicles = vehicles.Where(x =>
                        (x.make != null &&
                         x.make.ToLower().Contains(search)) ||
                        (x.model != null &&
                         x.model.ToLower().Contains(search)) ||
                        (x.vin != null &&
                         x.vin.ToLower().Contains(search)) ||
                        (x.license_plate != null &&
                         x.license_plate.ToLower().Contains(search)));
                }

                if (sortIndex == 1)
                {
                    vehicles = vehicles.OrderBy(x => x.make);
                }
                else if (sortIndex == 2)
                {
                    vehicles = vehicles.OrderByDescending(x => x.make);
                }
                else
                {
                    vehicles = vehicles.OrderBy(x => x.vehicle_id);
                }

                VehiclesDataGrid.ItemsSource = vehicles.ToList();
            }
            else if (tabIndex == 1)
            {
                var orders =
                    Core.Core.Context.work_orders.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    orders = orders.Where(x =>
                        (x.status != null &&
                         x.status.ToLower().Contains(search)) ||
                        x.order_id.ToString().Contains(search) ||
                        x.vehicle_id.ToString().Contains(search));
                }

                if (sortIndex == 1)
                {
                    orders = orders.OrderBy(x => x.start_date);
                }
                else if (sortIndex == 2)
                {
                    orders = orders.OrderByDescending(x => x.start_date);
                }
                else
                {
                    orders = orders.OrderByDescending(x => x.start_date);
                }

                OrdersDataGrid.ItemsSource = orders.ToList();
            }
        }

        private void SearchTextBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (IsInitialized)
                ApplySearch();
        }

        private void SortComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (IsInitialized)
                ApplySearch();
        }

        private void DataTabControl_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (!IsInitialized ||
                SearchTextBox == null ||
                SortComboBox == null)
                return;

            SearchTextBox.Clear();
            SortComboBox.SelectedIndex = 0;

            ApplySearch();
        }

        private void ResetButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SearchTextBox.Clear();
            SortComboBox.SelectedIndex = 0;

            LoadStatistics();
            ApplySearch();
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
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите выйти?",
                "Выход",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            AuthService.Logout();

            NavigationService.Navigate(new LoginPage());
        }

        private void ThemeButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ThemeManager.ToggleTheme();
        }
    }
}