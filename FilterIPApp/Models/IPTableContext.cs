using Microsoft.EntityFrameworkCore;

namespace FilterIPApp.Models
{
    public class IPTableContext : DbContext
    {
        public DbSet<IPTable> IPTables { get; set; }
        public IPTableContext(DbContextOptions<IPTableContext> options)
            : base(options)
        {
            Database.EnsureCreated();// Create database by EF Core Model First, if its not exists
        }
    }
}
