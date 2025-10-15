using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSanVicente.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("AppointmentDate")]
        public DateTime Date { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; } = "Scheduled"; // Scheduled | Attended | Canceled

        // Foreign key: Patient
        [Required]
        [ForeignKey("Patient")]
        [Column("PatientId")]
        public int PatientDocument { get; set; } 

        public Patient? Patient { get; set; }

        // Foreign key: Doctor
        [Required]
        [ForeignKey("Doctor")]
        [Column("DoctorId")]
        public int DoctorDocument { get; set; } 

        public Doctor? Doctor { get; set; }
    }
}
