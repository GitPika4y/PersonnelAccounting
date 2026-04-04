using Data.Extensions;
using Data.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CH_User_Role",
                EnumExtensions.EnumToSqlQuery<User>(typeof(UserRole), u => u.Role));
        });

        builder.Property(u => u.Role)
            .HasConversion<string>();

        builder.HasData(
            new User
            {
                Id = Guid.NewGuid(),
                Role = UserRole.Admin,
                Login = "admin",
                Password = "$2a$11$OswhLeNo0PGwGSGnI0RwTONRZtZUfgw656L0CbJtjY0/L00pvpyea" // admin123
            });
    }
}