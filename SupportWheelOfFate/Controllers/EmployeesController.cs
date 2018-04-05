using Newtonsoft.Json;
using SupportWheelOfFate.DTOs;
using SupportWheelOfFate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SupportWheelOfFate.Controllers
{
    public class EmployeesController : ApiController
    {
        AppDbContext dbContext;
        const string connString = @"Data Source=DESKTOP-GL02KLJ\SQLEXPRESS;Initial Catalog = worksystem; Integrated Security = True";

        public EmployeesController()
        {
            dbContext = new AppDbContext();
        }

        [HttpGet]
        [Route("api/employees/getemployees")]
        public List<Employee> GetEmployees()
        {
            List<Employee> employees = dbContext.Employees.ToList();

            return employees;
        }

        [HttpGet]
        [Route("api/employees/getworklog")]
        public List<WorkLog> GetWorkLog()
        {
            return dbContext.WorkLogs.ToList();
        }

        [HttpGet]
        [Route("api/employees/getdayandweek")]
        public Timekeep GetDayAndWeek()
        {
            var timekeep = dbContext.Timekeep.SingleOrDefault();

            return timekeep;
        }

        [HttpGet]
        [Route("api/employees/getrules")]
        public WorkRule GetRules()
        {
            return dbContext.WorkRules.SingleOrDefault();
        }

        [HttpGet]
        [Route("api/employees/getnextemployees")]
        public List<Employee> GetNextEmployees()
        {
            Timekeep timekeep = dbContext.Timekeep.SingleOrDefault();
            WorkRule workRule = dbContext.WorkRules.SingleOrDefault();

            //Joining Employees table with WorkLogs table in order to select the employees which have worked in the given interval of days
            var query = from e in dbContext.Employees
                        join w in dbContext.WorkLogs on e.Id equals w.EmployeeId
                        where DbFunctions.DiffDays(timekeep.CurrentDateTime, w.LastDayWorked) >= workRule.WorkDaysInterval
                        select new EmployeeDto { Id = e.Id, Name = e.Name };

            var result = query.ToList();

            List<Employee> sidedEmployees = new List<Employee>();

            //Convert from DTO to DO
            foreach (var e in result)
            {
                sidedEmployees.Add(new Employee { Id = e.Id, Name = e.Name });
            }

            //Excluding from all employees list the ones which are not fit to work yet
            List<Employee> employees = dbContext.Employees.ToList().Except(sidedEmployees).ToList();

            //Get EmployeeIds for employees which have completed minimum days per weeks work interval(Quota)
            List<int> quotaIds = new List<int>();
            //Prepare SQL connection
            using (var conn = new SqlConnection(connString))
            //Prepare SQL command
            using (var command = new SqlCommand("dbo.GetEmployeeIdsWorked", conn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                command.Parameters.Add(new SqlParameter("@Week", timekeep.CurrentWeek));
                command.Parameters.Add(new SqlParameter("@PeriodOfWeeks", workRule.PeriodOfWeeks));
                command.Parameters.Add(new SqlParameter("@MinimumDays", workRule.MinimumDaysPerPeriod));
                conn.Open();

                //Store list of Ids
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        quotaIds.Add(Convert.ToInt32(dataReader["EmployeeId"]));
                    }
                }
            }

            //Remove Employees with id found in the quota list if Ids
            employees.RemoveAll((Employee employee) => { return quotaIds.Contains(employee.Id); });

            var rand = new Random();
            //Randomly choosing 2 employees from the remaining list of employees
            var chosenEmployees = employees.OrderBy(e => rand.Next()).Take(2).ToList();

            foreach (var e in chosenEmployees)
            {
                //Each of the selected employees will have a work log added for that day
                dbContext.WorkLogs.Add(new WorkLog
                {
                    Day = timekeep.CurrentDay,
                    Week = timekeep.CurrentWeek,
                    EmployeeId = e.Id,
                    LastDayWorked = timekeep.CurrentDateTime
                });

                dbContext.WorkedDay.Add(new DaysWorked
                {
                    EmployeeId = e.Id,
                    Week = timekeep.CurrentWeek,
                    DaysOfWork = workRule.WorkShiftSize
                });
            }

            //Increment Week counter and reset Day counter if it is the last day in the week
            if (timekeep.CurrentDay == 5)
            {
                timekeep.CurrentWeek++;
                timekeep.CurrentDay = 1;
            }
            else
            {
                timekeep.CurrentDay++;
            }

            //Increase DateTime counter for work days and for weekend
            timekeep.CurrentDateTime = timekeep.CurrentDateTime.AddDays(1);

            if (timekeep.CurrentDateTime.DayOfWeek == DayOfWeek.Friday)
            {
                timekeep.CurrentDateTime = timekeep.CurrentDateTime.AddDays(2);
            }

            dbContext.SaveChanges();

            return chosenEmployees;
        }

        [HttpPost]
        [Route("api/employees/changerules")]
        public WorkRule ChangeRules(WorkRule workRule)
        {
            //WorkRule workRule = JsonConvert.DeserializeObject<WorkRule>(rules);
            var oldRules = dbContext.WorkRules.SingleOrDefault();
            oldRules.MinimumDaysPerPeriod = workRule.MinimumDaysPerPeriod;
            oldRules.PeriodOfWeeks = workRule.PeriodOfWeeks;
            oldRules.WorkDaysInterval = workRule.WorkDaysInterval;
            oldRules.WorkShiftSize = workRule.WorkShiftSize;
            dbContext.SaveChanges();

            return workRule;
        }
    }
}
