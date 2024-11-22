





using HospitalManagements.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagements.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string DoctorSessionKey = "DoctorId";

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Login(Patient model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Email == model.Email);

        //    if (doctor == null || !VerifyPassword(model.Password, doctor.Password))
        //    {
        //        ModelState.AddModelError("", "Invalid email or password.");
        //        return View(model);
        //    }

        //    HttpContext.Session.SetInt32(DoctorSessionKey, doctor.Id);
        //    return RedirectToAction(nameof(Dashboard));
        //}



        //private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        //{





        //}



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Email == model.Email && d.Password == model.Password);

            if (doctor != null)
            {
                HttpContext.Session.SetInt32(DoctorSessionKey, doctor.Id);
                return RedirectToAction(nameof(Dashboard));
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }


        public async Task<IActionResult> Dashboard()
        {
            var doctorId = HttpContext.Session.GetInt32(DoctorSessionKey);
            if (doctorId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId.Value)
                .ToListAsync();

            return View(appointments);
        }
       
        public async Task<IActionResult> AnotherDashboard()
        {
            var doctorId = HttpContext.Session.GetInt32(DoctorSessionKey);
            //if (doctorId == null)
            //{
            //    return RedirectToAction(nameof(Login));
            //}

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId.Value)
                .ToListAsync();

            return View(appointments);
        }




        [HttpPost]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, bool isChecked)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.IsChecked = isChecked;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var appointment = await _context.Appointments.FindAsync(id);
        //    if (appointment == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(appointment);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,DoctorId,Date,Disease,PhoneNumber,IsChecked")] Appointment appointment)
        //{
        //    if (id != appointment.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(appointment);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AppointmentExists(appointment.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Dashboard));
        //    }
        //    return View(appointment);
        //}

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
