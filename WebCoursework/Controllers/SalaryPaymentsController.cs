using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebCoursework;
using WebCoursework.Models.SortStates;

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
        //public async Task<IActionResult> Index()
        //{
        //    var dentalClinicDBContext = _context.SalaryPayments.Include(s => s.Worker);
        //    return View(await dentalClinicDBContext.ToListAsync());

        //}
        public async Task<IActionResult> Index(int year, int month, string workerName ,SalaryPaymentSortState sortOrder = SalaryPaymentSortState.YearAsc)
        {
            var payments = _context.SalaryPayments.Include(w=>w.Worker).Select(x => x);
            //var workers = from m in _context.Workers
            //             select m;

            if (year != 0)
            {
                payments = payments.Where(w => w.Year == year);
            }
            if (month != 0)
            {
                payments = payments.Where(w => w.MonthNumber == month);
            }
            if (!String.IsNullOrEmpty(workerName))
            {
                payments = payments.Where(w => w.Worker.FirstName == workerName);
            }

            ViewData["YearSort"] = sortOrder == SalaryPaymentSortState.YearAsc ? SalaryPaymentSortState.YearDesc : SalaryPaymentSortState.YearAsc;
            ViewData["MonthSort"] = sortOrder == SalaryPaymentSortState.MonthAsc ? SalaryPaymentSortState.MonthDesc : SalaryPaymentSortState.MonthAsc;
            ViewData["WorkerSort"] = sortOrder == SalaryPaymentSortState.WorkerAsc ? SalaryPaymentSortState.WorkerDesc : SalaryPaymentSortState.WorkerAsc;

            payments = sortOrder switch
            {
                SalaryPaymentSortState.YearDesc => payments.OrderByDescending(s => s.Year),
                SalaryPaymentSortState.MonthAsc => payments.OrderBy(s => s.MonthNumber),
                SalaryPaymentSortState.MonthDesc => payments.OrderByDescending(s => s.MonthNumber),
                SalaryPaymentSortState.WorkerAsc => payments.OrderBy(s => s.Worker.FirstName),
                SalaryPaymentSortState.WorkerDesc => payments.OrderByDescending(s => s.Worker.FirstName),
                _ => payments.OrderBy(s => s.Year),
            };

            return View(await payments.ToListAsync());
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

            //string sqlQuery = "SELECT [CountPayment] ({0}, {1}, {2})"
            string sqlQuery = $"SELECT dbo.CountPayment (15, 2020, 10)";
            Object[] parameters = { 15, 2020, 10 };
            //int activityCount = _context.Database.SqlQuery<int>(sqlQuery, parameters).FirstOrDefault();
            //decimal s = _context.Database.ExecuteSqlRaw(sqlQuery, parameters);
            //var s = _context.Database.ExecuteSqlRaw(sqlQuery);
            
            //decimal fff = _context.Database
            //decimal s = _context.Workers.FromSqlRaw(sqlQuery, parameters);


            if (salaryPayment == null)
            {
                return NotFound();
            }

            return View(salaryPayment);
        }

        // GET: SalaryPayments/Create
        public IActionResult Create()
        {
            var selectList = _context.SalaryPayments
               .Select(w => new
               {
                   w.WorkerId,
                   CompoundWorker = $"{w.Worker.FirstName} {w.Worker.LastName}"
               });
            ViewData["WorkerId"] = new SelectList(selectList, "WorkerId", "CompoundWorker");
            //ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address");
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
            var selectList = _context.Workers
               .Select(w => new
               {
                   w.WorkerId,
                   CompoundWorker = $"{w.FirstName} {w.LastName}"
               });

            //ViewData["OfficeId"] = new SelectList(selectList, "OfficeId", "CompoundAddress", worker.OfficeId);
            ViewData["WorkerId"] = new SelectList(selectList, "WorkerId", "CompoundWorker", salaryPayment.WorkerId);
            //ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", salaryPayment.WorkerId);
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
            var selectList = _context.Workers
               .Select(w => new
               {
                   w.WorkerId,
                   CompoundWorker = $"{w.FirstName} {w.LastName}"
               });

            //ViewData["OfficeId"] = new SelectList(selectList, "OfficeId", "CompoundAddress", worker.OfficeId);
            ViewData["WorkerId"] = new SelectList(selectList, "WorkerId", "CompoundWorker", salaryPayment.WorkerId);

            //ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", salaryPayment.WorkerId);
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
