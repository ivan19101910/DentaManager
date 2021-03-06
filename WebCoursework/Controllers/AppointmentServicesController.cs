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
    public class AppointmentServicesController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public AppointmentServicesController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: AppointmentServices
        public async Task<IActionResult> Index(int? id, DateTime? date, TimeSpan? timeStart, string workerFirstName, string workerLastName
            ,AppointmentServiceSortState sortOrder = AppointmentServiceSortState.DateAsc)
        {
            var services = 
                _context.AppointmentServices
                .Include(a => a.Appointment)
                .ThenInclude(w=>w.Worker)
                .Include(a => a.Service)
                .Select(x=>x);

            if (id.HasValue)
            {
                services = services.Where(w => w.Appointment.AppointmentId == id);
            }
            if (date.HasValue)
            {
                services = services.Where(w => w.Appointment.AppointmentDate == date);
            }
            if (timeStart.HasValue)
            {
                services =  services.Where(w => w.Appointment.AppointmentTime == timeStart);
            }
            if (!String.IsNullOrEmpty(workerFirstName))
            {
                services = services.Where(w => w.Appointment.Worker.FirstName == workerFirstName);
            }
            if (!String.IsNullOrEmpty(workerLastName))
            {
                services = services.Where(w => w.Appointment.Worker.LastName == workerLastName);
            }

            ViewData["IdSort"] = sortOrder == AppointmentServiceSortState.IdAsc ? AppointmentServiceSortState.IdDesc : AppointmentServiceSortState.IdAsc;
            ViewData["DateSort"] = sortOrder == AppointmentServiceSortState.DateAsc ? AppointmentServiceSortState.DateDesc : AppointmentServiceSortState.DateAsc;
            ViewData["TimeStartSort"] = sortOrder == AppointmentServiceSortState.StartTimeAsc ? AppointmentServiceSortState.StartTimeDesc : AppointmentServiceSortState.StartTimeAsc;
            ViewData["WorkerNameSort"] = sortOrder == AppointmentServiceSortState.WorkerNameAsc ? AppointmentServiceSortState.WorkerNameDesc : AppointmentServiceSortState.WorkerNameAsc;
            
            ViewData["AmountSort"] = sortOrder == AppointmentServiceSortState.AmountAsc ? AppointmentServiceSortState.AmountDesc : AppointmentServiceSortState.AmountAsc;

            services = sortOrder switch
            {
                AppointmentServiceSortState.IdDesc => services.OrderByDescending(s => s.AppointmentId),
                AppointmentServiceSortState.DateAsc => services.OrderBy(s => s.Appointment.AppointmentDate),
                AppointmentServiceSortState.DateDesc => services.OrderByDescending(s => s.Appointment.AppointmentDate),
                AppointmentServiceSortState.StartTimeAsc => services.OrderBy(s => s.Appointment.AppointmentTime),
                AppointmentServiceSortState.StartTimeDesc => services.OrderByDescending(s => s.Appointment.AppointmentTime),
                AppointmentServiceSortState.WorkerNameAsc => services.OrderBy(s => s.Appointment.Worker.FirstName).ThenBy(s => s.Appointment.Worker.LastName),
                AppointmentServiceSortState.WorkerNameDesc => services.OrderByDescending(s => s.Appointment.Worker.FirstName).ThenBy(s => s.Appointment.Worker.LastName),
                AppointmentServiceSortState.AmountAsc => services.OrderBy(s => s.Amount),
                AppointmentServiceSortState.AmountDesc => services.OrderByDescending(s => s.Amount),
                _ => services.OrderBy(s => s.AppointmentId),
            };

            return View(await services.ToListAsync());
        }

        // GET: AppointmentServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentService = await _context.AppointmentServices
                .Include(a => a.Appointment)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentService == null)
            {
                return NotFound();
            }

            return View(appointmentService);
        }

        // GET: AppointmentServices/Create
        public IActionResult Create()
        {
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name");
            return View();
        }

        // POST: AppointmentServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServiceId,AppointmentId,Amount,CreatedDateTime,LastModifiedDateTime")] AppointmentService appointmentService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Notes", appointmentService.AppointmentId);
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Notes", appointmentService.AppointmentId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Description", appointmentService.ServiceId);
            return View(appointmentService);
        }

        // GET: AppointmentServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentService = await _context.AppointmentServices.FindAsync(id);
            if (appointmentService == null)
            {
                return NotFound();
            }
            //ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Notes", appointmentService.AppointmentId);
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId", appointmentService.AppointmentId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name", appointmentService.ServiceId);
            return View(appointmentService);
        }

        // POST: AppointmentServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceId,AppointmentId,Amount,CreatedDateTime,LastModifiedDateTime")] AppointmentService appointmentService)
        {
            if (id != appointmentService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentServiceExists(appointmentService.Id))
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
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Notes", appointmentService.AppointmentId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Description", appointmentService.ServiceId);
            return View(appointmentService);
        }

        // GET: AppointmentServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentService = await _context.AppointmentServices
                .Include(a => a.Appointment)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentService == null)
            {
                return NotFound();
            }

            return View(appointmentService);
        }

        // POST: AppointmentServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentService = await _context.AppointmentServices.FindAsync(id);
            _context.AppointmentServices.Remove(appointmentService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentServiceExists(int id)
        {
            return _context.AppointmentServices.Any(e => e.Id == id);
        }
    }
}
