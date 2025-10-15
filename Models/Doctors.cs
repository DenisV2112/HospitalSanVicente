using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSanVicente.Models
{
    public class Doctor
    {
        [Key]
        [Required]
        [Column("document")]
        public int Document { get; set; } 

        [Required]
        [MaxLength(100)]
        [Column("firstname")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("lastname")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column("area")]
        public string Area { get; set; } = string.Empty;

        [Required]
        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Mail { get; set; } = string.Empty;

        // Navigation property - one doctor can have many appointments
        public ICollection<Appointment>? Appointments { get; set; }
        
    }
}
