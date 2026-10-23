using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClosedXML.Excel;

namespace UP01.Core
{
    public static class ReportService
    {
        public static void ExportExcel()
        {
            try
            {
                var orders = Core.Context.work_orders
                    .ToList();

                if (orders.Count == 0)
                {
                    MessageBox.Show(
                        "В базе данных нет заказов для экспорта.",
                        "Экспорт",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    return;
                }

                string desktop =
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.Desktop);

                string filePath =
                    Path.Combine(
                        desktop,
                        "Отчет_по_заказам.xlsx");

                using (XLWorkbook workbook =
                    new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                        workbook.Worksheets.Add(
                            "Заказы");

                    worksheet.Cell(1, 1)
                        .Value = "Отчет по заказам";

                    worksheet.Cell(2, 1)
                        .Value = "Дата формирования:";

                    worksheet.Cell(2, 2)
                        .Value = DateTime.Now;

                    worksheet.Cell(4, 1)
                        .Value = "№ заказа";

                    worksheet.Cell(4, 2)
                        .Value = "Автомобиль";

                    worksheet.Cell(4, 3)
                        .Value = "Статус";

                    worksheet.Cell(4, 4)
                        .Value = "Дата начала";

                    worksheet.Cell(4, 5)
                        .Value = "Дата завершения";

                    worksheet.Cell(4, 6)
                        .Value = "Сумма";

                    int row = 5;

                    foreach (var order in orders)
                    {
                        var vehicle =
                            Core.Context.vehicles
                            .FirstOrDefault(
                                x => x.vehicle_id ==
                                order.vehicle_id);

                        string vehicleName =
                            vehicle == null
                                ? "Не указан"
                                : vehicle.make + " "
                                  + vehicle.model + " "
                                  + vehicle.license_plate;

                        worksheet.Cell(row, 1)
                            .Value = order.order_id;

                        worksheet.Cell(row, 2)
                            .Value = vehicleName;

                        worksheet.Cell(row, 3)
                            .Value = order.status;

                        worksheet.Cell(row, 4)
                            .Value = order.start_date;

                        worksheet.Cell(row, 5)
                            .Value =
                            order.completion_date;

                        worksheet.Cell(row, 6)
                            .Value =
                            order.total_amount;

                        row++;
                    }

                    worksheet.Range(
                        "A4:F4")
                        .Style.Font.Bold = true;

                    worksheet.Range(
                        "A1:F1")
                        .Merge();

                    worksheet.Cell(1, 1)
                        .Style.Font.Bold = true;

                    worksheet.Cell(1, 1)
                        .Style.Font.FontSize = 16;

                    worksheet.Columns()
                        .AdjustToContents();

                    workbook.SaveAs(filePath);
                }

                MessageBox.Show(
                    "Отчёт успешно экспортирован:\n\n"
                    + filePath,
                    "Экспорт завершён",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка при создании Excel-отчёта:\n\n"
                    + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}