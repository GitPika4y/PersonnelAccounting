using ClosedXML.Excel;
using Data.Models.Main;
using WPF_Desktop.Extensions;
using WPF_Desktop.Utils;

namespace WPF_Desktop.Services;

public class EmployeeReportExportService : IEmployeeReportExportService
{
    public void BuildReport(IEnumerable<Employee> employees, EmployeeFilters? filters, string savePath)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Employees");

        var currentDate = DateTime.Now;

        worksheet.Cell(1, 1).Value = $"СГЕНЕРИРОВАНО АВТОМАТИЧЕСКИ {currentDate:dd.MM.yyyy}";
        worksheet.Range(1, 1, 1, 20).Merge();

        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 16;
        worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

        var row = 3;

        FillHeader(ref row, worksheet, filters);
        row++;

        FillData(row, worksheet, employees, filters);

        worksheet.Columns().AdjustToContents();

        worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        workbook.SaveAs(savePath);
    }

    private void FillHeader(ref int row, IXLWorksheet worksheet, EmployeeFilters? filters)
    {
        // Определяем, показываем ли информацию о работе
        var showWorkInfo = filters?.Status is not (EmployeeStatus.Fired or EmployeeStatus.NotWorking);

        var workEndColumn = 20;

        if (!showWorkInfo)
        {
            // Если информацию о работе не показываем, то общая ширина таблицы — 14 колонок
            workEndColumn = 14;
        }

        if (filters is not null)
        {
            worksheet.Cell(row, 1).SetValue(filters.BuildDescription());

            worksheet.Range(row, 1, row, workEndColumn).Merge();

            worksheet.Cell(row, 1).Style.Font.Italic = true;
            worksheet.Cell(row, 1).Style.Font.FontColor = XLColor.DarkBlue;
            worksheet.Cell(row, 1).Style.Alignment.WrapText = true;

            row++;
        }

        // ===== ГРУППЫ =====
        worksheet.Range(row, 1, row, 7).Merge();
        worksheet.Cell(row, 1).Value = "Общая информация";

        worksheet.Range(row, 8, row, 10).Merge();
        worksheet.Cell(row, 8).Value = "Образование";

        worksheet.Range(row, 11, row, 14).Merge();
        worksheet.Cell(row, 11).Value = "Паспортные данные";

        if (showWorkInfo)
        {
            worksheet.Range(row, 15, row, 20).Merge();
            worksheet.Cell(row, 15).Value = "Информация о работе";
        }

        var groupRange = worksheet.Range(row, 1, row, workEndColumn);

        groupRange.Style.Font.Bold = true;
        groupRange.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
        groupRange.Style.Font.FontColor = XLColor.White;
        groupRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        groupRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        groupRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        groupRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        groupRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        groupRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        row++;

        // ===== ОСНОВНЫЕ ЗАГОЛОВКИ =====

        var col = 1;

        worksheet.Cell(row, col++).SetValue("Таб. номер");
        worksheet.Cell(row, col++).SetValue("Фамилия");
        worksheet.Cell(row, col++).SetValue("Имя");
        worksheet.Cell(row, col++).SetValue("Отчество");
        worksheet.Cell(row, col++).SetValue("Пол");
        worksheet.Cell(row, col++).SetValue("Дата рождения");
        worksheet.Cell(row, col++).SetValue("ИНН");

        worksheet.Cell(row, col++).SetValue("Квалификация");
        worksheet.Cell(row, col++).SetValue("Специальность");
        worksheet.Cell(row, col++).SetValue("Год окончания");

        worksheet.Cell(row, col++).SetValue("Паспорт серия");
        worksheet.Cell(row, col++).SetValue("Паспорт номер");
        worksheet.Cell(row, col++).SetValue("Паспорт дата");
        worksheet.Cell(row, col++).SetValue("Паспорт выдан");

        // Заголовки работы — только если показываем
        if (showWorkInfo)
        {
            worksheet.Cell(row, col++).SetValue("Статус");
            worksheet.Cell(row, col++).SetValue("Работает с");
            worksheet.Cell(row, col++).SetValue("Работает до");
            worksheet.Cell(row, col++).SetValue("Должность");
            worksheet.Cell(row, col++).SetValue("Отдел");
            worksheet.Cell(row, col).SetValue("Ставка");
        }

        var headerRange = worksheet.Range(row, 1, row, workEndColumn);

        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        headerRange.Style.Alignment.WrapText = true;

        headerRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    }

    private void FillData(int row, IXLWorksheet worksheet, IEnumerable<Employee> employees, EmployeeFilters? filters)
    {
        foreach (var employee in employees)
        {
            var startCol = 1;
            var col = startCol;

            worksheet.Cell(row, col++).SetValue(employee.Id);
            worksheet.Cell(row, col++).SetValue(employee.FirstName);
            worksheet.Cell(row, col++).SetValue(employee.LastName);
            worksheet.Cell(row, col++).SetValue(employee.MiddleName);
            worksheet.Cell(row, col++).SetValue(employee.Gender);
            worksheet.Cell(row, col++).SetValue(employee.BirthDate);
            worksheet.Cell(row, col++).SetValue(employee.Inn);
            worksheet.Cell(row, col++).SetValue(employee.Education.Qualification);
            worksheet.Cell(row, col++).SetValue(employee.Education.Specialization);
            worksheet.Cell(row, col++).SetValue(employee.Education.GraduationYear);
            worksheet.Cell(row, col++).SetValue(employee.Passport.Serial);
            worksheet.Cell(row, col++).SetValue(employee.Passport.Number);
            worksheet.Cell(row, col++).SetValue(employee.Passport.Date);
            worksheet.Cell(row, col++).SetValue(employee.Passport.GivenBy);

            var showWorkInfo = filters?.Status is not (EmployeeStatus.Fired or EmployeeStatus.NotWorking);

            if (showWorkInfo)
            {
                worksheet.Cell(row, col++).SetValue(employee.Status);
                worksheet.Cell(row, col++).SetValue(employee.InWorkSince);
                worksheet.Cell(row, col++).SetValue(employee.InWorkUntil);
                worksheet.Cell(row, col++).SetValue(employee.Position?.Title);
                worksheet.Cell(row, col++).SetValue(employee.Department?.Title);
                worksheet.Cell(row, col).SetValue(employee.Position?.Salary);
            }

            var dataRange = worksheet.Range(row, startCol, row, showWorkInfo ? 20 : 14);

            dataRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            row++;
        }
    }
}