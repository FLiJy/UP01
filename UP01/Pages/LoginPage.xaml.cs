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
using UP01;
namespace UP01.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private int failedAttempts = 0;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login))
            {
                ErrorTextBlock.Text = "Введите логин.";
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorTextBlock.Text = "Введите пароль.";
                return;
            }

            var user = Core.Context.users
                .FirstOrDefault(x => x.login == login);

            if (user == null)
            {
                failedAttempts++;
                ErrorTextBlock.Text = "Неверный логин или пароль.";
                return;
            }

            // Временно проверяем тестовые значения.
            // Позже заменим на PasswordService и хэширование.

            bool passwordCorrect =
                (login == "admin" && password == "admin123") ||
                (login == "user" && password == "user123");

            if (!passwordCorrect)
            {
                failedAttempts++;
                ErrorTextBlock.Text = "Неверный логин или пароль.";
                return;
            }

            failedAttempts = 0;

            ErrorTextBlock.Text = "";

            MessageBox.Show(
                "Авторизация выполнена успешно.",
                "Успешный вход",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            if (user.role == "Администратор")
            {
                // Позже откроем AdminPage
            }
            else
            {
                // Позже откроем UserPage
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Позже добавим переход на RegisterPage
            MessageBox.Show(
                "Страница регистрации будет добавлена следующим модулем.",
                "Регистрация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}

