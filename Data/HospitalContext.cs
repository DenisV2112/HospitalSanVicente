using Microsoft.EntityFrameworkCore;
using HospitalSanVicente.Models;

namespace HospitalSanVicente.Data
{
    // This class is responsible for managing the database connection
    // and mapping C# models to database tables using Entity Framework Core.
    public class HospitalContext : DbContext
    {
        // Constructor that receives configuration options (like the connection string)
        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        {
        }

        // DbSets represent tables in the database
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }


        // This method defines relationships, keys, and restrictions between entities
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table: Patients
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Document);
                entity.Property(p => p.Document).IsRequired();
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Age).IsRequired();
                entity.Property(p => p.Phone).IsRequired();
                entity.Property(p => p.BloodType).HasMaxLength(5);

                // Relationship: One Patient can have many Appointments
                entity.HasMany(p => p.Appointments)
                      .WithOne(a => a.Patient)
                      .HasForeignKey(a => a.PatientDocument)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Table: Doctors
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.Document);
                entity.Property(d => d.Document).IsRequired();
                entity.Property(d => d.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(d => d.LastName).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Area).IsRequired();
                entity.Property(d => d.Phone).IsRequired();
                entity.Property(d => d.Mail).IsRequired();

                // Relationship: One Doctor can have many Appointments
                entity.HasMany(d => d.Appointments)
                      .WithOne(a => a.Doctor)
                      .HasForeignKey(a => a.DoctorDocument)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Table: Appointments
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).ValueGeneratedOnAdd();
                entity.Property(a => a.Date).IsRequired();
                entity.Property(a => a.Status).IsRequired();

                // Relationships already configured from Patient and Doctor
            });
        }
    }
}
