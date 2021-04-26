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
    public class ServicesController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public ServicesController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index(decimal? price, string service, string serviceType, ServiceSortState sortOrder = ServiceSortState.PriceAsc)
        {
            //var dentalClinicDBContext = _context.Services.Include(s => s.ServiceType);
            //return View(await dentalClinicDBContext.ToListAsync());
            var services = _context.Services.Include(st=>st.ServiceType).Select(x => x);
            //var workers = from m in _context.Workers
            //             select m;
            if (price.HasValue)
            {
                services = services.Where(w => w.Price == price);
            }
            if (!String.IsNullOrEmpty(service))
            {
                services = services.Where(w => w.Name == service);
            }
            if (!String.IsNullOrEmpty(serviceType))
            {
                services = services.Where(w => w.ServiceType.Name == serviceType);
            }

            ViewData["PriceSort"] = sortOrder == ServiceSortState.PriceAsc ? ServiceSortState.PriceDesc : ServiceSortState.PriceAsc;
            ViewData["ServiceSort"] = sortOrder == ServiceSortState.ServiceAsc ? ServiceSortState.ServiceDesc : ServiceSortState.ServiceAsc;
            ViewData["ServiceTypeSort"] = sortOrder == ServiceSortState.ServiceAsc ? ServiceSortState.ServiceTypeDesc : ServiceSortState.ServiceTypeAsc;

            services = sortOrder switch
            {
                ServiceSortState.PriceDesc => services.OrderByDescending(s => s.Price),
                ServiceSortState.ServiceAsc => services.OrderBy(s => s.Name),
                ServiceSortState.ServiceDesc => services.OrderByDescending(s => s.Name),
                ServiceSortState.ServiceTypeAsc => services.OrderBy(s => s.ServiceType.Name),
                ServiceSortState.ServiceTypeDesc => services.OrderByDescending(s => s.ServiceType.Name),
                _ => services.OrderBy(s => s.Price),
            };
            return View(await services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.ServiceType)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "ServiceTypeId", "Name");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,Price,Name,Description,ServiceTypeId,CreatedDateTime,LastModifiedDateTime")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "ServiceTypeId", "Name", service.ServiceTypeId);
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "ServiceTypeId", "Name", service.ServiceTypeId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,Price,Name,Description,ServiceTypeId,CreatedDateTime,LastModifiedDateTime")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
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
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "ServiceTypeId", "Name", service.ServiceTypeId);
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.ServiceType)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
