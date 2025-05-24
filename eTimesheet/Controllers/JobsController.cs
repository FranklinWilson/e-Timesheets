using eTimesheet.Data;
using eTimesheet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTimesheet.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IActionResult Index()
        {
            return View();
        }

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Jobs()
        {
            var model = new JobList
            {
                Jobs = await _context.Jobs.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJob([Bind("Name,ExpectedHours")] Job newJob)
        {
            if (ModelState.IsValid)
            {
                _context.Jobs.Add(newJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Jobs));
            }

            // Reload the job list and pass the error state
            var model = new JobList
            {
                Jobs = await _context.Jobs.ToListAsync(),
                NewJob = newJob
            };
            return View("Jobs", model);
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
            return RedirectToAction(nameof(Jobs));
        }
    }
}
