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
    /// Логика взаимодействия для ChangePasswordPage.xaml
    /// </summary>
    public partial class ChangePasswordPage : Page
    {
        public ChangePasswordPage()
    {
        InitializeComponent();
    }

    private void SaveButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        string oldPassword =
            OldPasswordBox.Password;

        string newPassword =
            NewPasswordBox.Password;

        string repeatPassword =
            RepeatPasswordBox.Password;

        ErrorTextBlock.Text = "";

        if (string.IsNullOrWhiteSpace(oldPassword) ||
            string.IsNullOrWhiteSpace(newPassword) ||
            string.IsNullOrWhiteSpace(repeatPassword))
        {
            ErrorTextBlock.Text =
                "Заполните все поля.";

            return;
        }

        if (!PasswordService.VerifyPassword(
            oldPassword,
            AuthService.CurrentUser.password_hash))
        {
            ErrorTextBlock.Text =
                "Старый пароль введён неправильно.";

            return;
        }

        if (newPassword.Length < 6)
        {
            ErrorTextBlock.Text =
                "Новый пароль должен содержать минимум 6 символов.";

            return;
        }

        if (newPassword != repeatPassword)
        {
            ErrorTextBlock.Text =
                "Новые пароли не совпадают.";

            return;
        }

        if (oldPassword == newPassword)
        {
            ErrorTextBlock.Text =
                "Новый пароль должен отличаться от старого.";

            return;
        }

        AuthService.CurrentUser.password_hash =
            PasswordService.HashPassword(newPassword);

        Core.Core.Context.SaveChanges();

        MessageBox.Show(
            "Пароль успешно изменён.",
            "Смена пароля",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        NavigationService.GoBack();
    }

    private void BackButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}
}
