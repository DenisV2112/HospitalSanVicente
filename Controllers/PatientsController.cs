using Microsoft.AspNetCore.Mvc;
using HospitalSanVicente.Data;
using HospitalSanVicente.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalSanVicente.Controllers
{
    public class PatientsController : Controller
    {
        private readonly HospitalContext _context;

        public PatientsController(HospitalContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> List()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }
        

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            return View(patient);
        }


        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient)
        {
            if (id != patient.Document)
                return NotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Paciente actualizado correctamente.";
                    return RedirectToAction(nameof(List));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Patients.Any(p => p.Document == id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el paciente.");
                Console.WriteLine(ex.Message);
            }

            return View(patient);
        }

        // GET: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                    return NotFound();

                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Paciente eliminado correctamente.";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "No se puede eliminar el paciente porque tiene citas asociadas.";
                Console.WriteLine($"Error DB: {ex.Message}");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error inesperado al eliminar el paciente.";
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
