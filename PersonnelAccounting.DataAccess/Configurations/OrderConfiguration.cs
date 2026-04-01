using Data.Extensions;
using Data.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CH_Order_StartDate_EndDate",
                "[StartDate] <= [EndDate]");

            t.HasCheckConstraint(
                "CH_Order_Date_StartDate",
                "[Date] <= [StartDate]");

            t.HasCheckConstraint(
                "CH_Order_Date",
                "[Date] >= GETDATE()");

            t.HasCheckConstraint(
                "CH_Order_Type",
                EnumExtensions.EnumToSqlQuery<Order>(typeof(OrderType), o => o.Type));
        });

        builder.Property(o => o.Type)
            .HasConversion<string>();

        builder.HasOne(o => o.Employee)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.EmployeeId);

        builder.HasOne(o => o.HireDepartment)
            .WithMany(d => d.Orders)
            .HasForeignKey(o => o.HireDepartmentId);

        builder.HasOne(o => o.HirePosition)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.HirePositionId);
    }
}