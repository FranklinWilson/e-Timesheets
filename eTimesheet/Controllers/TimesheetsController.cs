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
        private readonly IEmailService _emailService;


        public TimesheetsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(int? employeeId, int? jobId, DateTime? startDate, DateTime? endDate)
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

            if (startDate.HasValue)
                timesheetsQuery = timesheetsQuery.Where(t => t.Date >= startDate.Value);

            if (endDate.HasValue)
                timesheetsQuery = timesheetsQuery.Where(t => t.Date <= endDate.Value);

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
                if (newTimesheet.HoursWorked > 10)
                {
                    var employee = await _context.Employees.FindAsync(newTimesheet.EmployeeId);
                    var job = await _context.Jobs.FindAsync(newTimesheet.JobId);

                    var emailBody = $"""
                                        Employee: {employee?.Name}
                                        Job: {job?.Name}
                                        Date: {newTimesheet.Date.ToShortDateString()}
                                        Hours Worked: {newTimesheet.HoursWorked}
                                    """;

                    await _emailService.SendEmailAsync(
                        to: "example@email.com",
                        subject: "Overtime Alert",
                        body: emailBody
                    );
                }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTimesheet(int id)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet != null)
            {
                _context.Timesheets.Remove(timesheet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
