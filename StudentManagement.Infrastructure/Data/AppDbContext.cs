using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities.Registration;
using StudentManagement.Domain.Entities.Student;

namespace StudentManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =====================================================
        // DBSETS
        // =====================================================

        // Student Details
        public DbSet<StudentDetails> StudentDetails
        {
            get;
            set;
        }

        // Student Registration
        public DbSet<StudentRegistration> StudentRegistrations
        {
            get;
            set;
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================================
            // STUDENT DETAILS
            // =====================================================

            modelBuilder.Entity<StudentDetails>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.ToTable(
                    "tblstudent_details",
                    "erpsystem");

                entity.Property(e => e.StudentId)
                    .HasColumnName("student_id");

                entity.Property(e => e.StudentName)
                    .HasColumnName("student_name");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender");

                entity.Property(e => e.MobileNumber)
                    .HasColumnName("mobile_number");

                entity.Property(e => e.EmailAddress)
                    .HasColumnName("email_address");

                entity.Property(e => e.Password)
                    .HasColumnName("password");

                entity.Property(e => e.BirthDate)
                    .HasColumnName("birth_date");

                entity.Property(e => e.ProfilePhoto)
                    .HasColumnName("profile_photo");

                entity.Property(e => e.Qualification)
                    .HasColumnName("qualification");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag");

                entity.Property(e => e.ParentName)
                    .HasColumnName("parent_name");

                entity.Property(e => e.ParentNumber)
                    .HasColumnName("parent_number");

                entity.Property(e => e.StudentCode)
                    .HasColumnName("student_code");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name");

                entity.Property(e => e.WhatsappNumber)
                    .HasColumnName("whatsapp_number");

                entity.Property(e => e.LocalAddress)
                    .HasColumnName("local_address");

                entity.Property(e => e.PermanentAddress)
                    .HasColumnName("permanent_address");

                entity.Property(
                        e => e.PermanentIdentificationNumber)
                    .HasColumnName(
                        "permanent_identification_number");

                entity.Property(e => e.AadharCardNumber)
                    .HasColumnName("aadhar_card_number");

                entity.Property(e => e.AadharCardPhoto)
                    .HasColumnName("aadhar_card_photo");

                entity.Property(e => e.BranchId)
                    .HasColumnName("branch_id");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("InsertedAt");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("DeletedAt");

                entity.Property(e => e.RestoredAt)
                    .HasColumnName("RestoredAt");
            });

            // =====================================================
            // STUDENT REGISTRATION
            // =====================================================

            modelBuilder.Entity<StudentRegistration>(entity =>
            {
                entity.HasKey(e => e.RegistrationId);

                entity.ToTable(
                    "tblstudent_registrations",
                    "erpsystem");

                entity.Property(e => e.RegistrationId)
                    .HasColumnName("registration_id");

                entity.Property(e => e.StudentId)
                    .HasColumnName("student_id");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnName("registration_date");

                entity.Property(e => e.Discount)
                    .HasColumnName("discount");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag");

                entity.Property(e => e.CurrentStatus)
                    .HasColumnName("current_status");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("InsertedAt");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("DeletedAt");

                entity.Property(e => e.RestoredAt)
                    .HasColumnName("RestoredAt");

                entity.Property(e => e.CourseId)
                    .HasColumnName("course_id");
            });
        }
    }
}

