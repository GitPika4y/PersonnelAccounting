using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.User;

public partial class EmployeeAddEditModalViewModel: ViewModelBase
{
    public string Title { get; } = "Добавить сотрудника";
    public string ConfirmButtonText { get; } = "Добавить";

    public ICollection<EmployeeGender> Genders { get; } = Enum.GetValues<EmployeeGender>();
    public ICollection<Qualification> Qualifications { get; } = Enum.GetValues<Qualification>();
    public ICollection<Specialization> Specializations { get; } = Enum.GetValues<Specialization>();

    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string _lastName = null!;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string _firstName = null!;
    [ObservableProperty] private string? _middleName;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private EmployeeGender _gender;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] [Range(typeof(DateTime), "1900-01-01", "2026-01-01")] private DateTime _birthDate;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string _phoneNumber = null!;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] [RegularExpression(@"^\d{12}$", ErrorMessage = "ИНН должен содержать 12 цифр")] private string _inn = null!;

    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] [RegularExpression(@"^\d{4}$", ErrorMessage = "Формат: 1234")] private string _passportSerial = null!;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] [RegularExpression(@"^\d{6}$", ErrorMessage = "Формат: 123456")] private string _passportNumber = null!;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] [Range(typeof(DateTime), "1900-01-01", "2026-01-01")] private DateTime _passportDate;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string _passportGivenBy = null!;

    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private Qualification _qualification;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private Specialization _specialization;
    [ObservableProperty] [Required(ErrorMessage = "Обязательное поле")] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] [Range(1976, 2026)] private int _graduationYear;

    private readonly Guid? _id;
    private readonly Guid? _passportId;
    private readonly Guid? _educationId;

    public EmployeeAddEditModalViewModel(Employee? employee = null)
    {
        if (employee is null)
            return;

        Title = "Редактировать сотрудника";
        ConfirmButtonText = "Редактированить";

        _id = employee.Id;
        _lastName = employee.LastName;
        _firstName = employee.FirstName;
        _middleName = employee.MiddleName;
        _birthDate = employee.BirthDate;
        _phoneNumber = employee.PhoneNumber;
        _inn = employee.Inn;
        _gender = employee.Gender;
        _passportId = employee.PassportId;
        _passportDate = employee.Passport.Date;
        _passportGivenBy = employee.Passport.GivenBy;
        _passportNumber = employee.Passport.Number;
        _passportSerial = employee.Passport.Serial;
        _educationId = employee.EducationId;
        _qualification = employee.Education.Qualification;
        _specialization = employee.Education.Specialization;
        _graduationYear = employee.Education.GraduationYear;
    }

    private bool CanSave() => CheckProperties();

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        Resource<Employee> resource;
        try
        {
            var employee = BuildEmployee();
            resource = Resource<Employee>.Success(employee);
        }
        catch (Exception e)
        {
            resource = Resource<Employee>.Failure(e.Message, e);
        }

        DialogHost.CloseDialogCommand.Execute(resource, null);
    }

    private Employee BuildEmployee()
    {
        var employee = new Employee
        {
            LastName = LastName,
            FirstName = FirstName,
            MiddleName = MiddleName,
            BirthDate = BirthDate,
            PhoneNumber = PhoneNumber,
            Inn = Inn,
            Gender = Gender,
            Passport = new EmployeePassport
            {
                Serial = PassportSerial,
                Number = PassportNumber,
                Date = PassportDate,
                GivenBy = PassportGivenBy
            },
            Education = new EmployeeEducation
            {
                Qualification = Qualification,
                Specialization = Specialization,
                GraduationYear = GraduationYear
            }
        };

        if (_id is not null)
            employee.Id = _id.Value;

        if (_educationId is not null)
        {
            employee.Education.Id = _educationId.Value;
            employee.EducationId = _educationId.Value;
        }

        if (_passportId is not null)
        {
            employee.Passport.Id = _passportId.Value;
            employee.PassportId = _passportId.Value;
        }

        return employee;
    }
}