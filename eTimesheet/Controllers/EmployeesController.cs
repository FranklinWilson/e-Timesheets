using eTimesheet.Data;
using eTimesheet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTimesheet.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Employees()
        {
            var model = new EmployeeList
            {
                Employees = await _context.Employees.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee([Bind("Name,Wage,Email")] Employee newEmployee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Employees));
            }

            // Reload the employee list and pass the error state
            var model = new EmployeeList
            {
                Employees = await _context.Employees.ToListAsync(),
                NewEmployee = newEmployee
            };
            return View("Employees", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Employees));
        }
    }
}
