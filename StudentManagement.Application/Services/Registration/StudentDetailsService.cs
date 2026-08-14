using System.Net.Mail;
using System.Text.RegularExpressions;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Services.Registration
{
    public class StudentDetailsService : IStudentDetailsService
    {
        private readonly IStudentDetailsRepository _repository;

        public StudentDetailsService(
            IStudentDetailsRepository repository)
        {
            _repository = repository;
        }

        // ==========================================
        // GET BY ID
        // ==========================================
        public async Task<StudentDetails?> GetByIdAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "Student ID must be greater than 0.");
            }

            return await _repository.GetByIdAsync(studentId);
        }

        // ==========================================
        // GET ALL
        // ==========================================
        public async Task<IEnumerable<StudentDetails>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // ==========================================
        // ADD
        // ==========================================
        public async Task<StudentDetails> AddAsync(
            StudentDetails student)
        {
            ValidateStudent(student);

            return await _repository.AddAsync(student);
        }

        // ==========================================
        // UPDATE
        // ==========================================
        public async Task UpdateAsync(
            StudentDetails student)
        {
            if (student.StudentId <= 0)
            {
                throw new ArgumentException(
                    "Student ID must be greater than 0.");
            }

            ValidateStudent(student);

            var existing =
                await _repository.GetByIdAsync(student.StudentId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Student not found.");
            }

            await _repository.UpdateAsync(student);
        }

        // ==========================================
        // DELETE
        // ==========================================
        public async Task DeleteAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "Student ID must be greater than 0.");
            }

            var existing =
                await _repository.GetByIdAsync(studentId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Student not found.");
            }

            await _repository.DeleteAsync(studentId);
        }

        // ==========================================
        // RESTORE
        // ==========================================
        public async Task RestoreAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "Student ID must be greater than 0.");
            }

            await _repository.RestoreAsync(studentId);
        }

        // ==========================================
        // VALIDATION
        // ==========================================
        private static void ValidateStudent(
            StudentDetails student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(
                    nameof(student),
                    "Student data is required.");
            }

            // ------------------------------------------
            // Student Name
            // ------------------------------------------
            if (string.IsNullOrWhiteSpace(student.StudentName))
            {
                throw new ArgumentException(
                    "Student name is required.");
            }

            if (student.StudentName.Length > 100)
            {
                throw new ArgumentException(
                    "Student name cannot exceed 100 characters.");
            }

            // ------------------------------------------
            // Gender
            // ------------------------------------------
            if (string.IsNullOrWhiteSpace(student.Gender))
            {
                throw new ArgumentException(
                    "Gender is required.");
            }

            if (student.Gender.Length > 10)
            {
                throw new ArgumentException(
                    "Gender cannot exceed 10 characters.");
            }

            // ------------------------------------------
            // Mobile Number
            // ------------------------------------------
            if (string.IsNullOrWhiteSpace(student.MobileNumber))
            {
                throw new ArgumentException(
                    "Mobile number is required.");
            }

            if (!Regex.IsMatch(
                student.MobileNumber,
                @"^\d{10,20}$"))
            {
                throw new ArgumentException(
                    "Mobile number must contain only digits and be between 10 and 20 digits.");
            }

            // ------------------------------------------
            // Email Address
            // ------------------------------------------
            if (string.IsNullOrWhiteSpace(student.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            if (student.EmailAddress.Length > 100)
            {
                throw new ArgumentException(
                    "Email address cannot exceed 100 characters.");
            }

            if (!IsValidEmail(student.EmailAddress))
            {
                throw new ArgumentException(
                    "Please enter a valid email address.");
            }

            // ------------------------------------------
            // Password
            // ------------------------------------------
            if (string.IsNullOrWhiteSpace(student.Password))
            {
                throw new ArgumentException(
                    "Password is required.");
            }

            if (student.Password.Length < 6)
            {
                throw new ArgumentException(
                    "Password must contain at least 6 characters.");
            }

            if (student.Password.Length > 100)
            {
                throw new ArgumentException(
                    "Password cannot exceed 100 characters.");
            }

            // ------------------------------------------
            // Birth Date
            // ------------------------------------------
            if (!student.BirthDate.HasValue)
            {
                throw new ArgumentException(
                    "Birth date is required.");
            }

            if (student.BirthDate.Value.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "Birth date cannot be in the future.");
            }

            // ------------------------------------------
            // Profile Photo
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(student.ProfilePhoto) &&
                student.ProfilePhoto.Length > 100)
            {
                throw new ArgumentException(
                    "Profile photo path cannot exceed 100 characters.");
            }

            // ------------------------------------------
            // Qualification
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(student.Qualification) &&
                student.Qualification.Length > 100)
            {
                throw new ArgumentException(
                    "Qualification cannot exceed 100 characters.");
            }

            // ------------------------------------------
            // Parent Name
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(student.ParentName) &&
                student.ParentName.Length > 100)
            {
                throw new ArgumentException(
                    "Parent name cannot exceed 100 characters.");
            }

            // ------------------------------------------
            // Parent Number
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(student.ParentNumber))
            {
                if (!Regex.IsMatch(
                    student.ParentNumber,
                    @"^\d{10,20}$"))
                {
                    throw new ArgumentException(
                        "Parent number must contain only digits and be between 10 and 20 digits.");
                }
            }

            // ------------------------------------------
            // Student Code
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(student.StudentCode) &&
                student.StudentCode.Length > 20)
            {
                throw new ArgumentException(
                    "Student code cannot exceed 20 characters.");
            }

            // ------------------------------------------
            // Last Name
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(student.LastName) &&
                student.LastName.Length > 100)
            {
                throw new ArgumentException(
                    "Last name cannot exceed 100 characters.");
            }

            // ------------------------------------------
            // WhatsApp Number
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(student.WhatsAppNumber))
            {
                if (!Regex.IsMatch(
                    student.WhatsAppNumber,
                    @"^\d{10,15}$"))
                {
                    throw new ArgumentException(
                        "WhatsApp number must contain only digits and be between 10 and 15 digits.");
                }
            }

            // ------------------------------------------
            // Local Address
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(student.LocalAddress) &&
                student.LocalAddress.Length > 100)
            {
                throw new ArgumentException(
                    "Local address cannot exceed 100 characters.");
            }

            // ------------------------------------------
            // Permanent Address
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(student.PermanentAddress) &&
                student.PermanentAddress.Length > 100)
            {
                throw new ArgumentException(
                    "Permanent address cannot exceed 100 characters.");
            }

            // ------------------------------------------
            // Permanent Identification Number
            // ------------------------------------------
            if (string.IsNullOrWhiteSpace(
                student.PermanentIdentificationNumber))
            {
                throw new ArgumentException(
                    "Permanent identification number is required.");
            }

            if (student.PermanentIdentificationNumber.Length > 15)
            {
                throw new ArgumentException(
                    "Permanent identification number cannot exceed 15 characters.");
            }

            // ------------------------------------------
            // Aadhar Card Number
            // Database column:
            // adhar_card_number
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(
                student.AadharCardNumber))
            {
                if (!Regex.IsMatch(
                    student.AadharCardNumber,
                    @"^\d{12}$"))
                {
                    throw new ArgumentException(
                        "Aadhar card number must contain exactly 12 digits.");
                }
            }

            // ------------------------------------------
            // Aadhar Card Photo
            // ------------------------------------------
            if (!string.IsNullOrWhiteSpace(
                student.AadharCardPhoto) &&
                student.AadharCardPhoto.Length > 100)
            {
                throw new ArgumentException(
                    "Aadhar card photo path cannot exceed 100 characters.");
            }
        }

        // ==========================================
        // EMAIL VALIDATION
        // ==========================================
        private static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);

                return mailAddress.Address.Equals(
                    email,
                    StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}