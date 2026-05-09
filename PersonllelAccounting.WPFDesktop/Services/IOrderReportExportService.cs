using Data.Models.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.Services;

public interface IOrderReportExportService
{
    void BuildReport(IEnumerable<Order> orders, OrderFilters? filters, string savePath);
}