namespace eTimesheet.Models
{
    public class EmployeeList
    {
        public List<Employee> Employees { get; set; }
        public Employee NewEmployee { get; set; } = new Employee();
    }
}
