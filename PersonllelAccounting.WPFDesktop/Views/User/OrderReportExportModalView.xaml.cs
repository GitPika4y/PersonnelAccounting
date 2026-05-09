using System.Windows.Controls;
using WPF_Desktop.ViewModels.User;

namespace WPF_Desktop.Views.User;

public partial class OrderReportExportModalView : UserControl
{
    public OrderReportExportModalView()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            if (DataContext is OrderReportExportModalViewModel vm)
            {
                OrdersGrid.Columns.Clear();

                foreach (var col in vm.Columns)
                    OrdersGrid.Columns.Add(col);
            }
        };
    }
}