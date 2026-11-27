
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

            LoadEmployees();
            LoadVehicles();

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

            LoadEmployees();
            LoadVehicles();
            LoadOrder();
        }

        // Загрузка сотрудников
        private void LoadEmployees()
        {
            var employees =
                Core.Core.Context.employees
                .Where(x => x.is_active == true)
                .OrderBy(x => x.last_name)
                .Select(x => new
                {
                    x.employee_id,

                    DisplayName =
                        x.last_name + " " +
                        x.first_name + " (" +
                        x.position + ")"
                })
                .ToList();

            EmployeeComboBox.ItemsSource = employees;
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

        // Загрузка информации о заказе
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

            if (VehicleComboBox.SelectedValue == null)
            {
                ErrorTextBlock.Text =
                    "Выберите автомобиль.";

                return;
            }

            if (StatusComboBox.SelectedItem == null)
            {
                ErrorTextBlock.Text =
                    "Выберите статус.";

                return;
            }

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

            // Записываем выбранного сотрудника
            if (EmployeeComboBox.SelectedValue != null)
            {
                currentOrder.employee_id =
                    Convert.ToInt32(
                        EmployeeComboBox.SelectedValue);
            }
            else
            {
                currentOrder.employee_id = null;
            }

            currentOrder.vehicle_id =
                Convert.ToInt32(
                    VehicleComboBox.SelectedValue);

            currentOrder.status =
                statusItem.Content.ToString();

            currentOrder.start_date = startDate;
            currentOrder.completion_date = completionDate;
            currentOrder.total_amount = totalAmount;

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
                    "Ошибка сохранения заказа:\n\n" +
                    ex.Message,
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
