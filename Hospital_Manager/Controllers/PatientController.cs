using Data_Hospital_Manager;
using Hospital_Manager.ViewModels.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Manager.Controllers
{
    public class PatientController : Controller
    {
        private readonly HospitalDbContext context;
        public PatientController(HospitalDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var patients = await context.Patients.Include(x => x.DoctorPatients).ThenInclude(dp => dp.Doctor).ToListAsync();
            var model = patients.Select(p => new ViewModels.Patient.PatientIndexViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber,
                DoctorName = string.Join(", ", p.DoctorPatients.Select(dp => dp.Doctor.FirstName + " " + dp.Doctor.LastName))
            }).ToList();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var patient = await context.Patients.Include(x => x.DoctorPatients).ThenInclude(dp => dp.Doctor).FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            var model = new ViewModels.Patient.PatientIndexViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DoctorName = string.Join(", ", patient.DoctorPatients.Select(dp => dp.Doctor.FirstName + " " + dp.Doctor.LastName))
            };
            return View(model);
        }
        public async Task LoadDoctors(List<int> ids)
        {
            var doctors = await context.Doctors.Select(x => new
            {
                x.Id,
                FullName = x.FirstName + " " + x.LastName
            }).ToListAsync();
            ViewBag.Doctors = new MultiSelectList(doctors, "Id", "FullName", ids);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDoctors(new List<int>());
            return View();
        }
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create(ViewModels.Patient.PatientCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Data_Hospital_Manager.Entities.Patient
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                context.Patients.Add(patient);
                await context.SaveChangesAsync();
                if (model.DoctorIds != null && model.DoctorIds.Count > 0)
                {
                    foreach (var doctorId in model.DoctorIds)
                    {
                        var doctorPatient = new Data_Hospital_Manager.Entities.DoctorPatient
                        {
                            DoctorId = doctorId,
                            PatientId = patient.Id
                        };
                        context.DoctorPatients.Add(doctorPatient);
                    }
                    await context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            await LoadDoctors(model.DoctorIds);
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await context.Patients.Include(x => x.DoctorPatients).FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            var model = new ViewModels.Patient.PatientEditViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DoctorIds = patient.DoctorPatients.Select(dp => dp.DoctorId).ToList()
            };
            await LoadDoctors(model.DoctorIds);
            return View(model);
        }
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(int id,ViewModels.Patient.PatientEditViewModel model)
        {
            if(id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var patient = await context.Patients.Include(x => x.DoctorPatients).FirstOrDefaultAsync(p => p.Id == model.Id);
                if (patient == null)
                {
                    return NotFound();
                }
                patient.FirstName = model.FirstName;
                patient.LastName = model.LastName;
                patient.Email = model.Email;
                patient.PhoneNumber = model.PhoneNumber;
                context.DoctorPatients.RemoveRange(patient.DoctorPatients);
                if (model.DoctorIds != null && model.DoctorIds.Count > 0)
                {
                    foreach (var doctorId in model.DoctorIds)
                    {
                        var doctorPatient = new Data_Hospital_Manager.Entities.DoctorPatient
                        {
                            DoctorId = doctorId,
                            PatientId = patient.Id
                        };
                        context.DoctorPatients.Add(doctorPatient);
                    }
                }
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadDoctors(model.DoctorIds);
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            var model = new PatientDeleteViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
            };
            return View(model);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await context.Patients.FindAsync(id);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
