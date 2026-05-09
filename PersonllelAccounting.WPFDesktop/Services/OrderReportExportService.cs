using ClosedXML.Excel;
using Data.Models.Main;
using WPF_Desktop.Extensions;
using WPF_Desktop.Utils;

namespace WPF_Desktop.Services;

public class OrderReportExportService : IOrderReportExportService
{
    public void BuildReport(IEnumerable<Order> orders, OrderFilters? filters, string savePath)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Orders");

        var currentDate = DateTime.Now;

        worksheet.Cell(1, 1).Value = $"СГЕНЕРИРОВАНО АВТОМАТИЧЕСКИ {currentDate:dd.MM.yyyy}";
        worksheet.Range(1, 1, 1, 10).Merge();

        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 16;
        worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

        var row = 3;

        if (filters != null)
        {
            PrintHeadersWithFilters(ref row, worksheet, filters);
            row++;

            ExportWithFilters(row, worksheet, orders, filters);
        }
        else
        {
            PrintHeaders(ref row, worksheet);
            row++;

            Export(row, worksheet, orders);
        }

        worksheet.Columns().AdjustToContents();
        worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        workbook.SaveAs(savePath);
    }

    private void PrintHeaders(ref int row, IXLWorksheet worksheet)
    {
        worksheet.Cell(row, 1).Value = "Номер";
        worksheet.Cell(row, 2).Value = "Дата";
        worksheet.Cell(row, 3).Value = "Дата вступления в силу";
        worksheet.Cell(row, 4).Value = "Дата окончания действия";
        worksheet.Cell(row, 5).Value = "Сотрудник";
        worksheet.Cell(row, 6).Value = "Статус";
        worksheet.Cell(row, 7).Value = "Тип";
        worksheet.Cell(row, 8).Value = "Отдел (найм)";
        worksheet.Cell(row, 9).Value = "Должность (найм)";
        worksheet.Cell(row, 10).Value = "Причина (увольнение)";

        var headerRange = worksheet.Range(row, 1, row, 10);

        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        headerRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    }

    private void Export(int row, IXLWorksheet worksheet, IEnumerable<Order> orders)
    {
        foreach (var order in orders)
        {
            var startCol = 1;
            var col = startCol;

            worksheet.Cell(row, col++).SetValue(order.Id);
            worksheet.Cell(row, col++).SetValue(order.Date);
            worksheet.Cell(row, col++).SetValue(order.StartDate);
            worksheet.Cell(row, col++).SetValue(order.EndDate);
            worksheet.Cell(row, col++).SetValue(order.Employee.FullName);
            worksheet.Cell(row, col++).SetValue(order.Status);
            worksheet.Cell(row, col++).SetValue(order.Type);
            worksheet.Cell(row, col++).SetValue(order.HireDepartment?.Title);
            worksheet.Cell(row, col++).SetValue(order.HirePosition?.Title);
            worksheet.Cell(row, col).SetValue(order.FireReason);

            var dataRange = worksheet.Range(row, startCol, row, 10);

            dataRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            row++;
        }
    }

    private void PrintHeadersWithFilters(ref int row, IXLWorksheet worksheet, OrderFilters filters)
    {
        worksheet.Cell(row, 1).Value = filters.BuildDescription();

        worksheet.Range(row, 1, row, 10).Merge();

        worksheet.Cell(row, 1).Style.Font.Italic = true;
        worksheet.Cell(row, 1).Style.Font.FontColor = XLColor.DarkBlue;
        worksheet.Cell(row, 1).Style.Alignment.WrapText = true;

        row++;

        worksheet.Cell(row, 1).Value = "Номер";
        worksheet.Cell(row, 2).Value = "Дата";
        worksheet.Cell(row, 3).Value = "Дата вступления в силу";
        worksheet.Cell(row, 4).Value = "Дата окончания действия";
        worksheet.Cell(row, 5).Value = "Сотрудник";
        worksheet.Cell(row, 6).Value = "Статус";
        worksheet.Cell(row, 7).Value = "Тип";

        switch (filters.OrderType)
        {
            case OrderType.Hire:
                worksheet.Cell(row, 8).Value = "Отдел";
                worksheet.Cell(row, 9).Value = "Должность";
                break;

            case OrderType.Fire:
                worksheet.Cell(row, 8).Value = "Причина увольнения";
                break;

            case OrderType.StudyLeave:
            case OrderType.Vacation:
            case OrderType.BusinessTrip:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        var headerRange = worksheet.Range(row, 1, row, 10);

        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        headerRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    }

    private void ExportWithFilters(
        int row,
        IXLWorksheet worksheet,
        IEnumerable<Order> orders,
        OrderFilters filters)
    {
        foreach (var order in orders)
        {
            var startCol = 1;
            var col = startCol;

            worksheet.Cell(row, col++).SetValue(order.Id);
            worksheet.Cell(row, col++).SetValue(order.Date);
            worksheet.Cell(row, col++).SetValue(order.StartDate);
            worksheet.Cell(row, col++).SetValue(order.EndDate);
            worksheet.Cell(row, col++).SetValue(order.Employee.FullName);
            worksheet.Cell(row, col++).SetValue(order.Status);
            worksheet.Cell(row, col++).SetValue(order.Type);

            switch (filters.OrderType)
            {
                case OrderType.Hire:
                    worksheet.Cell(row, col++).SetValue(order.HireDepartment?.Title);
                    worksheet.Cell(row, col).SetValue(order.HirePosition?.Title);
                    break;

                case OrderType.Fire:
                    worksheet.Cell(row, col).SetValue(order.FireReason);
                    break;

                case OrderType.StudyLeave:
                case OrderType.Vacation:
                case OrderType.BusinessTrip:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            var dataRange = worksheet.Range(row, startCol, row, 10);

            dataRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            row++;
        }
    }
}