namespace WorkSystem.Models
{
    public class WorkRule
    {
        public int Id { get; set; }

        public float WorkShiftSize { get; set; }

        public int WorkDaysInterval { get; set; }

        public int PeriodOfWeeks { get; set; }

        public int MinimumDaysPerPeriod { get; set; }
    }
}