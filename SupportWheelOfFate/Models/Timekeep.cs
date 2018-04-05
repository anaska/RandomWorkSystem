using System;

namespace SupportWheelOfFate.Models
{
    public class Timekeep
    {
        public int Id { get; set; }

        public int CurrentWeek { get; set; }

        public int CurrentDay { get; set; }

        public DateTime CurrentDateTime { get; set; }
    }
}