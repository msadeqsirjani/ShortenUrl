using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShortUrl.Repository.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            const string connection = @"Data Source=localhost;database=ShortUrlDb;Trusted_Connection=True;MultipleActiveResultSets=true";

            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer(connection);

            return new ApplicationDbContext(optionBuilder.Options);
        }
    }
}
