using eTimesheet.Data;
using eTimesheet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTimesheet.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public async Task<IActionResult> Index()
        {
            var model = new JobList
            {
                Jobs = await _context.Jobs.ToListAsync()
            };
            return View(model);
        }

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJob([Bind("Name,ExpectedHours")] Job newJob)
        {
            if (ModelState.IsValid)
            {
                _context.Jobs.Add(newJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload the job list and pass the error state
            var model = new JobList
            {
                Jobs = await _context.Jobs.ToListAsync(),
                NewJob = newJob
            };
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
