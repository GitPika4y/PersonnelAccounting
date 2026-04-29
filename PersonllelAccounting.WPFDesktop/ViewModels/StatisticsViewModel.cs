using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Data.Models.Main;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using WPF_Desktop.Extensions;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels;

public partial class StatisticsViewModel(
    IDepartmentUseCase departmentUseCase,
    IPositionUseCase positionUseCase,
    IEmployeeUseCase employeeUseCase,
    IOrderUseCase orderUseCase
    ): ViewModelBase
{
    public SeriesCollection EmployeesHireCartesianSeries { get; } = [];
    public SeriesCollection OrdersTypePieSeries { get; } = [];
    public SeriesCollection EmployeesDepartmentPositionPieSeries { get; } = [];
    public SeriesCollection NewOrdersCartesianSeries { get; } = [];

    public ObservableCollection<Axis> EmployeeHireCartesianXAxes { get; } = [];
    public ObservableCollection<Axis> NewOrdersCartesianXAxes { get; } = [];

    public IEnumerable<StatisticsEmployeeSeriesType> EmployeeSeriesTypes { get; } = Enum.GetValues<StatisticsEmployeeSeriesType>();

    [ObservableProperty] private string _employeesDepartmentPositionTitle = "Сотрудники по отделам";
    [ObservableProperty] private StatisticsEmployeeSeriesType _selectedEmployeeSeriesType = StatisticsEmployeeSeriesType.Department;

    [ObservableProperty] private int _ordersCount;
    [ObservableProperty] private int _employeesCount;
    [ObservableProperty] private int _departmentsCount;
    [ObservableProperty] private int _positionsCount;
    [ObservableProperty] private string? _mostPopularPosition;
    [ObservableProperty] private double _hireToFireRate;

    partial void OnSelectedEmployeeSeriesTypeChanged(StatisticsEmployeeSeriesType value)
    {
        EmployeesDepartmentPositionTitle = value switch
        {
            StatisticsEmployeeSeriesType.Department => "Сотрудники по отделам",
            StatisticsEmployeeSeriesType.Position => "Сотрудники по должностям",
            _ => value.ToString()
        };

        _ = DrawEmployeesDepartmentPositionChart();
    }

    public async Task InitializeAsync()
    {
        await DrawOrdersTypeChart();
        await DrawEmployeesHireChart();
        await DrawEmployeesDepartmentPositionChart();
        await DrawNewOrdersChart();
        await CalculateStatistics();
    }

    private async Task DrawOrdersTypeChart()
    {
        OrdersTypePieSeries.Clear();

        var orderResource = await orderUseCase.GetAllAsync();

        await HandleResource(
            orderResource,
            orders =>
            {
                var groupedByType = orders.GroupBy(o => o.Type)
                    .Select(g => new {Type = g.Key, Count = g.Count()});

                foreach (var group in groupedByType)
                {
                    var pieSeries = new PieSeries<int>(group.Count) { Name = group.Type.GetDisplayName() };
                    OrdersTypePieSeries.Add(pieSeries);
                }
            });
    }

    private async Task DrawEmployeesHireChart()
    {
        EmployeeHireCartesianXAxes.Clear();
        EmployeesHireCartesianSeries.Clear();

        var currentDate = DateTime.Now;
        var lastWeekDate = currentDate.AddDays(-7);

        var ordersResource = await orderUseCase.GetAllAsync(o =>
            o.StartDate >= lastWeekDate
            && o.StartDate <= currentDate);

        await HandleResource(
            ordersResource,
            orders =>
            {
                var hired = orders
                    .Where(o => o.Type is OrderType.Hire)
                    .GroupBy(o => o.StartDate.Date)
                    .Select(g => new {Date = g.Key, Count = g.Count()})
                    .ToList();

                var fired = orders
                    .Where(o => o.Type is OrderType.Fire)
                    .GroupBy(o => o.StartDate.Date)
                    .Select(g => new {Date = g.Key, Count = g.Count()})
                    .ToList();

                var hireSeries = new ColumnSeries<int>
                {
                    Values = hired.Select(g => g.Count).ToList(),
                    Name = "Нанято"
                };

                var fireSeries = new ColumnSeries<int>
                {
                    Values = fired.Select(g => g.Count).ToList(),
                    Name = "Уволено"
                };

                var dates = hired.Select(x => x.Date)
                    .Concat(fired.Select(x => x.Date))
                    .Distinct()
                    .Select(x => x.ToDateString())
                    .ToList();

                EmployeesHireCartesianSeries.Add(hireSeries);
                EmployeesHireCartesianSeries.Add(fireSeries);

                EmployeeHireCartesianXAxes.Add(new Axis { Labels = dates });
            } );
    }

    private async Task DrawEmployeesDepartmentPositionChart()
    {
        EmployeesDepartmentPositionPieSeries.Clear();

        var employeesResource = await employeeUseCase.GetAllAsync();
        await HandleResource(
            employeesResource,
            employees =>
            {
                var workingEmployees = employees.Where(e => e.IsWorking);

                switch (SelectedEmployeeSeriesType)
                {
                    case StatisticsEmployeeSeriesType.Department:
                        var departmentsCount = workingEmployees
                            .GroupBy(e => e.Department!.Title)
                            .Select(g => new { Type = g.Key!, Count = g.Count() })
                            .ToList();

                        foreach (var g in departmentsCount)
                        {
                            var pieSeries = new PieSeries<int>(g.Count) { Name = g.Type };
                            EmployeesDepartmentPositionPieSeries.Add(pieSeries);
                        }

                        break;
                    case StatisticsEmployeeSeriesType.Position:
                        var positionsCount = workingEmployees
                            .GroupBy(e => e.Position!.Title)
                            .Select(g => new { Type = g.Key!, Count = g.Count() })
                            .ToList();

                        foreach (var g in positionsCount)
                        {
                            var pieSeries = new PieSeries<int>(g.Count) { Name = g.Type };
                            EmployeesDepartmentPositionPieSeries.Add(pieSeries);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
    }

    private async Task DrawNewOrdersChart()
    {
        var currentDate = DateTime.Now;
        var lastWeekDate = currentDate.AddDays(-7);

        var ordersResource = await orderUseCase.GetAllAsync(o =>
            o.Date >= lastWeekDate
            && o.Date <= currentDate);

        await HandleResource(
            ordersResource,
            orders =>
            {
                var groupedByDate = orders
                    .GroupBy(o => o.Date.Date)
                    .Select(g => new {Date = g.Key, Count = g.Count()})
                    .ToList();

                var values = groupedByDate.Select(o => o.Count).ToList();
                var dates = groupedByDate.Select(o => o.Date.ToDateString()).ToList();

                var series = new LineSeries<int>(values);
                var axis = new Axis {Labels = dates};

                NewOrdersCartesianSeries.Add(series);
                NewOrdersCartesianXAxes.Add(axis);
            });
    }

    private async Task CalculateStatistics()
    {
        var orderResource = await orderUseCase.GetAllAsync();
        var employeeResource = await employeeUseCase.GetAllAsync();
        var departmentResource = await departmentUseCase.GetAllAsync();
        var positionResource = await positionUseCase.GetAllAsync();

        await HandleResource(
            orderResource,
            orders =>
            {
                OrdersCount = orders.Count;
                var hireOrdersCount = orders.Count(o => o.Type is OrderType.Hire);
                var fireOrdersCount = orders.Count(o => o.Type is OrderType.Fire);
                HireToFireRate = (double)fireOrdersCount / hireOrdersCount;
            });
        await HandleResource(
            employeeResource,
            employees => EmployeesCount = employees.Count);
        await HandleResource(
            departmentResource,
            departments => DepartmentsCount = departments.Count);
        await HandleResource(
            positionResource,
            positions =>
            {
                PositionsCount = positions.Count;
                MostPopularPosition = positions
                    .Select(p => new { Position = p, OrderCount = p.Orders.Count })
                    .OrderByDescending(g => g.OrderCount)
                    .FirstOrDefault()
                    ?.Position.Title;
            });
    }
}