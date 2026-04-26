using Data.Extensions;
using Data.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class EmployeeEducationConfiguration: IEntityTypeConfiguration<EmployeeEducation>
{
    public void Configure(EntityTypeBuilder<EmployeeEducation> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Qualification)
            .HasConversion<string>();

        builder.Property(e => e.Specialization)
            .HasConversion<string>();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CH_EmployeeEducation_Qualification",
                EnumExtensions.EnumToSqlQuery<EmployeeEducation>(typeof(Qualification), e => e.Qualification));

            t.HasCheckConstraint(
                "CH_EmployeeEducation_Specialization",
                EnumExtensions.EnumToSqlQuery<EmployeeEducation>(typeof(Specialization), e => e.Specialization));
        });
    }
}