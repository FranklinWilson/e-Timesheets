using Microsoft.AspNetCore.Mvc.Rendering;

namespace eTimesheet.Models
{
    public class TimesheetList
    {
        public List<Timesheet> Timesheets { get; set; }
        public Timesheet NewTimesheet { get; set; } = new Timesheet();
        public SelectList EmployeeList { get; set; }
        public SelectList JobList { get; set; }
    }
}
