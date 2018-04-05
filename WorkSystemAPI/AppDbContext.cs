using SupportWheelOfFate.Models;
using System.Data.Entity;

namespace SupportWheelOfFate
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
          : base("worksystem")
        {
        }

        public AppDbContext(string connectionStringName)
          : base("worksystem")
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<WorkLog> WorkLogs { get; set; }

        public DbSet<Timekeep> Timekeep { get; set; }

        public DbSet<WorkRule> WorkRules { get; set; }

        public DbSet<DaysWorked> WorkedDay { get; set; }
    }
}