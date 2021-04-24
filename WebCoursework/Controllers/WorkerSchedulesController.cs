using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCoursework;

namespace WebCoursework.Controllers
{
    public class WorkerSchedulesController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public WorkerSchedulesController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: WorkerSchedules
        public async Task<IActionResult> Index()
        {
            var dentalClinicDBContext = _context.WorkerSchedules.Include(w => w.Schedule).Include(w => w.Worker);
            return View(await dentalClinicDBContext.ToListAsync());
        }

        // GET: WorkerSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workerSchedule = await _context.WorkerSchedules
                .Include(w => w.Schedule)
                .Include(w => w.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workerSchedule == null)
            {
                return NotFound();
            }

            return View(workerSchedule);
        }

        // GET: WorkerSchedules/Create
        public IActionResult Create()
        {
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "ScheduleId", "ScheduleId");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address");
            return View();
        }

        // POST: WorkerSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkerId,ScheduleId,CreatedDateTime,LastModifiedDateTime")] WorkerSchedule workerSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workerSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "ScheduleId", "ScheduleId", workerSchedule.ScheduleId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", workerSchedule.WorkerId);
            return View(workerSchedule);
        }

        // GET: WorkerSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workerSchedule = await _context.WorkerSchedules.FindAsync(id);
            if (workerSchedule == null)
            {
                return NotFound();
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "ScheduleId", "ScheduleId", workerSchedule.ScheduleId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", workerSchedule.WorkerId);
            return View(workerSchedule);
        }

        // POST: WorkerSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkerId,ScheduleId,CreatedDateTime,LastModifiedDateTime")] WorkerSchedule workerSchedule)
        {
            if (id != workerSchedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workerSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerScheduleExists(workerSchedule.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "ScheduleId", "ScheduleId", workerSchedule.ScheduleId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", workerSchedule.WorkerId);
            return View(workerSchedule);
        }

        // GET: WorkerSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workerSchedule = await _context.WorkerSchedules
                .Include(w => w.Schedule)
                .Include(w => w.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workerSchedule == null)
            {
                return NotFound();
            }

            return View(workerSchedule);
        }

        // POST: WorkerSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workerSchedule = await _context.WorkerSchedules.FindAsync(id);
            _context.WorkerSchedules.Remove(workerSchedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerScheduleExists(int id)
        {
            return _context.WorkerSchedules.Any(e => e.Id == id);
        }
    }
}
