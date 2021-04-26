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
    public class AppointmentsController : Controller
    {
        private readonly DentalClinicDBContext _context;

        public AppointmentsController(DentalClinicDBContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(DateTime? date, TimeSpan? startTime, TimeSpan? endTime, string workerFirstName, string workerLastName, string patientFirstName,
            string patientLastName, string status, AppointmentSortState sortOrder = AppointmentSortState.DateAsc)
        {
            var appointments = _context.Appointments.Include(a => a.Patient).Include(a => a.Status).Include(a => a.Worker).Select(x=>x);

            

            if (date.HasValue)
            {
                appointments = appointments.Where(w => w.AppointmentDate == date);
            }
            if (startTime.HasValue)
            {
                appointments = appointments.Where(w => w.AppointmentTime == startTime);
            }
            if (endTime.HasValue)
            {
                appointments = appointments.Where(w => w.RealEndTime == endTime);
            }
            if (!String.IsNullOrEmpty(workerFirstName))
            {
                appointments = appointments.Where(w => w.Worker.FirstName == workerFirstName);
            }
            if (!String.IsNullOrEmpty(workerLastName))
            {
                appointments = appointments.Where(w => w.Worker.LastName == workerLastName);
            }
            if (!String.IsNullOrEmpty(patientFirstName))
            {
                appointments = appointments.Where(w => w.Patient.FirstName == patientFirstName);
            }
            if (!String.IsNullOrEmpty(patientLastName))
            {
                appointments = appointments.Where(w => w.Patient.LastName == patientLastName);
            }
            if (!String.IsNullOrEmpty(status))
            {
                appointments = appointments.Where(w => w.Status.Name == status);
            }


            ViewData["DateSort"] = sortOrder == AppointmentSortState.DateAsc ? AppointmentSortState.DateDesc : AppointmentSortState.DateAsc;
            ViewData["TimeStartSort"] = sortOrder == AppointmentSortState.StartTimeAsc ? AppointmentSortState.StartTimeDesc : AppointmentSortState.StartTimeAsc;
            ViewData["TimeEndSort"] = sortOrder == AppointmentSortState.EndTimeAsc ? AppointmentSortState.EndTimeDesc : AppointmentSortState.EndTimeAsc;
            ViewData["WorkerFirstNameSort"] = sortOrder == AppointmentSortState.WorkerFirstNameAsc ? AppointmentSortState.WorkerFirstNameDesc : AppointmentSortState.WorkerFirstNameAsc;
            ViewData["WorkerLastNameSort"] = sortOrder == AppointmentSortState.WorkerLastNameAsc ? AppointmentSortState.WorkerLastNameDesc : AppointmentSortState.WorkerLastNameAsc;
            ViewData["PatientFirstNameSort"] = sortOrder == AppointmentSortState.PatientFirstNameAsc ? AppointmentSortState.PatientFirstNameDesc : AppointmentSortState.PatientFirstNameAsc;
            ViewData["PatientLastNameSort"] = sortOrder == AppointmentSortState.PatientLastNameAsc ? AppointmentSortState.PatientLastNameDesc : AppointmentSortState.PatientLastNameAsc;
            ViewData["StatusSort"] = sortOrder == AppointmentSortState.StatusAsc ? AppointmentSortState.StatusDesc : AppointmentSortState.StatusAsc;

            appointments = sortOrder switch
            {
                AppointmentSortState.DateDesc => appointments.OrderByDescending(s => s.AppointmentDate),
                AppointmentSortState.StartTimeAsc => appointments.OrderBy(s => s.AppointmentTime),
                AppointmentSortState.StartTimeDesc => appointments.OrderByDescending(s => s.AppointmentTime),
                AppointmentSortState.EndTimeAsc => appointments.OrderBy(s => s.RealEndTime),
                AppointmentSortState.EndTimeDesc => appointments.OrderByDescending(s => s.RealEndTime),
                AppointmentSortState.WorkerFirstNameAsc => appointments.OrderBy(s => s.Worker.FirstName),
                AppointmentSortState.WorkerFirstNameDesc => appointments.OrderByDescending(s => s.Worker.FirstName),
                AppointmentSortState.WorkerLastNameAsc => appointments.OrderBy(s => s.Worker.LastName),
                AppointmentSortState.WorkerLastNameDesc => appointments.OrderByDescending(s => s.Worker.LastName),
                AppointmentSortState.PatientFirstNameAsc => appointments.OrderBy(s => s.Patient.FirstName),
                AppointmentSortState.PatientFirstNameDesc => appointments.OrderByDescending(s => s.Patient.FirstName),
                AppointmentSortState.PatientLastNameAsc => appointments.OrderBy(s => s.Patient.LastName),
                AppointmentSortState.PatientLastNameDesc => appointments.OrderByDescending(s => s.Patient.LastName),
                AppointmentSortState.StatusAsc => appointments.OrderBy(s => s.Status.Name),
                AppointmentSortState.StatusDesc => appointments.OrderByDescending(s => s.Status.Name),
                _ => appointments.OrderBy(s => s.AppointmentDate),
            };

            return View(await appointments.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Status)
                .Include(a => a.Worker)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            

            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Address");
            ViewData["StatusId"] = new SelectList(_context.AppointmentStatuses, "StatusId", "Name");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,AppointmentDate,Notes,RealEndTime,AppointmentTime,WorkerId,PatientId,StatusId,CreatedDateTime,LastModifiedDateTime,TotalSum")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Address", appointment.PatientId);
            ViewData["StatusId"] = new SelectList(_context.AppointmentStatuses, "StatusId", "Name", appointment.StatusId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", appointment.WorkerId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            ViewData["PatientId"] = new SelectList(
                _context.Patients
                .Select(p=>new
                {
                    p.PatientId,
                    CompoundName = $"{p.FirstName} {p.LastName}"
                }), "PatientId", "CompoundName", appointment.PatientId);

            ViewData["StatusId"] = new SelectList(_context.AppointmentStatuses, "StatusId", "Name", appointment.StatusId);
            ViewData["WorkerId"] = new SelectList(
                _context.Workers
                .Include(o=>o.Office)
                .ThenInclude(c=>c.City)
                .Select(w => new
                {
                    w.WorkerId,
                    CompoundWorker = $"{w.FirstName} {w.LastName} | {w.Office.City.Name}"
                })
                , "WorkerId", "CompoundWorker", appointment.WorkerId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,AppointmentDate,Notes,RealEndTime,AppointmentTime,WorkerId,PatientId,StatusId,CreatedDateTime,LastModifiedDateTime,TotalSum")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Address", appointment.PatientId);
            ViewData["StatusId"] = new SelectList(_context.AppointmentStatuses, "StatusId", "Name", appointment.StatusId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Address", appointment.WorkerId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Status)
                .Include(a => a.Worker)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}
