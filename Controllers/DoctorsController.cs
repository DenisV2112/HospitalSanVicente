using Microsoft.AspNetCore.Mvc;
using HospitalSanVicente.Data;
using HospitalSanVicente.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalSanVicente.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly HospitalContext _context;

        public DoctorsController(HospitalContext context)
        {
            _context = context;
        }

        // Acción para listar todos los doctores o filtrar por especialidad
        public async Task<IActionResult> List(string specialty)
        {
            // Llenamos ViewBag.Specialties con todas las áreas disponibles
            ViewBag.Specialties = await _context.Doctors
                .Select(d => d.Area)
                .Distinct()
                .ToListAsync();

            List<Doctor> doctors;

            if (!string.IsNullOrEmpty(specialty))
            {
                doctors = await _context.Doctors
                    .Where(d => d.Area == specialty)
                    .ToListAsync();
                ViewBag.Filter = $"Doctores filtrados por especialidad: {specialty}";
            }
            else
            {
                doctors = await _context.Doctors.ToListAsync();
            }

            return View(doctors);
        }
    }
}
