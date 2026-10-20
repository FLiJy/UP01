using System;
using System.Collections.Generic;
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

namespace UP01.Windows
{
    /// <summary>
    /// Логика взаимодействия для VehicleEditWindow.xaml
    /// </summary>
    public partial class VehicleEditWindow : Window
    {
        private vehicles currentVehicle;
        private bool isEdit;

        public VehicleEditWindow()
        {
            InitializeComponent();

            isEdit = false;

            LoadClients();

            currentVehicle =
                Core.Core.Context.vehicles.Create();
        }


        public VehicleEditWindow(vehicles vehicle)
        {
            InitializeComponent();

            currentVehicle = vehicle;

            isEdit = true;

            TitleTextBlock.Text =
                "Редактирование автомобиля";

            LoadClients();
            LoadVehicle();
        }


        // =========================
        // ЗАГРУЗКА КЛИЕНТОВ
        // =========================

        private void LoadClients()
        {
            var clients =
                Core.Core.Context.clients
                    .OrderBy(x => x.last_name)
                    .ToList()
                    .Select(x => new
                    {
                        x.client_id,
                        FullName =
                            x.first_name
                            + " "
                            + x.last_name
                    })
                    .ToList();

            ClientComboBox.ItemsSource = clients;
        }


        // =========================
        // ЗАГРУЗКА АВТОМОБИЛЯ
        // =========================

        private void LoadVehicle()
        {
            ClientComboBox.SelectedValue =
                currentVehicle.client_id;

            VinTextBox.Text =
                currentVehicle.vin;

            MakeTextBox.Text =
                currentVehicle.make;

            ModelTextBox.Text =
                currentVehicle.model;

            YearTextBox.Text =
                currentVehicle.year.ToString();

            LicensePlateTextBox.Text =
                currentVehicle.license_plate;
        }


        // =========================
        // СОХРАНЕНИЕ
        // =========================

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";

            // Клиент

            if (ClientComboBox.SelectedValue == null)
            {
                ErrorTextBlock.Text =
                    "Выберите владельца автомобиля.";

                return;
            }


            int clientId =
                Convert.ToInt32(
                    ClientComboBox.SelectedValue);


            // VIN

            string vin =
                VinTextBox.Text.Trim().ToUpper();

            if (!Regex.IsMatch(
                vin,
                @"^[A-HJ-NPR-Z0-9]{17}$"))
            {
                ErrorTextBlock.Text =
                    "VIN должен содержать ровно 17 символов.";

                return;
            }


            // Марка

            string make =
                MakeTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(make))
            {
                ErrorTextBlock.Text =
                    "Введите марку автомобиля.";

                return;
            }


            // Модель

            string model =
                ModelTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(model))
            {
                ErrorTextBlock.Text =
                    "Введите модель автомобиля.";

                return;
            }


            // Год

            int year;

            if (!int.TryParse(
                YearTextBox.Text,
                out year))
            {
                ErrorTextBlock.Text =
                    "Год должен быть числом.";

                return;
            }

            if (year < 1900 ||
                year > DateTime.Now.Year)
            {
                ErrorTextBlock.Text =
                    "Введите корректный год выпуска.";

                return;
            }


            // Госномер

            string licensePlate =
                LicensePlateTextBox.Text
                    .Trim()
                    .ToUpper();

            if (!Regex.IsMatch(
                licensePlate,
                @"^[АВЕКМНОРСТУХA-Z]\d{3}[АВЕКМНОРСТУХA-Z]{2}\d{2,3}$"))
            {
                ErrorTextBlock.Text =
                    "Некорректный государственный номер.";

                return;
            }


            // Проверяем VIN на дубликат

            bool vinExists =
                Core.Core.Context.vehicles.Any(x =>
                    x.vin == vin &&
                    x.vehicle_id !=
                    currentVehicle.vehicle_id);

            if (vinExists)
            {
                ErrorTextBlock.Text =
                    "Автомобиль с таким VIN уже существует.";

                return;
            }


            // Записываем данные

            currentVehicle.client_id =
                clientId;

            currentVehicle.vin =
                vin;

            currentVehicle.make =
                make;

            currentVehicle.model =
                model;

            currentVehicle.year =
                year;

            currentVehicle.license_plate =
                licensePlate;


            try
            {
                if (!isEdit)
                {
                    Core.Core.Context.vehicles.Add(
                        currentVehicle);
                }

                Core.Core.Context.SaveChanges();

                MessageBox.Show(
                    isEdit
                        ? "Данные автомобиля изменены."
                        : "Автомобиль успешно добавлен.",
                    "Готово",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось сохранить автомобиль.\n\n"
                    + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================
        // ТОЛЬКО ЦИФРЫ
        // =========================

        private void NumberOnly_PreviewTextInput(
            object sender,
            TextCompositionEventArgs e)
        {
            e.Handled =
                !Regex.IsMatch(
                    e.Text,
                    @"^[0-9]+$");
        }


        // =========================
        // VIN
        // =========================

        private void VinTextBox_PreviewTextInput(
            object sender,
            TextCompositionEventArgs e)
        {
            e.Handled =
                !Regex.IsMatch(
                    e.Text,
                    @"^[A-Za-z0-9]+$");
        }


        // =========================
        // ОТМЕНА
        // =========================

        private void CancelButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}