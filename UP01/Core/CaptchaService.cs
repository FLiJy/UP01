using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP01.Core
{
    public static class CaptchaService
    {
        private static string currentCaptcha;

        public static string Generate()
        {
            const string chars =
                "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

            Random random = new Random();

            StringBuilder result =
                new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                result.Append(
                    chars[random.Next(chars.Length)]);
            }

            currentCaptcha = result.ToString();

            return currentCaptcha;
        }

        public static bool Check(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return string.Equals(
                value.Trim(),
                currentCaptcha,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
