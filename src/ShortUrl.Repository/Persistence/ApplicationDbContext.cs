using Microsoft.EntityFrameworkCore;
using ShortUrl.Domain.Entities;

namespace ShortUrl.Repository.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UrlInfo> UrlInfos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
