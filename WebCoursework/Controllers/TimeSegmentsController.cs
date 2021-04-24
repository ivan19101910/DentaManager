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
    public class TimeSegmentsController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public TimeSegmentsController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: TimeSegments
        public async Task<IActionResult> Index()
        {
            return View(await _context.TimeSegments.ToListAsync());
        }

        // GET: TimeSegments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeSegment = await _context.TimeSegments
                .FirstOrDefaultAsync(m => m.TimeSegmentId == id);
            if (timeSegment == null)
            {
                return NotFound();
            }

            return View(timeSegment);
        }

        // GET: TimeSegments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TimeSegments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimeSegmentId,TimeStart,TimeEnd,CreatedDateTime,LastModifiedDateTime")] TimeSegment timeSegment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeSegment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeSegment);
        }

        // GET: TimeSegments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeSegment = await _context.TimeSegments.FindAsync(id);
            if (timeSegment == null)
            {
                return NotFound();
            }
            return View(timeSegment);
        }

        // POST: TimeSegments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimeSegmentId,TimeStart,TimeEnd,CreatedDateTime,LastModifiedDateTime")] TimeSegment timeSegment)
        {
            if (id != timeSegment.TimeSegmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeSegment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeSegmentExists(timeSegment.TimeSegmentId))
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
            return View(timeSegment);
        }

        // GET: TimeSegments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeSegment = await _context.TimeSegments
                .FirstOrDefaultAsync(m => m.TimeSegmentId == id);
            if (timeSegment == null)
            {
                return NotFound();
            }

            return View(timeSegment);
        }

        // POST: TimeSegments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeSegment = await _context.TimeSegments.FindAsync(id);
            _context.TimeSegments.Remove(timeSegment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeSegmentExists(int id)
        {
            return _context.TimeSegments.Any(e => e.TimeSegmentId == id);
        }
    }
}
