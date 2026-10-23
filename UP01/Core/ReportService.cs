using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClosedXML.Excel;
using Xceed.Words.NET;
using Xceed.Document.NET;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel;

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

        public static void ExportWord()
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
                        "Отчет_по_заказам.docx");

                using (DocX document = DocX.Create(filePath))
                {
                    document.InsertParagraph(
                        "Отчет по заказам")
                        .Bold()
                        .FontSize(18)
                        .Alignment = Alignment.center;

                    document.InsertParagraph(
                        "Дата формирования: "
                        + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));

                    document.InsertParagraph("");

                    Xceed.Document.NET.Table table = document.AddTable(
                         orders.Count + 1,  6);

                    table.Design = TableDesign.TableGrid;

                    table.Rows[0].Cells[0]
                        .Paragraphs[0]
                        .Append("№ заказа");

                    table.Rows[0].Cells[1]
                        .Paragraphs[0]
                        .Append("Автомобиль");

                    table.Rows[0].Cells[2]
                        .Paragraphs[0]
                        .Append("Статус");

                    table.Rows[0].Cells[3]
                        .Paragraphs[0]
                        .Append("Дата начала");

                    table.Rows[0].Cells[4]
                        .Paragraphs[0]
                        .Append("Дата завершения");

                    table.Rows[0].Cells[5]
                        .Paragraphs[0]
                        .Append("Сумма");

                    for (int i = 0; i < orders.Count; i++)
                    {
                        var order = orders[i];

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

                        int row = i + 1;

                        table.Rows[row].Cells[0]
                            .Paragraphs[0]
                            .Append(
                                order.order_id.ToString());

                        table.Rows[row].Cells[1]
                            .Paragraphs[0]
                            .Append(vehicleName);

                        table.Rows[row].Cells[2]
                            .Paragraphs[0]
                            .Append(order.status);

                        table.Rows[row].Cells[3]
                            .Paragraphs[0]
                            .Append(
                                order.start_date
                                .ToString("dd.MM.yyyy"));

                        table.Rows[row].Cells[4]
                            .Paragraphs[0]
                            .Append(
                                order.completion_date.HasValue
                                    ? order.completion_date
                                        .Value
                                        .ToString("dd.MM.yyyy")
                                    : "-");

                        table.Rows[row].Cells[5]
                            .Paragraphs[0]
                            .Append(
                                order.total_amount
                                .ToString("N2") + " руб.");
                    }

                    document.InsertTable(table);

                    document.InsertParagraph("");

                    document.InsertParagraph(
                        "Всего заказов: " + orders.Count)
                        .Bold();

                    document.Save();
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
                    "Ошибка при создании Word-отчёта:\n\n"
                    + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }




        public static void ExportPdf()
        {
            try
            {
                var orders = Core.Context.work_orders.ToList();

                if (orders.Count == 0)
                {
                    MessageBox.Show(
                        "В базе данных нет заказов для экспорта.",
                        "Экспорт",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                string desktop = Environment.GetFolderPath(
                    Environment.SpecialFolder.Desktop);

                string filePath = Path.Combine(
                    desktop, "Отчет_по_заказам.pdf");

                var document = new MigraDoc.DocumentObjectModel.Document();
                document.Info.Title = "Отчет по заказам";

                var section = document.AddSection();

                section.PageSetup.PageFormat =
                    MigraDoc.DocumentObjectModel.PageFormat.A4;

                section.PageSetup.Orientation =
                    MigraDoc.DocumentObjectModel.Orientation.Landscape;

                section.PageSetup.LeftMargin =
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);

                section.PageSetup.RightMargin =
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);

                var title = section.AddParagraph("Отчет по заказам");
                title.Format.Font.Name = "Arial";
                title.Format.Font.Size = 18;
                title.Format.Font.Bold = true;
                title.Format.Alignment =
                    MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;

                var date = section.AddParagraph(
                    "Дата формирования: " +
                    DateTime.Now.ToString("dd.MM.yyyy HH:mm"));

                date.Format.Font.Name = "Arial";
                date.Format.Font.Size = 10;
                date.Format.SpaceAfter =
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0.5);

                MigraDoc.DocumentObjectModel.Tables.Table pdfTable =
                    section.AddTable();

                pdfTable.Borders.Width = 0.5;

                pdfTable.AddColumn(
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
                pdfTable.AddColumn(
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5.5));
                pdfTable.AddColumn(
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));
                pdfTable.AddColumn(
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.2));
                pdfTable.AddColumn(
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.2));
                pdfTable.AddColumn(
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));

                MigraDoc.DocumentObjectModel.Tables.Row headerRow =
                    pdfTable.AddRow();

                headerRow.HeadingFormat = true;
                headerRow.Format.Font.Name = "Arial";
                headerRow.Format.Font.Size = 9;
                headerRow.Format.Font.Bold = true;
                headerRow.Shading.Color =
                    MigraDoc.DocumentObjectModel.Colors.LightGray;

                headerRow.Cells[0].AddParagraph("№");
                headerRow.Cells[1].AddParagraph("Автомобиль");
                headerRow.Cells[2].AddParagraph("Статус");
                headerRow.Cells[3].AddParagraph("Дата начала");
                headerRow.Cells[4].AddParagraph("Дата завершения");
                headerRow.Cells[5].AddParagraph("Сумма");

                foreach (var order in orders)
                {
                    var vehicle = Core.Context.vehicles.FirstOrDefault(
                        x => x.vehicle_id == order.vehicle_id);

                    string vehicleName = vehicle == null
                        ? "Не указан"
                        : vehicle.make + " " + vehicle.model +
                          " (" + vehicle.license_plate + ")";

                    MigraDoc.DocumentObjectModel.Tables.Row orderRow =
                        pdfTable.AddRow();

                    orderRow.Format.Font.Name = "Arial";
                    orderRow.Format.Font.Size = 9;

                    orderRow.Cells[0].AddParagraph(
                        order.order_id.ToString());

                    orderRow.Cells[1].AddParagraph(vehicleName);
                    orderRow.Cells[2].AddParagraph(order.status ?? "-");

                    orderRow.Cells[3].AddParagraph(
                        order.start_date.ToString("dd.MM.yyyy"));

                    orderRow.Cells[4].AddParagraph(
                        order.completion_date.HasValue
                            ? order.completion_date.Value.ToString("dd.MM.yyyy")
                            : "-");

                    orderRow.Cells[5].AddParagraph(
                        order.total_amount.ToString("N2") + " руб.");
                }

                var summary = section.AddParagraph(
                    "Всего заказов: " + orders.Count);

                summary.Format.Font.Name = "Arial";
                summary.Format.Font.Size = 11;
                summary.Format.Font.Bold = true;
                summary.Format.SpaceBefore =
                    MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0.5);

                var renderer =
                    new MigraDoc.Rendering.PdfDocumentRenderer();

                renderer.Document = document;
                renderer.RenderDocument();
                renderer.PdfDocument.Save(filePath);

                MessageBox.Show(
                    "PDF-отчет успешно создан:\n\n" + filePath,
                    "Экспорт завершен",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка при создании PDF-отчета:\n\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}