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

public partial class EmployeeReportExportModalViewModel: ViewModelBase
{
    public IEnumerable<Employee> Employees { get; }
    public EmployeeFilters? Filters { get; }
    public string? FiltersDescription { get; }
    public ObservableCollection<DataGridColumn> Columns { get; } = [];

    [ObservableProperty] private int _count;

    public EmployeeReportExportModalViewModel(
        IEnumerable<Employee> employees,
        EmployeeFilters? filters)
    {
        Employees = employees;
        Filters = filters;
        FiltersDescription = filters?.BuildDescription();

        Count = Employees.Count();

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
            Header = "Таб. номер",
            Binding = new Binding("Id")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Фамилия",
            Binding = new Binding("LastName")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Имя",
            Binding = new Binding("FirstName")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Отчество",
            Binding = new Binding("MiddleName")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Пол",
            Binding = new Binding("Gender")
            {
                Converter = new EnumToDisplayNameConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Дата рождения",
            Binding = new Binding("BirthDate")
            {
                Converter = new DateOrInfiniteConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "ИНН",
            Binding = new Binding("Inn")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Квалификация",
            Binding = new Binding("Education.Qualification")
            {
                Converter = new EnumToDisplayNameConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Специальность",
            Binding = new Binding("Education.Specialization")
            {
                Converter = new EnumToDisplayNameConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Год окончания",
            Binding = new Binding("Education.GraduationYear")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Паспорт серия",
            Binding = new Binding("Passport.Serial")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Паспорт номер",
            Binding = new Binding("Passport.Number")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Паспорт дата",
            Binding = new Binding("Passport.Date")
            {
                Converter = new DateOrInfiniteConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Паспорт выдан",
            Binding = new Binding("Passport.GivenBy")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Статус",
            Binding = new Binding("Status")
            {
                Converter = new EnumToDisplayNameConverter()
            }
        });

        if (Filters is { Status: EmployeeStatus.Fired or EmployeeStatus.NotWorking })
            return;

        Columns.Add(new DataGridTextColumn
        {
            Header = "Работает с",
            Binding = new Binding("InWorkSince")
            {
                Converter = new DateOrInfiniteConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Работает до",
            Binding = new Binding("InWorkUntil")
            {
                Converter = new DateOrInfiniteConverter()
            }
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Должность",
            Binding = new Binding("Position.Title")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Отдел",
            Binding = new Binding("Department.Title")
        });

        Columns.Add(new DataGridTextColumn
        {
            Header = "Ставка",
            Binding = new Binding("Position.Salary")
        });
    }
}