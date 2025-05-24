using System.ComponentModel.DataAnnotations;

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
    }
}
