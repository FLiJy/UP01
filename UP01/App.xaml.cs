using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UP01.Core;

namespace UP01
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(
    StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeManager.LoadTheme();
        }
    }

}
