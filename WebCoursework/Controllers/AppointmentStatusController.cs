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
    public class AppointmentStatusController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public AppointmentStatusController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: AppointmentStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppointmentStatuses.ToListAsync());
        }

        // GET: AppointmentStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentStatus = await _context.AppointmentStatuses
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (appointmentStatus == null)
            {
                return NotFound();
            }

            return View(appointmentStatus);
        }

        // GET: AppointmentStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppointmentStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,Name,CreatedDateTime,LastModifiedDateTime")] AppointmentStatus appointmentStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentStatus);
        }

        // GET: AppointmentStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentStatus = await _context.AppointmentStatuses.FindAsync(id);
            if (appointmentStatus == null)
            {
                return NotFound();
            }
            return View(appointmentStatus);
        }

        // POST: AppointmentStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,Name,CreatedDateTime,LastModifiedDateTime")] AppointmentStatus appointmentStatus)
        {
            if (id != appointmentStatus.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentStatusExists(appointmentStatus.StatusId))
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
            return View(appointmentStatus);
        }

        // GET: AppointmentStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentStatus = await _context.AppointmentStatuses
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (appointmentStatus == null)
            {
                return NotFound();
            }

            return View(appointmentStatus);
        }

        // POST: AppointmentStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentStatus = await _context.AppointmentStatuses.FindAsync(id);
            _context.AppointmentStatuses.Remove(appointmentStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentStatusExists(int id)
        {
            return _context.AppointmentStatuses.Any(e => e.StatusId == id);
        }
    }
}
