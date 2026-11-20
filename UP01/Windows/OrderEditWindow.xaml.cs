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

namespace UP01.Windows
{
    public partial class OrderEditWindow : Window
    {
        private work_orders currentOrder;
        private bool isEdit;

        // Добавление заказа
        public OrderEditWindow()
        {
            InitializeComponent();

            isEdit = false;

            currentOrder =
                Core.Core.Context.work_orders.Create();

            LoadVehicles();
            LoadEmployees();

            StartDatePicker.SelectedDate = DateTime.Today;
            StatusComboBox.SelectedIndex = 0;
        }

        // Редактирование заказа
        public OrderEditWindow(work_orders order)
        {
            InitializeComponent();

            isEdit = true;
            currentOrder = order;

            TitleTextBlock.Text = "Редактирование заказа";

            LoadVehicles();
            LoadEmployees();

            LoadOrder();
        }

        // Загрузка автомобилей
        private void LoadVehicles()
        {
            var vehicles =
                Core.Core.Context.vehicles
                .Select(x => new
                {
                    x.vehicle_id,

                    DisplayName =
                        x.make + " " +
                        x.model + " - " +
                        x.license_plate
                })
                .ToList();

            VehicleComboBox.ItemsSource = vehicles;
        }

        // Загрузка сотрудников
        private void LoadEmployees()
        {
            var employees =
                Core.Core.Context.employees
                .Where(x => x.is_active == true)
                .Select(x => new
                {
                    x.employee_id,

                    DisplayName =
                        x.first_name + " " +
                        x.last_name + " - " +
                        x.position
                })
                .ToList();

            EmployeeComboBox.ItemsSource = employees;
        }

        // Загрузка данных заказа
        private void LoadOrder()
        {
            EmployeeComboBox.SelectedValue =
                currentOrder.employee_id;

            VehicleComboBox.SelectedValue =
                currentOrder.vehicle_id;

            StartDatePicker.SelectedDate =
                currentOrder.start_date;

            CompletionDatePicker.SelectedDate =
                currentOrder.completion_date;

            TotalAmountTextBox.Text =
                currentOrder.total_amount.ToString();

            for (int i = 0;
                 i < StatusComboBox.Items.Count;
                 i++)
            {
                ComboBoxItem item =
                    StatusComboBox.Items[i] as ComboBoxItem;

                if (item != null &&
                    item.Content.ToString() == currentOrder.status)
                {
                    StatusComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        // Сохранение заказа
        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";

            // Проверка сотрудника
            if (EmployeeComboBox.SelectedValue == null)
            {
                ErrorTextBlock.Text =
                    "Выберите сотрудника.";

                return;
            }

            // Проверка автомобиля
            if (VehicleComboBox.SelectedValue == null)
            {
                ErrorTextBlock.Text =
                    "Выберите автомобиль.";

                return;
            }

            // Проверка статуса
            if (StatusComboBox.SelectedItem == null)
            {
                ErrorTextBlock.Text =
                    "Выберите статус.";

                return;
            }

            // Проверка даты начала
            if (StartDatePicker.SelectedDate == null)
            {
                ErrorTextBlock.Text =
                    "Выберите дату начала.";

                return;
            }

            DateTime startDate =
                StartDatePicker.SelectedDate.Value;

            DateTime? completionDate =
                CompletionDatePicker.SelectedDate;

            if (completionDate.HasValue &&
                completionDate.Value < startDate)
            {
                ErrorTextBlock.Text =
                    "Дата завершения не может быть раньше даты начала.";

                return;
            }

            // Проверка суммы
            decimal totalAmount;

            if (!decimal.TryParse(
                TotalAmountTextBox.Text,
                out totalAmount))
            {
                ErrorTextBlock.Text =
                    "Введите корректную сумму.";

                return;
            }

            if (totalAmount < 0)
            {
                ErrorTextBlock.Text =
                    "Сумма не может быть отрицательной.";

                return;
            }

            ComboBoxItem statusItem =
                StatusComboBox.SelectedItem as ComboBoxItem;

            // Запись данных
            currentOrder.employee_id =
                Convert.ToInt32(EmployeeComboBox.SelectedValue);

            currentOrder.vehicle_id =
                Convert.ToInt32(VehicleComboBox.SelectedValue);

            currentOrder.status =
                statusItem.Content.ToString();

            currentOrder.start_date =
                startDate;

            currentOrder.completion_date =
                completionDate;

            currentOrder.total_amount =
                totalAmount;

            try
            {
                if (!isEdit)
                {
                    Core.Core.Context.work_orders.Add(
                        currentOrder);
                }

                Core.Core.Context.SaveChanges();

                MessageBox.Show(
                    isEdit
                        ? "Заказ успешно изменён."
                        : "Заказ успешно добавлен.",
                    "Готово",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка сохранения заказа:\n\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Отмена
        private void CancelButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}