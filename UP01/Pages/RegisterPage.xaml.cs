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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string firstName =
                FirstNameTextBox.Text.Trim();

            string lastName =
                LastNameTextBox.Text.Trim();

            string login =
                LoginTextBox.Text.Trim();

            string password =
                PasswordBox.Password;

            string repeatPassword =
                RepeatPasswordBox.Password;

            string result = AuthService.Register(
                firstName,
                lastName,
                login,
                password,
                repeatPassword);

            if (!string.IsNullOrEmpty(result))
            {
                ErrorTextBlock.Text = result;
                return;
            }

            MessageBox.Show(
                "Регистрация успешно завершена.",
                "Регистрация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            NavigationService.Navigate(
                new LoginPage());
        }

        private void BackButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new LoginPage());
        }
    }
}
