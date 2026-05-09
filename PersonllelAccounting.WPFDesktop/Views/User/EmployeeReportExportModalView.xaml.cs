using System.Windows.Controls;
using WPF_Desktop.ViewModels.User;

namespace WPF_Desktop.Views.User;

public partial class EmployeeReportExportModalView : UserControl
{
    public EmployeeReportExportModalView()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            if (DataContext is EmployeeReportExportModalViewModel vm)
            {
                EmployeeGrid.Columns.Clear();

                foreach (var col in vm.Columns)
                    EmployeeGrid.Columns.Add(col);
            }
        };
    }
}