using System.ComponentModel.DataAnnotations;

namespace eTimesheet.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Wage { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public static implicit operator Employee(Job v)
        {
            throw new NotImplementedException();
        }
    }
}
