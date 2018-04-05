using System;

namespace SupportWheelOfFate.Models
{
    public class WorkLog
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int Day { get; set; }

        public int Week { get; set; }

        public DateTime LastDayWorked { get; set; }
    }
}