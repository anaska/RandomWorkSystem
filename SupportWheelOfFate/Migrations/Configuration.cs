namespace SupportWheelOfFate.Migrations
{
    using SupportWheelOfFate.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Employees.AddOrUpdate(
                new Employee() { Id = 1, Name = "Peter Dinklage" },
                new Employee() { Id = 2, Name = "Lena Headey" },
                new Employee() { Id = 3, Name = "Emilia Clarke" },
                new Employee() { Id = 4, Name = "Kit Harington" },
                new Employee() { Id = 5, Name = "Sophie Turner" },
                new Employee() { Id = 6, Name = "Maisie Williams" },
                new Employee() { Id = 7, Name = "Iain Glen" },
                new Employee() { Id = 8, Name = "Alfie Allen" },
                new Employee() { Id = 9, Name = "John Bradley" },
                new Employee() { Id = 10, Name = "Liam Cunningham" });

            context.Timekeep.Add(
                new Timekeep() { Id = 1, CurrentDay = 1, CurrentWeek = 1, CurrentDateTime = DateTime.Now });

            context.WorkRules.Add(
                new WorkRule() { Id = 1, PeriodOfWeeks = 2, WorkDaysInterval = 2, WorkShiftSize = 0.5f,
                    MinimumDaysPerPeriod = 1 });
        }
    }
}
