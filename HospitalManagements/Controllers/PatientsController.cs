using HospitalManagements.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Routing.Constraints;

namespace HospitalManagements.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PatientsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Registration([FromForm] PatientViewModel patientViewModel)
        {
            if (patientViewModel.ResumeImages != null && patientViewModel.ResumeImages.Length > 0)
            {
                string uploadfolder = Path.Combine(_hostingEnvironment.WebRootPath, "imeges");

                // Ensure the upload folder exists
                if (!Directory.Exists(uploadfolder))
                {
                    Directory.CreateDirectory(uploadfolder);
                }

                string filename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(patientViewModel.ResumeImages.FileName);
                string filepath = Path.Combine(uploadfolder, filename);

                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    await patientViewModel.ResumeImages.CopyToAsync(fileStream);
                }

                // Assign the filename to the view model
                patientViewModel.ResumeImageFileName = filename;
            }
            else
            {
                throw new Exception("Image not Found");
            }
            // Create a new Patient object
            Patient patient = new Patient
            {
                Id = Guid.NewGuid(),
                Name = patientViewModel.Name,
                PhoneNumber = patientViewModel.PhoneNumber,
                Email = patientViewModel.Email,
                Password = patientViewModel.Password,
                Date = DateTime.Now,
                City = patientViewModel.City,
                State = patientViewModel.State,
                IsActive = patientViewModel.IsActive,
                ResumeImages = patientViewModel.ResumeImageFileName, // Use the filename property here
            };

            _context.Add(patient);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("PatientId", patient.Id.ToString());
            return RedirectToAction(nameof(Login));
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            
            var patient = _context.Patients.FirstOrDefault(p => p.Email == email && p.Password == password);
            if (patient != null)
            {
                HttpContext.Session.SetString("PatientId", patient.Id.ToString());
                return RedirectToAction(nameof(Dashboard));
            }
           // ModelState.AddModelError("", "Invalid login attempt.");
           else
            {
                Response.Redirect("invalid Email or Password");
            }
            return View();
        }



        //Forget Password Functionality.
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgetPassword(string phoneNumber)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.PhoneNumber == phoneNumber);

            if (patient != null)
            {
                // Display the password (Note: Displaying plain text passwords is not recommended for security reasons)
                ViewBag.PasswordResult = $"Your password is: {patient.Password}";
            }
            else
            {
                ViewBag.PasswordResult = "Invalid phone number.";
            }

            return View();
        }















        public async Task<IActionResult> Dashboard()
        {
            var patientIdString = HttpContext.Session.GetString("PatientId");
            if (Guid.TryParse(patientIdString, out var patientId))
            {
                var patient = await _context.Patients.FindAsync(patientId);
                if (patient != null)
                {
                    return View(patient);
                }
            }
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Register()
        {
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name");
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(Appointment appointment)
        {
            var startTime = appointment.Date;
            var endTime = appointment.Date.AddHours(1);

            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.DoctorId == appointment.DoctorId &&
                                           ((a.Date >= startTime && a.Date < endTime) ||
                                            (a.Date.AddHours(1) > startTime && a.Date.AddHours(1) <= endTime)));

            if (existingAppointment != null)
            {
                ModelState.AddModelError("", $"This doctor already has an appointment from {existingAppointment.Date} to {existingAppointment.Date.AddHours(1)}.");
                ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name");
                return View("Register", appointment);
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Doctors");
        }




        //[HttpPost]
        //public async Task<IActionResult> BookAppointment(Appointment appointment)
        //{


        //      Appointment newappointment = new Appointment
        //        {
        //            PatientId = appointment.PatientId,
        //            DoctorId = appointment.DoctorId,
        //            Disease = appointment.Disease,
        //            Date = appointment.Date,
        //            PhoneNumber = appointment.PhoneNumber
        //        };
        //        _context.Appointments.Add(newappointment);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Login", "Doctors");

        //}
        //    //return View(appointment);












        //public async Task<IActionResult> Edit(Guid id)
        //{
        //    var patient = await _context.Patients.FindAsync(id);
        //    if (patient == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(patient);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,PhoneNumber,Email,Password,City,State,IsActive")] Patient patient, IFormFile ResumeImage)
        //{
        //    if (id != patient.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (ResumeImage != null)
        //            {
        //                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "imeges");
        //                if (!Directory.Exists(uploads))
        //                {
        //                    Directory.CreateDirectory(uploads);
        //                }
        //                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ResumeImage.FileName);
        //                var filePath = Path.Combine(uploads, fileName);
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await ResumeImage.CopyToAsync(stream);
        //                }
        //                patient.ResumeImages = "/imeges/" + fileName; // Store relative path to the file
        //            }

        //            _context.Update(patient);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PatientExists(patient.Id))
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
        //    return View(patient);
        //}

        private bool PatientExists(Guid id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
