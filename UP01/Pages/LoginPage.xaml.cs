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
using UP01.Core;
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

        private void LoginButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string login =
                LoginTextBox.Text.Trim();

            string password =
                PasswordBox.Password;

            ErrorTextBlock.Text = "";

            // Проверка капчи после 3 ошибок
            if (failedAttempts >= 3)
            {
                if (!CaptchaService.Check(
                    CaptchaTextBox.Text))
                {
                    ErrorTextBlock.Text =
                        "Введите правильную капчу.";

                    CaptchaTextBlock.Text =
                        CaptchaService.Generate();

                    CaptchaTextBox.Clear();

                    return;
                }
            }

            // Авторизация
            string result =
                AuthService.Login(login, password);

            // Ошибка авторизации
            if (!string.IsNullOrEmpty(result))
            {
                failedAttempts++;

                ErrorTextBlock.Text = result;

                // Показываем капчу после 3 ошибок
                if (failedAttempts >= 3)
                {
                    CaptchaPanel.Visibility =
                        Visibility.Visible;

                    CaptchaTextBlock.Text =
                        CaptchaService.Generate();

                    CaptchaTextBox.Clear();
                }

                return;
            }

            // Успешный вход
            failedAttempts = 0;

            CaptchaPanel.Visibility =
                Visibility.Collapsed;

            CaptchaTextBox.Clear();

            ErrorTextBlock.Text = "";

            MessageBox.Show(
                "Авторизация выполнена успешно.",
                "Успешный вход",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            if (AuthService.CurrentUser.role ==
     "Администратор")
            {
                NavigationService.Navigate(
                    new AdminPage());
            }
            else
            {
                NavigationService.Navigate(
                    new UserPage());
            }
        }

        private void RegisterButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new RegisterPage());
        }
    }
}