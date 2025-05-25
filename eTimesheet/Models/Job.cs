using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace eTimesheet.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Name")]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public int ExpectedHours { get; set; }

        public ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();

        public double TimeRemaining
        {
            get
            {
                var totalWorked = Timesheets?.Sum(ts => ts.HoursWorked) ?? 0;
                return ExpectedHours - totalWorked;
            }
        }
    }
}
