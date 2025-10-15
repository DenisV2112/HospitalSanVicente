using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSanVicente.Models
{
    public class EmailLog
    {
        [Key]
        public int Id { get; set; }

        public int? AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public Appointment? Appointment { get; set; }

        [Required, MaxLength(255)]
        public string RecipientEmail { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "Sent";

        public string? ErrorMessage { get; set; }
    }
}
