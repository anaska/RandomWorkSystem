using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkSystem.Models;

namespace WorkSystem.ViewModels
{
    public class EmployeesVewModel
    {
        public Timekeep Timekeep { get; set; }

        public List<Employee> Employees { get; set; }
    }
}