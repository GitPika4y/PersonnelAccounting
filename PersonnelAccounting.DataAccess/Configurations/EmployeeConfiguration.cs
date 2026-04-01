using Data.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class EmployeeConfiguration: IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable(t =>
        {
            // Passport Regex "####-######"
            t.HasCheckConstraint(
                "CH_Employee_Passport",
                "LEN([Passport]) = 11 AND [Passport] LIKE '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][0-9][0-9]'");

            t.HasCheckConstraint(
                "CH_Employee_Inn",
                "LEN([Inn]) IN (12, 14)");

            t.HasCheckConstraint(
                "CH_Employee_PhoneNumber",
                "LEN([PhoneNumber]) BETWEEN 10 AND 14");

            t.HasCheckConstraint(
                "CH_Employee_BirthDate",
                "[BirthDate] <= GETDATE()");
        });
    }
}