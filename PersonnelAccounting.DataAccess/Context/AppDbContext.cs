using Data.Configurations;
using Data.Models.Auth;
using Data.Models.Main;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Position> Positions { get; set; }
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<EmployeeEducation> EmployeeEducations { get; set; }
    public virtual DbSet<EmployeePassport> EmployeePassports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeEducation).Assembly);
    }
}