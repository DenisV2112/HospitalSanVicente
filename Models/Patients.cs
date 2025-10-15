using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSanVicente.Models
{
public enum BloodType
    {
        A_Positive,  
        A_Negative,  
        B_Positive,  
        B_Negative,  
        AB_Positive, 
        AB_Negative, 
        O_Positive,  
        O_Negative  
    }
    public class Patient
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
        [Range(0, 120)]
        [Column("age")]
        public int Age { get; set; }

        [Required]
        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        [Column("email")]
        public string? Email { get; set; }

        [MaxLength(5)]
        [Column("bloodType")]
        public string? BloodType { get; set; }

        // Navigation property - one patient can have many appointments
        public ICollection<Appointment>? Appointments { get; set; }
        
    }
}
