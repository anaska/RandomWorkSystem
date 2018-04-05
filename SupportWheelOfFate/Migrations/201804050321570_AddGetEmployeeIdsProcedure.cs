namespace SupportWheelOfFate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGetEmployeeIdsProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE PROCEDURE dbo.GetEmployeeIdsWorked @Week int, @PeriodOfWeeks int, @MinimumDays int
                AS
                SELECT result.EmployeeId
                FROM(SELECT DISTINCT EmployeeId, SUM(DaysOfWork) AS TotalDays

                    FROM worksystem.dbo.DaysWorkeds

                    WHERE Week BETWEEN @Week - @PeriodOfWeeks + 1 AND @Week

                    GROUP BY EmployeeId) result
                WHERE result.TotalDays >= @MinimumDays");
        }
        
        public override void Down()
        {
        }
    }
}
