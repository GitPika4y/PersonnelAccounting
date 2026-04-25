using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using Data.Services.Main;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.User;

public partial class OrderAddModalViewModel: ViewModelPagination<Employee>
{
    public bool HasEmployee { get; }
    private readonly IEmployeeUseCase _employeeUseCase;
    private readonly IOrderTypeService _orderTypeService;

    [ObservableProperty] [Required] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private DateTime _date = DateTime.Now;
    [ObservableProperty] [Required] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private DateTime _startDate = DateTime.Now;
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private DateTime? _endDate;

    public ObservableCollection<OrderType> OrderTypes { get; } = [];
    [ObservableProperty] [Required] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private OrderType _selectedOrderType;

    public ObservableCollection<Employee> Employees { get; } = [];
    [ObservableProperty] [Required] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private Employee? _selectedEmployee;

    public ObservableCollection<Position> Positions { get; } = [];
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private Position? _selectedPosition;

    public ObservableCollection<Department> Departments { get; } = [];
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private Department? _selectedDepartment;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string _fireReason;

    partial void OnSelectedEmployeeChanged(Employee? value) => ChangeAvailableOrderTypes();

    private void ChangeAvailableOrderTypes()
    {
        if (SelectedEmployee is null)
            return;

        var availableOrderTypes = _orderTypeService.GetAvailable(SelectedEmployee);
        UpdateObservableCollection(OrderTypes, availableOrderTypes.ToList());
        SelectedOrderType = OrderTypes.First();
    }

    protected override async Task UpdatePaginationCollection()
    {
        var resource = await _employeeUseCase.GetAllAsync(SelectedPage, SelectedPageSize);
        await HandleResource(
            resource,
            paginationModel =>
            {
                Pagination = paginationModel;
                UpdateObservableCollection(Employees, paginationModel.Items);
            });
    }

    public OrderAddModalViewModel(IOrderTypeService orderTypeService, ICollection<Position> positions, ICollection<Department> departments)
    {
        _orderTypeService = orderTypeService;

        UpdateObservableCollection(Positions, positions.ToList());
        UpdateObservableCollection(Departments, departments.ToList());
    }

    public OrderAddModalViewModel(IEmployeeUseCase employeeUseCase, IOrderTypeService orderTypeService, ICollection<Position> positions, ICollection<Department> departments)
        : this(orderTypeService,  positions, departments)
    {
        _employeeUseCase = employeeUseCase;
        _ = UpdatePaginationCollection();
    }

    public OrderAddModalViewModel(Employee employee, IOrderTypeService orderTypeService, ICollection<Position> positions, ICollection<Department> departments)
        : this(orderTypeService, positions, departments)
    {
        SelectedEmployee = employee;
        HasEmployee = true;
    }

    private bool CanSave()
    {
        return ValidateByOrderType()
               && ValidateDates()
               && CheckProperties();
    }

    private bool ValidateByOrderType()
    {
        return SelectedOrderType switch
        {
            OrderType.Hire => SelectedDepartment is not null && SelectedPosition is not null,
            OrderType.Fire => !string.IsNullOrEmpty(FireReason),
            _ => true
        };
    }

    private bool ValidateDates()
    {
        // Date <= StartDate <= EndDate
        return StartDate >= Date
            && EndDate is null || EndDate >= StartDate;
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        Resource<Order> resource;
        try
        {
            var order = BuildOrder();
            resource = Resource<Order>.Success(order);
        }
        catch (Exception e)
        {
            resource = Resource<Order>.Failure(e.Message, e);
        }

        DialogHost.CloseDialogCommand.Execute(resource, null);
    }

    private Order BuildOrder()
    {
        if (SelectedEmployee is null)
        {
            throw new Exception("'Сотрудник' не был выбран, при создании 'приказа'");
        }

        var order = new Order
        {
            EmployeeId = SelectedEmployee.Id,
            Type = SelectedOrderType,
            Date = Date,
            StartDate = StartDate,
            EndDate = EndDate
        };

        switch (SelectedOrderType)
        {
            case OrderType.Hire:
                if (SelectedDepartment is null
                    || SelectedPosition is null)
                {
                    throw new Exception("'Должность' или 'Отдел' не были выбраны при создании 'приказа о найме'");
                }

                order.HireDepartmentId = SelectedDepartment.Id;
                order.HirePositionId = SelectedPosition.Id;
                break;

            case OrderType.Fire:
                if (string.IsNullOrEmpty(FireReason))
                {
                    throw new Exception("Не была указана 'Причина увольнения' при создании 'приказа об увольнении'");
                }

                order.FireReason = FireReason;
                break;

            case OrderType.StudyLeave:
            case OrderType.Vacation:
            case OrderType.BusinessTrip:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return order;
    }
}