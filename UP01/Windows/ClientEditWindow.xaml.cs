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
    /// Логика взаимодействия для ClientEditWindow.xaml
    /// </summary>
    public partial class ClientEditWindow : Window
    {
        private dynamic currentClient;
        private bool isEdit;

        public ClientEditWindow()
        {
            InitializeComponent();

            currentClient =
                Core.Core.Context.clients.Create();

            isEdit = false;
        }
        public ClientEditWindow(object client)
        {
            InitializeComponent();

            currentClient = client;

            isEdit = true;

            TitleTextBlock.Text =
                "Редактирование клиента";

            LoadClient();
        }


        // =========================
        // ЗАГРУЗКА КЛИЕНТА
        // =========================

        private void LoadClient()
        {
            FirstNameTextBox.Text =
                currentClient.first_name;

            LastNameTextBox.Text =
                currentClient.last_name;

            PhoneTextBox.Text =
                currentClient.phone;

            EmailTextBox.Text =
                currentClient.email;
        }


        // =========================
        // СОХРАНЕНИЕ
        // =========================

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";

            string firstName =
                FirstNameTextBox.Text.Trim();

            string lastName =
                LastNameTextBox.Text.Trim();

            string phone =
                PhoneTextBox.Text.Trim();

            string email =
                EmailTextBox.Text.Trim();


            // Имя

            if (string.IsNullOrWhiteSpace(firstName))
            {
                ErrorTextBlock.Text =
                    "Введите имя.";

                return;
            }


            // Фамилия

            if (string.IsNullOrWhiteSpace(lastName))
            {
                ErrorTextBlock.Text =
                    "Введите фамилию.";

                return;
            }


            // Телефон

            if (!Regex.IsMatch(
                phone,
                @"^\+7\d{10}$"))
            {
                ErrorTextBlock.Text =
                    "Телефон должен быть в формате +79991234567.";

                return;
            }


            // Email

            if (!Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ErrorTextBlock.Text =
                    "Введите корректный Email.";

                return;
            }


            // Заполняем объект

            currentClient.first_name =
                firstName;

            currentClient.last_name =
                lastName;

            currentClient.phone =
                phone;

            currentClient.email =
                email;


            // Добавление

            if (!isEdit)
            {
                currentClient.created_at =
                    DateTime.Now;

                Core.Core.Context.clients.Add(
                    currentClient);
            }


            try
            {
                Core.Core.Context.SaveChanges();

                MessageBox.Show(
                    isEdit
                        ? "Данные клиента изменены."
                        : "Клиент успешно добавлен.",
                    "Готово",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Не удалось сохранить данные клиента.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================
        // БУКВЫ
        // =========================

        private void LettersOnly_PreviewTextInput(
            object sender,
            TextCompositionEventArgs e)
        {
            e.Handled =
                !Regex.IsMatch(
                    e.Text,
                    @"^[А-Яа-яA-Za-zЁё]+$");
        }


        // =========================
        // ТЕЛЕФОН
        // =========================

        private void Phone_PreviewTextInput(
            object sender,
            TextCompositionEventArgs e)
        {
            e.Handled =
                !Regex.IsMatch(
                    e.Text,
                    @"^[0-9+]+$");
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