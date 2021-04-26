using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCoursework;
using WebCoursework.Models.SortStates;

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
        public async Task<IActionResult> Index(string workerFirstName, string workerLastName, string position, string day, TimeSpan? startTime, TimeSpan? endTime,
            WorkerScheduleSortState sortOrder = WorkerScheduleSortState.WorkerLastNameAsc)
        {
            var schedules = _context.WorkerSchedules
                .Include(w => w.Schedule)
                    .ThenInclude(t=>t.TimeSegment)
                .Include(w => w.Schedule)
                    .ThenInclude(d=>d.Day)
                .Include(w => w.Worker)
                    .ThenInclude(p=>p.Position)
                    .Select(x=>x);

            if (!String.IsNullOrEmpty(workerFirstName))
            {
                schedules = schedules.Where(w => w.Worker.FirstName == workerFirstName);
            }
            if (!String.IsNullOrEmpty(workerLastName))
            {
                schedules = schedules.Where(w => w.Worker.LastName == workerLastName);
            }
            if (!String.IsNullOrEmpty(position))
            {
                schedules = schedules.Where(w => w.Worker.Position.PositionName == position);
            }
            if (!String.IsNullOrEmpty(day))
            {
                schedules = schedules.Where(w => w.Schedule.Day.Name == day);
            }
            if (startTime.HasValue)
            {
                schedules = schedules.Where(w => w.Schedule.TimeSegment.TimeStart == startTime);
            }
            if (endTime.HasValue)
            {
                schedules = schedules.Where(w => w.Schedule.TimeSegment.TimeEnd == endTime);
            }
            ViewData["WorkerFirstNameSort"] = sortOrder == WorkerScheduleSortState.WorkerFirstNameAsc ? WorkerScheduleSortState.WorkerFirstNameDesc : WorkerScheduleSortState.WorkerFirstNameAsc;
            ViewData["WorkerLastNameSort"] = sortOrder == WorkerScheduleSortState.WorkerLastNameAsc ? WorkerScheduleSortState.WorkerLastNameDesc : WorkerScheduleSortState.WorkerLastNameAsc;
            ViewData["PositionSort"] = sortOrder == WorkerScheduleSortState.PositionAsc ? WorkerScheduleSortState.PositionDesc : WorkerScheduleSortState.PositionAsc;
            ViewData["DaySort"] = sortOrder == WorkerScheduleSortState.DayAsc ? WorkerScheduleSortState.DayDesc : WorkerScheduleSortState.DayAsc;
            ViewData["TimeStartSort"] = sortOrder == WorkerScheduleSortState.StartTimeAsc ? WorkerScheduleSortState.StartTimeDesc : WorkerScheduleSortState.StartTimeAsc;
            ViewData["TimeEndSort"] = sortOrder == WorkerScheduleSortState.EndTimeAsc ? WorkerScheduleSortState.EndTimeDesc : WorkerScheduleSortState.EndTimeAsc;

            schedules = sortOrder switch
            {
                //WorkerScheduleSortState.WorkerFirstNameAsc => schedules.OrderBy(s => s.Worker.FirstName),
                WorkerScheduleSortState.WorkerLastNameDesc => schedules.OrderByDescending(s => s.Worker.LastName),
                WorkerScheduleSortState.WorkerFirstNameAsc => schedules.OrderBy(s => s.Worker.FirstName),
                WorkerScheduleSortState.WorkerFirstNameDesc => schedules.OrderByDescending(s => s.Worker.FirstName),                                            
                WorkerScheduleSortState.PositionAsc => schedules.OrderBy(s => s.Worker.Position.PositionName),
                WorkerScheduleSortState.PositionDesc => schedules.OrderByDescending(s => s.Worker.Position.PositionName),
                WorkerScheduleSortState.DayDesc => schedules.OrderBy(s => s.Schedule.Day.Name),
                WorkerScheduleSortState.DayAsc => schedules.OrderByDescending(s => s.Schedule.Day.Name),
                WorkerScheduleSortState.StartTimeAsc => schedules.OrderBy(s => s.Schedule.TimeSegment.TimeStart),
                WorkerScheduleSortState.StartTimeDesc => schedules.OrderByDescending(s => s.Schedule.TimeSegment.TimeStart),
                WorkerScheduleSortState.EndTimeAsc => schedules.OrderBy(s => s.Schedule.TimeSegment.TimeEnd),
                WorkerScheduleSortState.EndTimeDesc => schedules.OrderByDescending(s => s.Schedule.TimeSegment.TimeEnd),
                _ => schedules.OrderBy(s => s.Worker.LastName),
            };

            return View(await schedules.ToListAsync());
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
            ViewData["ScheduleId"] = new SelectList(
                 _context.Schedules
                 .Include(t => t.TimeSegment)
                 .Include(d => d.Day)
                .Select(t => new
                {
                    t.ScheduleId,
                    CompoundSchedule = $"{t.Day.Name} {t.TimeSegment.TimeStart} - {t.TimeSegment.TimeEnd}"
                }), "ScheduleId", "CompoundSchedule");

            //ViewData["ScheduleId"] = new SelectList(_context.Schedules, "ScheduleId", "ScheduleId");
            ViewData["WorkerId"] = new SelectList(
                _context.Workers
                .Select(w=>new 
                {
                    w.WorkerId,
                    CompoundWorker = $"{w.FirstName} {w.LastName}"
                }), "WorkerId", "CompoundWorker");
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
            var selectList = _context.Schedules
               .Include(d => d.Day)
               .Include(t => t.TimeSegment)
               .OrderBy(d=>d.Day.Name)
               .Select(s => new
               {
                   ScheduleId = id,
                   CompoundSchedule = $"{s.Day.Name} | {s.TimeSegment.TimeStart} - {s.TimeSegment.TimeEnd}"

               });


            //ViewData["ScheduleId"] = new SelectList(_context.Schedules, "ScheduleId", "ScheduleId", workerSchedule.ScheduleId);
            ViewData["ScheduleId"] = new SelectList(selectList, "ScheduleId", "CompoundSchedule", workerSchedule.ScheduleId);
            //ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", workerSchedule.WorkerId);
            ViewData["WorkerId"] = new SelectList(
                _context.Workers
                .Select(w => new
                {
                    w.WorkerId,
                    CompoundWorker = $"{w.FirstName} {w.LastName}"
                }), "WorkerId", "CompoundWorker");
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
                    .ThenInclude(d=>d.Day)
                .Include(w => w.Schedule)
                    .ThenInclude(t=>t.TimeSegment)
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
