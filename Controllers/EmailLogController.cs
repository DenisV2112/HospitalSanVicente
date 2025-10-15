using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanVicente.Data;
using HospitalSanVicente.Models;

namespace HospitalSanVicente.Controllers
{
    public class EmailLogController : Controller
    {
        private readonly HospitalContext _context;

        public EmailLogController(HospitalContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var emails = await _context.EmailLogs
                .Include(e => e.Appointment)
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();

            return View(emails);
        }
    }
}
