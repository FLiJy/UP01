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
    public partial class EmployeeEditWindow : Window
    {
        private employees currentEmployee;
        private bool isEditMode;

        // Добавление сотрудника
        public EmployeeEditWindow()
        {
            InitializeComponent();

            currentEmployee = new employees();
            isEditMode = false;

            WindowTitle.Text = "Добавление сотрудника";
            HireDatePicker.SelectedDate = DateTime.Today;
            IsActiveCheckBox.IsChecked = true;
        }

        // Редактирование сотрудника
        public EmployeeEditWindow(employees employee)
        {
            InitializeComponent();

            currentEmployee = employee;
            isEditMode = true;

            WindowTitle.Text = "Редактирование сотрудника";

            FirstNameTextBox.Text = employee.first_name;
            LastNameTextBox.Text = employee.last_name;
            PhoneTextBox.Text = employee.phone;
            PositionTextBox.Text = employee.position;
            HireDatePicker.SelectedDate = employee.hire_date;
            IsActiveCheckBox.IsChecked = employee.is_active;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PositionTextBox.Text))
            {
                MessageBox.Show(
                    "Заполните имя, фамилию и должность сотрудника.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (HireDatePicker.SelectedDate == null)
            {
                MessageBox.Show(
                    "Укажите дату приёма на работу.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            currentEmployee.first_name = FirstNameTextBox.Text.Trim();
            currentEmployee.last_name = LastNameTextBox.Text.Trim();
            currentEmployee.phone = PhoneTextBox.Text.Trim();
            currentEmployee.position = PositionTextBox.Text.Trim();
            currentEmployee.hire_date = HireDatePicker.SelectedDate.Value;
            currentEmployee.is_active = IsActiveCheckBox.IsChecked == true;

            try
            {
                if (!isEditMode)
                {
                    Core.Core.Context.employees.Add(currentEmployee);
                }

                Core.Core.Context.SaveChanges();

                MessageBox.Show(
                    "Данные сотрудника успешно сохранены.",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось сохранить сотрудника: " + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}