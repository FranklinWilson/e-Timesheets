using System.ComponentModel.DataAnnotations;

namespace eTimesheet.Models
{
    public class Timesheet
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int JobId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double HoursWorked { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public Employee? Employee { get; set; }
        public Job? Job { get; set; }
    }
}