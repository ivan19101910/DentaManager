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
    public class OfficesController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public OfficesController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: Offices
        public async Task<IActionResult> Index(string address, string city, OfficeSortState sortOrder = OfficeSortState.AddressAsc)
        {
            //var dentalClinicDBContext = _context.Offices.Include(o => o.City);

            var offices = _context.Offices.Include(o => o.City).Select(x => x);

            if (!String.IsNullOrEmpty(address))
            {
                offices = offices.Where(w => w.Address == address);
            }
            if (!String.IsNullOrEmpty(city))
            {
                offices = offices.Where(w => w.City.Name == city);
            }

            ViewData["AddressSort"] = sortOrder == OfficeSortState.AddressAsc ? OfficeSortState.AddressDesc : OfficeSortState.AddressAsc;
            ViewData["CitySort"] = sortOrder == OfficeSortState.CityAsc ? OfficeSortState.CityDesc : OfficeSortState.CityAsc;

            offices = sortOrder switch
            {
                OfficeSortState.AddressDesc => offices.OrderByDescending(s => s.Address),
                OfficeSortState.CityAsc => offices.OrderBy(s => s.City.Name),
                OfficeSortState.CityDesc => offices.OrderByDescending(s => s.City.Name),
                _ => offices.OrderBy(s => s.Address),
            };

            return View(await offices.ToListAsync());
        }

        // GET: Offices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var office = await _context.Offices
                .Include(o => o.City)
                .FirstOrDefaultAsync(m => m.OfficeId == id);
            if (office == null)
            {
                return NotFound();
            }

            return View(office);
        }

        // GET: Offices/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name");
            return View();
        }

        // POST: Offices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfficeId,Address,CityId,CreatedDateTime,LastModifiedDateTime")] Office office)
        {
            if (ModelState.IsValid)
            {
                _context.Add(office);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name", office.CityId);
            return View(office);
        }

        // GET: Offices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var office = await _context.Offices.FindAsync(id);
            if (office == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name", office.CityId);
            return View(office);
        }

        // POST: Offices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OfficeId,Address,CityId,CreatedDateTime,LastModifiedDateTime")] Office office)
        {
            if (id != office.OfficeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(office);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfficeExists(office.OfficeId))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name", office.CityId);
            return View(office);
        }

        // GET: Offices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var office = await _context.Offices
                .Include(o => o.City)
                .FirstOrDefaultAsync(m => m.OfficeId == id);
            if (office == null)
            {
                return NotFound();
            }

            return View(office);
        }

        // POST: Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var office = await _context.Offices.FindAsync(id);
            _context.Offices.Remove(office);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfficeExists(int id)
        {
            return _context.Offices.Any(e => e.OfficeId == id);
        }
    }
}
