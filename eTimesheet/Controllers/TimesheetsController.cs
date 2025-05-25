using eTimesheet.Data;
using eTimesheet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTimesheet.Controllers
{
    public class TimesheetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimesheetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? employeeId, int? jobId)
        {
            var employees = await _context.Employees.ToListAsync();
            var jobs = await _context.Jobs.ToListAsync();

            var timesheetsQuery = _context.Timesheets
                .Include(t => t.Employee)
                .Include(t => t.Job)
                .AsQueryable();

            if (employeeId.HasValue)
                timesheetsQuery = timesheetsQuery.Where(t => t.EmployeeId == employeeId.Value);

            if (jobId.HasValue)
                timesheetsQuery = timesheetsQuery.Where(t => t.JobId == jobId.Value);

            var timesheets = await timesheetsQuery.ToListAsync();

            var model = new TimesheetList
            {
                Timesheets = timesheets,
                EmployeeList = new SelectList(employees, "Id", "Name", employeeId),
                JobList = new SelectList(jobs, "Id", "Name", jobId)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTimesheet([Bind("EmployeeId,JobId,HoursWorked,Date")] Timesheet newTimesheet)
        {
            if (ModelState.IsValid)
            {
                _context.Timesheets.Add(newTimesheet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var employees = await _context.Employees.ToListAsync();
            var jobs = await _context.Jobs.ToListAsync();
            var timesheets = await _context.Timesheets
                .Include(t => t.Employee)
                .Include(t => t.Job)
                .ToListAsync();

            var model = new TimesheetList
            {
                Timesheets = timesheets,
                NewTimesheet = newTimesheet,
                EmployeeList = new SelectList(employees, "Id", "Name", newTimesheet.EmployeeId),
                JobList = new SelectList(jobs, "Id", "Name", newTimesheet.JobId)
            };
            return View("Index", model);
        }
    }
}
