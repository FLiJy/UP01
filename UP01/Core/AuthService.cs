using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP01.Core
{
    public static class AuthService
    {
        public static users CurrentUser { get; private set; }

        public static string Login(
            string login,
            string password)
        {
            if (string.IsNullOrWhiteSpace(login))
                return "Введите логин.";

            if (string.IsNullOrWhiteSpace(password))
                return "Введите пароль.";

            var user = Core.Context.users
                .FirstOrDefault(x => x.login == login);

            if (user == null)
                return "Неверный логин или пароль.";

            if (!PasswordService.VerifyPassword(
                password,
                user.password_hash))
            {
                return "Неверный логин или пароль.";
            }

            CurrentUser = user;

            return "";
        }

        public static string Register(
            string firstName,
            string lastName,
            string login,
            string password,
            string repeatPassword)
        {
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                return "Заполните все поля.";
            }

            if (password != repeatPassword)
                return "Пароли не совпадают.";

            if (password.Length < 6)
                return "Пароль должен содержать минимум 6 символов.";

            if (Core.Context.users.Any(x => x.login == login))
                return "Пользователь с таким логином уже существует.";

            var user = new users
            {
                login = login,
                password_hash = PasswordService.HashPassword(password),
                first_name = firstName,
                last_name = lastName,
                role = "Пользователь",
                created_at = DateTime.Now
            };

            Core.Context.users.Add(user);
            Core.Context.SaveChanges();

            return "";
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}