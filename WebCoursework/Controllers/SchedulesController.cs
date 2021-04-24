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
    public class SchedulesController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public SchedulesController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var dentalClinicDBContext = _context.Schedules.Include(s => s.Day).Include(s => s.TimeSegment);
            return View(await dentalClinicDBContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Day)
                .Include(s => s.TimeSegment)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "Name");
            ViewData["TimeSegmentId"] = new SelectList(_context.TimeSegments, "TimeSegmentId", "TimeSegmentId");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,DayId,TimeSegmentId,CreatedDateTime,LastModifiedDateTime")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "Name", schedule.DayId);
            ViewData["TimeSegmentId"] = new SelectList(_context.TimeSegments, "TimeSegmentId", "TimeSegmentId", schedule.TimeSegmentId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "Name", schedule.DayId);
            //ViewData["TimeSegmentId"] = new SelectList(
            //    _context.TimeSegments
            //    .Select(t=>new 
            //    { 
            //TimeSegmentId = id,
            //    CompoundSegment = $"{t.TimeStart} - {t.TimeEnd}"
            //    }), "TimeSegmentId", "TimeSegmentId", schedule.TimeSegmentId);
            ViewData["TimeSegmentId"] = new SelectList(
                _context.TimeSegments
                .Select(t => new
                {
                    TimeSegmentId = id,
                    CompoundSegment = $"{t.TimeStart} - {t.TimeEnd}"
                }), "TimeSegmentId", "CompoundSegment", schedule.TimeSegmentId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,DayId,TimeSegmentId,CreatedDateTime,LastModifiedDateTime")] Schedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.ScheduleId))
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
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "Name", schedule.DayId);
            ViewData["TimeSegmentId"] = new SelectList(_context.TimeSegments, "TimeSegmentId", "TimeSegmentId", schedule.TimeSegmentId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Day)
                .Include(s => s.TimeSegment)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.ScheduleId == id);
        }
    }
}
