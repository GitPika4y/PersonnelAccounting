using Data.Models.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.Services;

public interface IEmployeeReportExportService
{
    void BuildReport(IEnumerable<Employee> employees, EmployeeFilters? filters, string savePath);
}