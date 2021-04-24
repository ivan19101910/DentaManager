using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCoursework;
using WebCoursework.Models;

namespace WebCoursework.Controllers
{
    public class WorkersController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public WorkersController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: Workers
        //public async Task<IActionResult> Index()
        //{
        //    var dentalClinicDBContext = _context.Workers.Include(w => w.Office).Include(w => w.Position);
        //    return View(await dentalClinicDBContext.ToListAsync());
        //}
        //GET: Workers
        public async Task<IActionResult> Index(string firstName, string lastName, WorkerSortState sortOrder = WorkerSortState.FirstNameAsc)
        {
            var workers = _context.Workers.Select(x => x);
            //var workers = from m in _context.Workers
            //             select m;

            if (!String.IsNullOrEmpty(firstName))
            {
                workers = workers.Where(w => w.FirstName == firstName);
            }
            if (!String.IsNullOrEmpty(lastName))
            {
                workers = workers.Where(w => w.LastName == lastName);
            }

            ViewData["FirstNameSort"] = sortOrder == WorkerSortState.FirstNameAsc ? WorkerSortState.FirstNameDesc : WorkerSortState.FirstNameAsc;
            ViewData["LastNameSort"] = sortOrder == WorkerSortState.LastNameAsc ? WorkerSortState.LastNameDesc : WorkerSortState.LastNameAsc;
            ViewData["OfficeSort"] = sortOrder == WorkerSortState.OfficeAsc ? WorkerSortState.OfficeDesc : WorkerSortState.OfficeAsc;

            workers = sortOrder switch 
            {
                WorkerSortState.FirstNameDesc => workers.OrderByDescending(s => s.FirstName),
                WorkerSortState.LastNameAsc => workers.OrderBy(s => s.LastName),
                WorkerSortState.LastNameDesc => workers.OrderByDescending(s => s.LastName),
                WorkerSortState.OfficeAsc => workers.OrderBy(s => s.Office.Address),
                WorkerSortState.OfficeDesc => workers.OrderByDescending(s => s.Office.Address),
                _ => workers.OrderBy(s => s.FirstName),
            };

            return View(await workers.Include(w => w.Office).ThenInclude(c => c.City).Include(w => w.Position).ToListAsync());
        }

        //public async Task<IActionResult> Index(WorkerSearch searchModel)
        //{
        //    var workers = new Worker();
        //    var model = workers.GetWorkers(searchModel);
        //    return View(model);
        //}



        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(w => w.Office)
                .ThenInclude(c=>c.City)
                .Include(w => w.Position)
                .FirstOrDefaultAsync(m => m.WorkerId == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            ViewData["OfficeId"] = new SelectList(_context.Offices, "OfficeId", "Address");
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName");
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkerId,FirstName,LastName,PhoneNumber,Email,Password,Address,PositionId,OfficeId,CreatedDateTime,LastModifiedDateTime")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfficeId"] = new SelectList(_context.Offices, "OfficeId", "Address", worker.OfficeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", worker.PositionId);
            return View(worker);
        }

        // GET: Workers/Edit/5
        //for edit page
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.FindAsync(id);

            if (worker == null)
            {
                return NotFound();
            }
            var selectList = _context.Offices
               .Include(c => c.City)
               .Select(c => new
               {
                   //CityId = c.CityId,
                   OfficeId = c.OfficeId,
                   CompoundAddress = $"{c.City.Name} | {c.Address}"
                   
               });
            //var selectList = _context.Workers.
            //    Include(o => o.Office)
            //   .ThenInclude(c => c.City);

            //ViewData["OfficeId"] = new SelectList(selectList, "RestaurantId", "Descr", officeId);
            //ViewData["OfficeId"] = new SelectList(_context.Offices, "OfficeId", "Address", worker.OfficeId);
            //ViewData["OfficeId"] = new SelectList(selectList, "OfficeId", "Address", worker.OfficeId);

            //-----ITS WORKS//represent only city for office
            //ViewData["OfficeId"] = new SelectList(_context.Offices.Include(c=>c.City), "OfficeId", "City.Name", worker.OfficeId);
            //-----
            //---WORKS CORRECTLY
            ViewData["OfficeId"] = new SelectList(selectList, "OfficeId", "CompoundAddress", worker.OfficeId);
            //-----
            //ViewData["CityId"] = new SelectList(_context.Offices, "OfficeId", "Address", worker.OfficeId);
            //ViewData["PositionId"] = new SelectList(selectList, "PositionId", "PositionName", worker.OfficeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", worker.PositionId);
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598
        //edit process.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkerId,FirstName,LastName,PhoneNumber,Email,Password,Address,PositionId,OfficeId,CreatedDateTime,LastModifiedDateTime")] Worker worker)
        {
            if (id != worker.WorkerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.WorkerId))
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
            ViewData["OfficeId"] = new SelectList(_context.Offices, "OfficeId", "Address", worker.OfficeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", worker.PositionId);
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(w => w.Office)
                .Include(w => w.Position)
                .FirstOrDefaultAsync(m => m.WorkerId == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.WorkerId == id);
        }
    }
}
