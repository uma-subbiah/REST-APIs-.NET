using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EmployeeApi.Models
{
    // DbContext is like the database, DbSets are like the tables in the database -> 1 DbContext can have many DbSets
    public class EmployeeContext : DbContext 
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;
    }
}