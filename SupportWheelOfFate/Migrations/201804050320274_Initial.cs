namespace SupportWheelOfFate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Timekeeps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentWeek = c.Int(nullable: false),
                        CurrentDay = c.Int(nullable: false),
                        CurrentDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DaysWorkeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Week = c.Int(nullable: false),
                        DaysOfWork = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Day = c.Int(nullable: false),
                        Week = c.Int(nullable: false),
                        LastDayWorked = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkShiftSize = c.Single(nullable: false),
                        WorkDaysInterval = c.Int(nullable: false),
                        PeriodOfWeeks = c.Int(nullable: false),
                        MinimumDaysPerPeriod = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WorkRules");
            DropTable("dbo.WorkLogs");
            DropTable("dbo.DaysWorkeds");
            DropTable("dbo.Timekeeps");
            DropTable("dbo.Employees");
        }
    }
}
