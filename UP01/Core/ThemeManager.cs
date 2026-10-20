using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace UP01.Core
{
    public static class ThemeManager
    {
        public static bool IsDarkTheme { get; private set; }

        public static void LoadTheme()
        {
            string theme =
                Properties.Settings.Default.Theme;

            if (theme == "Dark")
            {
                SetDarkTheme();
            }
            else
            {
                SetLightTheme();
            }
        }


        public static void ToggleTheme()
        {
            if (IsDarkTheme)
            {
                SetLightTheme();
            }
            else
            {
                SetDarkTheme();
            }
        }


        public static void SetLightTheme()
        {
            SetColor(
                "PageBackgroundBrush",
                "#F3F3F3");

            SetColor(
                "CardBackgroundBrush",
                "#FFFFFF");

            SetColor(
                "WhiteBrush",
                "#FFFFFF");

            SetColor(
                "TextBrush",
                "#193D57");

            SetColor(
                "SecondaryTextBrush",
                "#9197A3");

            SetColor(
                "PrimaryBrush",
                "#0198AF");

            SetColor(
                "PrimaryDarkBrush",
                "#006B80");

            SetColor(
                "SecondaryBrush",
                "#233E76");

            SetColor(
                "BorderBrush",
                "#EAEBED");

            SetColor(
                "BlueDarkBrush",
                "#193D57");

            SetColor(
                "LightBackgroundBrush",
                "#F3F3F3");

            IsDarkTheme = false;

            SaveTheme("Light");
        }


        public static void SetDarkTheme()
        {
            SetColor(
                "PageBackgroundBrush",
                "#18212B");

            SetColor(
                "CardBackgroundBrush",
                "#24313D");

            SetColor(
                "WhiteBrush",
                "#FFFFFF");

            SetColor(
                "TextBrush",
                "#F3F3F3");

            SetColor(
                "SecondaryTextBrush",
                "#B8C0C8");

            SetColor(
                "PrimaryBrush",
                "#4DC6C5");

            SetColor(
                "PrimaryDarkBrush",
                "#006B80");

            SetColor(
                "SecondaryBrush",
                "#6D91C9");

            SetColor(
                "BorderBrush",
                "#3A4855");

            SetColor(
                "BlueDarkBrush",
                "#15202A");

            SetColor(
                "LightBackgroundBrush",
                "#18212B");

            IsDarkTheme = true;

            SaveTheme("Dark");
        }


        private static void SetColor(
                 string resourceName,
                 string color)
            {
            SolidColorBrush brush =
                Application.Current.Resources[resourceName]
                as SolidColorBrush;

            if (brush != null)
            {
                SolidColorBrush newBrush =
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString(color));

                Application.Current.Resources[resourceName] =
                    newBrush;
            }
        }


        private static void SaveTheme(
            string theme)
        {
            Properties.Settings.Default.Theme =
                theme;

            Properties.Settings.Default.Save();
        }
    }
}