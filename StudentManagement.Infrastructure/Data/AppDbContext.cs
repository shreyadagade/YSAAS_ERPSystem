using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.DTOs.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudentDetails> StudentDetails { get; set; }

        public DbSet<StudentQualification> StudentQualifications { get; set; }

        public DbSet<StudentRegistration> StudentRegistrations { get; set; }

        public DbSet<StudentPayment> StudentPayments { get; set; }

        public DbSet<StudentDetailsDto> StudentDetailsResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Student Details
            // =========================
            modelBuilder.Entity<StudentDetails>()
                .HasKey(x => x.StudentId);

            modelBuilder.Entity<StudentDetails>()
                .ToTable("tblstudent_details", "erpsystem");

            // =========================
            // Student Qualification
            // =========================
            modelBuilder.Entity<StudentQualification>()
                .HasKey(x => x.QualificationId);

            modelBuilder.Entity<StudentQualification>()
                .ToTable("tblstudent_qualifications", "erpsystem");

            // =========================
            // Student Registration
            // =========================
            modelBuilder.Entity<StudentRegistration>()
                .HasKey(x => x.RegistrationId);

            modelBuilder.Entity<StudentRegistration>()
                .ToTable("tblstudent_registrations", "erpsystem");

            // =========================
            // Student Payment
            // =========================
            modelBuilder.Entity<StudentPayment>()
                .HasKey(x => x.PaymentId);

            modelBuilder.Entity<StudentPayment>()
                .ToTable("tblstudent_payments", "erpsystem");

            // =========================
            // Student Details SP Result
            // =========================
            modelBuilder.Entity<StudentDetailsDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(x => x.AadharCardNumber)
                    .HasColumnName("adhar_card_number");

                entity.Property(x => x.AadharCardPhoto)
                    .HasColumnName("aadhar_card_photo");
            });
        }
    }
}