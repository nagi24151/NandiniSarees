using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NandiniSareesAPIs.Models
{
    // Provides a design-time factory so 'dotnet ef' can create the DbContext when tooling is invoked
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NandiniSareesDbContext>
    {
        public NandiniSareesDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NandiniSareesDbContext>();
            // Use an environment variable or fallback to localdb for development
            var conn = Environment.GetEnvironmentVariable("NANDINI_CONNECTION")
                       ?? "Server=(localdb)\\mssqllocaldb;Database=NandiniSareesDb;Trusted_Connection=True;MultipleActiveResultSets=true";
            builder.UseSqlServer(conn);
            return new NandiniSareesDbContext(builder.Options);
        }
    }
}
