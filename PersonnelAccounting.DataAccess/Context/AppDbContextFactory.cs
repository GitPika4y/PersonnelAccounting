using Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Context;

public class AppDbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = DotEnvExtensions.GetConnectionString();

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(connectionString);

        return new AppDbContext(builder.Options);
    }
}