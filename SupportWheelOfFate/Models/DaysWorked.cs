namespace SupportWheelOfFate.Models
{
    public class DaysWorked
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int Week { get; set; }

        public float DaysOfWork { get; set; }
    }
}