using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Models
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
            // Add-Migration "FirstCreate" -Context EmployeeManager.Models.EmployeeDbContext
            // Update-Database -Context EmployeeManager.Models.EmployeeDbContext
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeHistory> EmployeesHistories { get; set; }
    }
}
