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
    public class SalaryPaymentsController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public SalaryPaymentsController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: SalaryPayments
        public async Task<IActionResult> Index()
        {
            var dentalClinicDBContext = _context.SalaryPayments.Include(s => s.Worker);
            return View(await dentalClinicDBContext.ToListAsync());
        }

        // GET: SalaryPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryPayment = await _context.SalaryPayments
                .Include(s => s.Worker)
                .FirstOrDefaultAsync(m => m.SalaryPaymentId == id);
            if (salaryPayment == null)
            {
                return NotFound();
            }

            return View(salaryPayment);
        }

        // GET: SalaryPayments/Create
        public IActionResult Create()
        {
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address");
            return View();
        }

        // POST: SalaryPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalaryPaymentId,MonthNumber,Year,TransactionNumber,Amount,WorkerId,CreatedDateTime,LastModifiedDateTime")] SalaryPayment salaryPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salaryPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", salaryPayment.WorkerId);
            return View(salaryPayment);
        }

        // GET: SalaryPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryPayment = await _context.SalaryPayments.FindAsync(id);
            if (salaryPayment == null)
            {
                return NotFound();
            }
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", salaryPayment.WorkerId);
            return View(salaryPayment);
        }

        // POST: SalaryPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalaryPaymentId,MonthNumber,Year,TransactionNumber,Amount,WorkerId,CreatedDateTime,LastModifiedDateTime")] SalaryPayment salaryPayment)
        {
            if (id != salaryPayment.SalaryPaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salaryPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryPaymentExists(salaryPayment.SalaryPaymentId))
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
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", salaryPayment.WorkerId);
            return View(salaryPayment);
        }

        // GET: SalaryPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryPayment = await _context.SalaryPayments
                .Include(s => s.Worker)
                .FirstOrDefaultAsync(m => m.SalaryPaymentId == id);
            if (salaryPayment == null)
            {
                return NotFound();
            }

            return View(salaryPayment);
        }

        // POST: SalaryPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaryPayment = await _context.SalaryPayments.FindAsync(id);
            _context.SalaryPayments.Remove(salaryPayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryPaymentExists(int id)
        {
            return _context.SalaryPayments.Any(e => e.SalaryPaymentId == id);
        }
    }
}
