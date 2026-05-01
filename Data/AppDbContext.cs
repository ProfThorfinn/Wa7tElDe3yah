using Microsoft.EntityFrameworkCore;
using Wa7at_ElDr3yah_API.Models;

namespace Wa7at_ElDr3yah_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Capital> Capitals { get; set; }
        public DbSet<MonthlyReport> MonthlyReports { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
