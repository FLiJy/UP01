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
using UP01.Windows;
using Xceed.Words.NET;
using Xceed.Document.NET;
using Xceed;

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
            ClientEditWindow window =
                new ClientEditWindow();

            if (window.ShowDialog() == true)
            {
                LoadClients();
            }
        }


        private void EditClientButton_Click(
         object sender,
         RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите клиента для редактирования.",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            ClientEditWindow window =
                new ClientEditWindow(
                    ClientsDataGrid.SelectedItem);

            if (window.ShowDialog() == true)
            {
                LoadClients();
            }
        }


        private void DeleteClientButton_Click(
     object sender,
     RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите клиента для удаления.",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            dynamic client =
                ClientsDataGrid.SelectedItem;

            MessageBoxResult result =
                MessageBox.Show(
                    "Вы действительно хотите удалить клиента "
                    + client.first_name + " "
                    + client.last_name
                    + "?\n\n"
                    + "Также будут удалены связанные автомобили и заказы.",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                Core.Core.Context.clients.Remove(client);

                Core.Core.Context.SaveChanges();

                LoadClients();
                LoadVehicles();
                LoadOrders();
                LoadStatistics();

                MessageBox.Show(
                    "Клиент успешно удалён.",
                    "Готово",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось удалить клиента.\n\n"
                    + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private void AddVehicleButton_Click(
        object sender,
        RoutedEventArgs e)
            {
                VehicleEditWindow window =
                new VehicleEditWindow();

            if (window.ShowDialog() == true)
            {
                LoadVehicles();
                LoadStatistics();
            }
        }


        private void EditVehicleButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (VehiclesDataGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите автомобиль для редактирования.",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            vehicles vehicle =
                VehiclesDataGrid.SelectedItem
                as vehicles;

            if (vehicle == null)
                return;

            VehicleEditWindow window =
                new VehicleEditWindow(vehicle);

            if (window.ShowDialog() == true)
            {
                LoadVehicles();
            }
        }


        private void DeleteVehicleButton_Click(
        object sender,
        RoutedEventArgs e)
            {
            if (VehiclesDataGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите автомобиль для удаления.",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            vehicles vehicle =
                VehiclesDataGrid.SelectedItem
                as vehicles;

            if (vehicle == null)
                return;

            MessageBoxResult result =
                MessageBox.Show(
                    "Вы действительно хотите удалить автомобиль "
                    + vehicle.make + " "
                    + vehicle.model
                    + "?\n\n"
                    + "Связанные заказы также будут удалены.",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                Core.Core.Context.vehicles.Remove(vehicle);

                Core.Core.Context.SaveChanges();

                LoadVehicles();
                LoadOrders();
                LoadStatistics();

                MessageBox.Show(
                    "Автомобиль успешно удалён.",
                    "Готово",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось удалить автомобиль.\n\n"
                    + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private void AddOrderButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            OrderEditWindow window = new OrderEditWindow();

            if (window.ShowDialog() == true)
            {
                LoadOrders();
            }
        }


        private void EditOrderButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите заказ.",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            work_orders order =
                OrdersDataGrid.SelectedItem
                as work_orders;

            if (order == null)
                return;


            OrderEditWindow window =
                new OrderEditWindow(order);

            if (window.ShowDialog() == true)
            {
                LoadOrders();
            }
        }


        private void DeleteOrderButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите заказ.",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            work_orders order =
                OrdersDataGrid.SelectedItem
                as work_orders;

            if (order == null)
                return;


            MessageBoxResult result =
                MessageBox.Show(
                    "Вы действительно хотите удалить заказ №"
                    + order.order_id
                    + "?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;


            try
            {
                Core.Core.Context.work_orders.Remove(order);

                Core.Core.Context.SaveChanges();

                LoadOrders();

                MessageBox.Show(
                    "Заказ удалён.",
                    "Готово",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось удалить заказ:\n\n"
                    + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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
        // ОТЧЁТ
        // =========================
        private void ExportDocxButton_Click(
            object sender,
            RoutedEventArgs e)
                {
            ReportService.ExportWord();
             }


        private void ExportPdfButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ReportService.ExportPdf();
        }



        private void ExportXlsxButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ReportService.ExportExcel();

        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            StatisticWindow window = new StatisticWindow();

            window.ShowDialog();
        }



        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ToggleTheme();
        }
    }
}

