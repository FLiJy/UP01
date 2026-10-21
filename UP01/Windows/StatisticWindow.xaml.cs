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
    /// <summary>
    /// Логика взаимодействия для StatisticWindow.xaml
    /// </summary>
    public partial class StatisticWindow : Window
    {
        public StatisticWindow()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                var statuses = Core.Core.Context.work_orders
                    .Select(x => x.status)
                    .ToList();

                string[] allStatuses =
                {
                    "Новый",
                    "В работе",
                    "Ожидает запчасти",
                    "Завершён",
                    "Отменён"
                };

                int total = statuses.Count;

                List<StatusStatistic> statistics =
                    new List<StatusStatistic>();

                foreach (string status in allStatuses)
                {
                    int count = statuses.Count(x =>
                        string.Equals(
                            x,
                            status,
                            StringComparison.OrdinalIgnoreCase));

                    double percent = total == 0
                        ? 0
                        : count * 100.0 / total;

                    statistics.Add(new StatusStatistic
                    {
                        Name = status,
                        Count = count,
                        Percent = percent
                    });
                }

                TotalTextBlock.Text =
                    "Всего заказов: " + total;

                StatisticsItemsControl.ItemsSource =
                    statistics;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось загрузить статистику.\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoadStatistics();
        }
    }

    public class StatusStatistic
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }
    }
}