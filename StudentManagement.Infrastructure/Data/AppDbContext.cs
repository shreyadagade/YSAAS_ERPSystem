using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.DTOs.Registration;
using StudentManagement.Domain.Entities.Registration;

using CourseEntity = StudentManagement.Domain.Entities.Course.Course;

namespace StudentManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<CourseEntity> Courses { get; set; }

        public DbSet<StudentDetails> StudentDetails { get; set; }

        public DbSet<StudentQualification> StudentQualifications { get; set; }

        public DbSet<StudentRegistration> StudentRegistrations { get; set; }

        public DbSet<StudentPayment> StudentPayments { get; set; }

        public DbSet<StudentDetailsDto> StudentDetailsResults { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // =========================
            // TRAINING COURSES
            // =========================

            modelBuilder.Entity<CourseEntity>(entity =>
            {
                entity.HasKey(x => x.CourseId);

                entity.ToTable(
                    "tbltraining_courses",
                    "erpsystem");

                entity.Property(x => x.CourseId)
                    .HasColumnName("course_id");

                entity.Property(x => x.CourseName)
                    .HasColumnName("course_name");

                entity.Property(x => x.Flag)
                    .HasColumnName("flag");

                entity.Property(x => x.InsertedAt)
                    .HasColumnName("InsertedAt");

                entity.Property(x => x.UpdatedAt)
                    .HasColumnName("UpdatedAt");

                entity.Property(x => x.DeletedAt)
                    .HasColumnName("DeletedAt");

                entity.Property(x => x.RestoredAt)
                    .HasColumnName("RestoredAt");

                entity.Property(x => x.FeesAmount)
                    .HasColumnName("fees_amount");

                entity.Property(x => x.FeesChangeDate)
                    .HasColumnName("fees_change_date");

                entity.Property(x => x.InstallmentPercentage)
                    .HasColumnName("installment_percentage");
            });


            // =========================
            // STUDENT DETAILS
            // =========================

            modelBuilder.Entity<StudentDetails>(entity =>
            {
                entity.HasKey(x => x.StudentId);

                entity.ToTable(
                    "tblstudent_details",
                    "erpsystem");
            });


            // =========================
            // STUDENT QUALIFICATION
            // =========================

            modelBuilder.Entity<StudentQualification>(entity =>
            {
                entity.HasKey(x => x.QualificationId);

                entity.ToTable(
                    "tblstudent_qualifications",
                    "erpsystem");
            });


            // =========================
            // STUDENT REGISTRATION
            // =========================

            modelBuilder.Entity<StudentRegistration>(entity =>
            {
                entity.HasKey(x => x.RegistrationId);

                entity.ToTable(
                    "tblstudent_registrations",
                    "erpsystem");
            });


            // =========================
            // STUDENT PAYMENT
            // =========================

            modelBuilder.Entity<StudentPayment>(entity =>
            {
                entity.HasKey(x => x.PaymentId);

                entity.ToTable(
                    "tblstudent_payments",
                    "erpsystem");
            });


            // =========================
            // STUDENT DETAILS SP RESULT
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