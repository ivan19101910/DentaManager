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
    public class AppointmentPaymentsController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public AppointmentPaymentsController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: AppointmentPayments
        public async Task<IActionResult> Index(int? id, DateTime? date, AppointmentPaymentSortState sortOrder = AppointmentPaymentSortState.IdAsc)
        {
            var payments = _context.AppointmentPayments.Include(a => a.Appointment).Select(x=>x);

            if (id.HasValue)
            {
                payments = payments.Where(w => w.Appointment.AppointmentId == id);
            }
            if (date.HasValue)
            {
                payments = payments.Where(w => w.Appointment.AppointmentDate == date);
            }

            ViewData["IdSort"] = sortOrder == AppointmentPaymentSortState.IdAsc ? AppointmentPaymentSortState.IdDesc : AppointmentPaymentSortState.IdAsc;
            ViewData["DateSort"] = sortOrder == AppointmentPaymentSortState.DateAsc ? AppointmentPaymentSortState.DateDesc : AppointmentPaymentSortState.DateAsc;
            ViewData["TotalSort"] = sortOrder == AppointmentPaymentSortState.TotalAsc ? AppointmentPaymentSortState.TotalDesc : AppointmentPaymentSortState.TotalAsc;

            payments = sortOrder switch
            {
                AppointmentPaymentSortState.IdDesc => payments.OrderByDescending(s => s.AppointmentId),
                AppointmentPaymentSortState.DateAsc => payments.OrderBy(s => s.Appointment.AppointmentDate),
                AppointmentPaymentSortState.DateDesc => payments.OrderByDescending(s => s.Appointment.AppointmentDate),
                AppointmentPaymentSortState.TotalAsc => payments.OrderBy(s => s.Total),
                AppointmentPaymentSortState.TotalDesc => payments.OrderByDescending(s => s.Total),
                _ => payments.OrderBy(s => s.AppointmentId),
            };

            return View(await payments.ToListAsync());
        }

        // GET: AppointmentPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentPayment = await _context.AppointmentPayments
                .Include(a => a.Appointment)
                .FirstOrDefaultAsync(m => m.AppointmentPaymentId == id);
            if (appointmentPayment == null)
            {
                return NotFound();
            }

            return View(appointmentPayment);
        }

        // GET: AppointmentPayments/Create
        public IActionResult Create()
        {
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            return View();
        }

        // POST: AppointmentPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("AppointmentPaymentId,TransactionNumber,AppointmentId,Total,CreatedDateTime,LastModifiedDateTime")] AppointmentPayment appointmentPayment)
        //{
            
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(appointmentPayment);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Notes", appointmentPayment.AppointmentId);
        //    return View(appointmentPayment);
        //}
        public async Task<IActionResult> Create([Bind("AppointmentPaymentId,TransactionNumber,AppointmentId,Total,CreatedDateTime,LastModifiedDateTime")] AppointmentPayment appointmentPayment)
        {
            Random rnd = new Random();
            appointmentPayment.TransactionNumber = rnd.Next(1, 100);
            if (ModelState.IsValid)
            {
                _context.Add(appointmentPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Notes", appointmentPayment.AppointmentId);
            return View(appointmentPayment);
        }

        // GET: AppointmentPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentPayment = await _context.AppointmentPayments.FindAsync(id);
            if (appointmentPayment == null)
            {
                return NotFound();
            }
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId", appointmentPayment.AppointmentId);
            return View(appointmentPayment);
        }

        // POST: AppointmentPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentPaymentId,TransactionNumber,AppointmentId,Total,CreatedDateTime,LastModifiedDateTime")] AppointmentPayment appointmentPayment)
        {
            if (id != appointmentPayment.AppointmentPaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentPaymentExists(appointmentPayment.AppointmentPaymentId))
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
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Notes", appointmentPayment.AppointmentId);
            return View(appointmentPayment);
        }

        // GET: AppointmentPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentPayment = await _context.AppointmentPayments
                .Include(a => a.Appointment)
                .FirstOrDefaultAsync(m => m.AppointmentPaymentId == id);
            if (appointmentPayment == null)
            {
                return NotFound();
            }

            return View(appointmentPayment);
        }

        // POST: AppointmentPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentPayment = await _context.AppointmentPayments.FindAsync(id);
            _context.AppointmentPayments.Remove(appointmentPayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentPaymentExists(int id)
        {
            return _context.AppointmentPayments.Any(e => e.AppointmentPaymentId == id);
        }
    }
}
