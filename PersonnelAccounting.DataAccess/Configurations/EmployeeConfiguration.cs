using Data.Extensions;
using Data.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class EmployeeConfiguration: IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Gender)
            .HasConversion<string>();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CH_Employee_Inn",
                "LEN([Inn]) IN (12, 14)");

            t.HasCheckConstraint(
                "CH_Employee_PhoneNumber",
                "LEN([PhoneNumber]) BETWEEN 10 AND 16");

            t.HasCheckConstraint(
                "CH_Employee_BirthDate",
                "[BirthDate] <= GETDATE()");

            t.HasCheckConstraint(
                "CH_Employee_Gender",
                EnumExtensions.EnumToSqlQuery<Employee>(typeof(EmployeeGender), e => e.Gender));
        });
    }
}