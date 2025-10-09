using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AutomotiveApp.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // hanya utk migration

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // Replace with your actual connection string
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AutomotiveAppDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
