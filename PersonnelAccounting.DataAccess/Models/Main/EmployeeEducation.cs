namespace Data.Models.Main;

public class EmployeeEducation: EntityModel
{
    public required Qualification Qualification { get; set; }
    public required Specialization Specialization { get; set; }
    public required int GraduationYear { get; set; }

    public virtual Employee Employee { get; set; }
}