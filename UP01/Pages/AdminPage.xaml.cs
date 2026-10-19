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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();

            SortComboBox.SelectedIndex = 0;

            LoadAdmin();
            LoadClients();
            LoadVehicles();
            LoadOrders();
            LoadStatistics();
        }


        private void LoadAdmin()
        {
            if (AuthService.CurrentUser == null)
                return;

            AdminNameTextBlock.Text =
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


        // =========================
        // ПОИСК
        // =========================

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


            if (AdminTabControl.SelectedIndex == 0)
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


            else if (AdminTabControl.SelectedIndex == 1)
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


            else if (AdminTabControl.SelectedIndex == 2)
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


        // =========================
        // СОРТИРОВКА
        // =========================

        private void SortComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (!IsInitialized)
                return;

            ApplySearch();
        }


        // =========================
        // ВКЛАДКИ
        // =========================

        private void AdminTabControl_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (!IsInitialized)
                return;

            SearchTextBox.Clear();
            SortComboBox.SelectedIndex = 0;
        }


        // =========================
        // СБРОС
        // =========================

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


        // =========================
        // СТАТИСТИКА
        // =========================

        private void LoadStatistics()
        {
            int clients =
                Core.Core.Context.clients.Count();

            int vehicles =
                Core.Core.Context.vehicles.Count();

            int orders =
                Core.Core.Context.work_orders.Count();

            StatisticsTextBlock.Text =
                "Клиентов: " + clients +
                "\nАвтомобилей: " + vehicles +
                "\nЗаказов: " + orders;
        }


        private void RefreshStatisticsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoadStatistics();

            MessageBox.Show(
                "Статистика обновлена.",
                "Статистика",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================
        // CRUD - ПОКА ЗАГЛУШКИ
        // =========================

        private void AddClientButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Окно добавления клиента будет подключено следующим модулем.");
        }


        private void EditClientButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Окно редактирования клиента будет подключено следующим модулем.");
        }


        private void DeleteClientButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Удаление клиента будет подключено следующим модулем.");
        }


        private void AddVehicleButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Окно добавления автомобиля будет подключено следующим модулем.");
        }


        private void EditVehicleButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Окно редактирования автомобиля будет подключено следующим модулем.");
        }


        private void DeleteVehicleButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Удаление автомобиля будет подключено следующим модулем.");
        }


        private void AddOrderButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Окно добавления заказа будет подключено следующим модулем.");
        }


        private void EditOrderButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Окно редактирования заказа будет подключено следующим модулем.");
        }


        private void DeleteOrderButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Удаление заказа будет подключено следующим модулем.");
        }


        // =========================
        // СМЕНА ПАРОЛЯ
        // =========================

        private void ChangePasswordButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new ChangePasswordPage());
        }


        // =========================
        // ВЫХОД
        // =========================

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


        // =========================
        // ОТЧЁТЫ - ПОКА ЗАГЛУШКИ
        // =========================

        private void ExportDocxButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Экспорт DOCX будет подключён следующим модулем.");
        }


        private void ExportPdfButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Экспорт PDF будет подключён следующим модулем.");
        }


        private void ExportXlsxButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Экспорт XLSX будет подключён следующим модулем.");
        }
    }
}

