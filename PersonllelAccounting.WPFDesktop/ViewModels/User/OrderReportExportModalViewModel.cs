using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Converters;
using WPF_Desktop.Utils;
using DataGridTextColumn = System.Windows.Controls.DataGridTextColumn;

namespace WPF_Desktop.ViewModels.User;

public partial class OrderReportExportModalViewModel: ViewModelBase
{
    public IEnumerable<Order> Orders { get; }
    public OrderFilters? Filters { get; }
    public string? FiltersDescription { get; }
    public ObservableCollection<DataGridColumn> Columns { get; } = [];
    [ObservableProperty] private int _count;


    public OrderReportExportModalViewModel(IEnumerable<Order> orders, OrderFilters? filters)
    {
        Orders = orders;
        Filters = filters;
        FiltersDescription = filters?.BuildDescription();
        Count = Orders.Count();
        BuildColumns();
    }

    [RelayCommand]
    private void Save()
    {
        DialogHost.CloseDialogCommand.Execute(true, null);
    }

    private void BuildColumns()
    {
        Columns.Clear();

        Columns.Add(new DataGridTextColumn
        {
            Header = "Номер",
            Binding = new Binding("Id")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Дата",
            Binding = new Binding("Date")
            {
                Converter = new DateOrInfiniteConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Дата вступления",
            Binding = new Binding("StartDate")
            {
                Converter = new DateOrInfiniteConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Дата окончания",
            Binding = new Binding("EndDate")
            {
                Converter = new DateOrInfiniteConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Сотрудник",
            Binding = new Binding("Employee.FullName"),
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Статус",
            Binding = new Binding("Status")
            {
                Converter = new EnumToDisplayNameConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Тип",
            Binding = new Binding("Type")
            {
                Converter = new EnumToDisplayNameConverter()
            }
        });

        if (Filters?.OrderType is null)
        {
            Columns.Add(new DataGridTextColumn
            {
                Header = "Отдел (найм)",
                Binding = new Binding("HireDepartment.Title")
            });

            Columns.Add(new DataGridTextColumn
            {
                Header = "Должность (найм)",
                Binding = new Binding("HirePosition.Title")
            });

            Columns.Add(new DataGridTextColumn
            {
                Header = "Причина увольнения",
                Binding = new Binding("FireReason")
            });
        }

        if (Filters?.OrderType == OrderType.Hire)
        {
            Columns.Add(new DataGridTextColumn
            {
                Header = "Отдел",
                Binding = new Binding("HireDepartment.Title")
            });

            Columns.Add(new DataGridTextColumn
            {
                Header = "Должность",
                Binding = new Binding("HirePosition.Title")
            });
        }

        if (Filters?.OrderType == OrderType.Fire)
        {
            Columns.Add(new DataGridTextColumn
            {
                Header = "Причина увольнения",
                Binding = new Binding("FireReason")
            });
        }
    }
}