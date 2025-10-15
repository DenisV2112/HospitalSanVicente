using Microsoft.AspNetCore.Mvc;
using HospitalSanVicente.Data;
using HospitalSanVicente.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using HospitalSanVicente.Helpers;

namespace HospitalSanVicente.Controllers
{
    public class AppointmentsController : Controller
    {  
        private readonly HospitalContext _context;
    private readonly SmtpSettings _smtpSettings;

    public AppointmentsController(HospitalContext context, IOptions<SmtpSettings> smtpOptions)
    {
        _context = context;
        _smtpSettings = smtpOptions.Value;
    }

        public async Task<IActionResult> List()
        {
            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Doctors = await _context.Doctors.ToListAsync();

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();

            return View(appointments);
        }

        public async Task<IActionResult> ListByPatient(int patientDocument)
        {
            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Doctors = await _context.Doctors.ToListAsync();

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientDocument == patientDocument)
                .ToListAsync();

            ViewBag.Filter = $"Citas del paciente: {appointments.FirstOrDefault()?.Patient.FirstName} {appointments.FirstOrDefault()?.Patient.LastName}";
            return View("List", appointments);
        }

        public async Task<IActionResult> ListByDoctor(int doctorDocument)
        {
            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Doctors = await _context.Doctors.ToListAsync();

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorDocument == doctorDocument)
                .ToListAsync();

            ViewBag.Filter = $"Citas del doctor: {appointments.FirstOrDefault()?.Doctor.FirstName} {appointments.FirstOrDefault()?.Doctor.LastName}";
            return View("List", appointments);
        }

        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Doctors = await _context.Doctors.ToListAsync();
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _context.Patients.ToListAsync();
                ViewBag.Doctors = await _context.Doctors.ToListAsync();
                return View(appointment);
            }

            var start = appointment.Date.AddHours(-1);
            var end = appointment.Date.AddHours(1);

            bool hasConflict = await _context.Appointments
                .AnyAsync(a => a.DoctorDocument == appointment.DoctorDocument
                            && a.Date >= start
                            && a.Date <= end);

            if (hasConflict)
            {
                ModelState.AddModelError("", "El doctor ya tiene otra cita programada dentro de 1 hora de esta hora.");
                ViewBag.Patients = await _context.Patients.ToListAsync();
                ViewBag.Doctors = await _context.Doctors.ToListAsync();
                return View(appointment);
            }

            try
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();

                // Enviar email y registrar en EmailLogs
                try
                {
                    await SendEmailAndLogAsync(appointment);
                    TempData["SuccessMessage"] = "Cita creada correctamente y correo de confirmación enviado.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al enviar email: {ex.Message}");
                    TempData["ErrorMessage"] = "Cita creada, pero hubo un error al enviar el correo de confirmación.";
                }

                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al crear la cita.");
                Console.WriteLine(ex.Message);
                return View(appointment);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, string newStatus)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.Status = newStatus;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Cita actualizada a {newStatus}.";
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> EmailHistory()
        {
            var emails = await _context.EmailLogs
                .Include(e => e.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(e => e.Appointment)
                    .ThenInclude(a => a.Doctor)
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();

            return View(emails);
        }

        private async Task SendEmailAndLogAsync(Appointment appointment)
        {
            var patient = await _context.Patients.FindAsync(appointment.PatientDocument);
            var doctor = await _context.Doctors.FindAsync(appointment.DoctorDocument);

            if (patient == null || doctor == null)
                throw new InvalidOperationException("Paciente o Doctor no encontrado para enviar email.");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hospital San Vicente", _smtpSettings.User));
            message.To.Add(new MailboxAddress($"{patient.FirstName} {patient.LastName}", patient.Email));
            message.Subject = "Confirmación de cita médica";
            message.Body = new TextPart("plain")
            {
                Text = $"Hola {patient.FirstName},\n\n" +
                       $"Se ha programado una cita con el doctor {doctor.FirstName} {doctor.LastName} " +
                       $"para el día {appointment.Date:dd/MM/yyyy HH:mm}.\n\nGracias."
            };

        using var client = new SmtpClient();

await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);

await client.AuthenticateAsync(_smtpSettings.User, _smtpSettings.Pass);

await client.SendAsync(message);

await client.DisconnectAsync(true);

            // EmailLogs
            var emailLog = new EmailLog
            {
                AppointmentId = appointment.Id,
                RecipientEmail = patient.Email,
                Subject = message.Subject,
                Body = message.Body.ToString(),
                SentAt = DateTime.Now,
                Status = "Sent"
            };

            _context.EmailLogs.Add(emailLog);
            await _context.SaveChangesAsync();
        }
    }
}
