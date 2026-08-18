using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Domain.Entities.Registration;
using StudentManagement.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace StudentManagement.Infrastructure.Repositories.Registration
{
    public class StudentDetailsRepository : IStudentDetailsRepository
    {
        private readonly AppDbContext _context;

        public StudentDetailsRepository(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<StudentDetails?> GetByIdAsync(int studentId)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = "erpsystem.sp_tblstudent_details";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@Type", "GetById");
            AddParameter(command, "@student_id", studentId);

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapStudent(reader);
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<StudentDetails>> GetAllAsync()
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = "erpsystem.sp_tblstudent_details";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@Type", "GetAll");

            using var reader = await command.ExecuteReaderAsync();

            var students = new List<StudentDetails>();

            while (await reader.ReadAsync())
            {
                students.Add(MapStudent(reader));
            }

            return students;
        }

        // =========================
        // INSERT
        // =========================
        public async Task<StudentDetails> AddAsync(StudentDetails student)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_details
                  @Type = 'Insert',
                  @student_name = {0},
                  @gender = {1},
                  @mobile_number = {2},
                  @email_address = {3},
                  @password = {4},
                  @birth_date = {5},
                  @profile_photo = {6},
                  @qualification = {7},
                  @parent_name = {8},
                  @parent_number = {9},
                  @student_code = {10},
                  @last_name = {11},
                  @whatsapp_number = {12},
                  @local_address = {13},
                  @permanent_address = {14},
                  @permanent_identification_number = {15},
                  @aadhar_card_number = {16},
                  @aadhar_card_photo = {17},
                  @branch_id = {18}",
                student.StudentName,
                student.Gender,
                student.MobileNumber,
                student.EmailAddress,
                student.Password,
                student.BirthDate,
                student.ProfilePhoto,
                student.Qualification,
                student.ParentName,
                student.ParentNumber,
                student.StudentCode,
                student.LastName,
                student.WhatsAppNumber,
                student.LocalAddress,
                student.PermanentAddress,
                student.PermanentIdentificationNumber,
                student.AadharCardNumber,
                student.AadharCardPhoto,
                student.BranchId);

            return student;
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(StudentDetails student)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_details
                  @Type = 'Update',
                  @student_id = {0},
                  @student_name = {1},
                  @gender = {2},
                  @mobile_number = {3},
                  @email_address = {4},
                  @password = {5},
                  @birth_date = {6},
                  @profile_photo = {7},
                  @qualification = {8},
                  @parent_name = {9},
                  @parent_number = {10},
                  @student_code = {11},
                  @last_name = {12},
                  @whatsapp_number = {13},
                  @local_address = {14},
                  @permanent_address = {15},
                  @permanent_identification_number = {16},
                  @aadhar_card_number = {17},
                  @aadhar_card_photo = {18},
                  @branch_id = {19}",
                student.StudentId,
                student.StudentName,
                student.Gender,
                student.MobileNumber,
                student.EmailAddress,
                student.Password,
                student.BirthDate,
                student.ProfilePhoto,
                student.Qualification,
                student.ParentName,
                student.ParentNumber,
                student.StudentCode,
                student.LastName,
                student.WhatsAppNumber,
                student.LocalAddress,
                student.PermanentAddress,
                student.PermanentIdentificationNumber,
                student.AadharCardNumber,
                student.AadharCardPhoto,
                student.BranchId);
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(int studentId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_details
                  @Type = 'Delete',
                  @student_id = {0}",
                studentId);
        }

        // =========================
        // RESTORE
        // =========================
        public async Task RestoreAsync(int studentId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_details
                  @Type = 'Restore',
                  @student_id = {0}",
                studentId);
        }

        // =========================
        // MAPPING
        // =========================
        private static StudentDetails MapStudent(DbDataReader reader)
        {
            return new StudentDetails
            {
                StudentId = GetInt(reader, "student_id"),

                StudentName = GetString(reader, "student_name"),

                BranchName = GetString(reader, "branch_name"),

                Gender = GetString(reader, "gender"),

                MobileNumber = GetString(reader, "mobile_number"),

                EmailAddress =
                    GetString(reader, "email_address")
                    ?? string.Empty,

                Password = GetString(reader, "password"),

                BirthDate = GetDateTime(reader, "birth_date"),

                ProfilePhoto = GetString(reader, "profile_photo"),

                Qualification = GetString(reader, "qualification"),

                ParentName = GetString(reader, "parent_name"),

                ParentNumber = GetString(reader, "parent_number"),

                StudentCode = GetString(reader, "student_code"),

                LastName = GetString(reader, "last_name"),

                WhatsAppNumber =
                    GetString(reader, "whatsapp_number"),

                LocalAddress =
                    GetString(reader, "local_address"),

                PermanentAddress =
                    GetString(reader, "permanent_address"),

                PermanentIdentificationNumber =
                    GetString(
                        reader,
                        "permanent_identification_number")
                    ?? string.Empty,

                // IMPORTANT:
                // Your SP returns aadhar_card_number
                AadharCardNumber =
                    GetString(reader, "aadhar_card_number"),

                // Your SP returns aadhar_card_photo
                AadharCardPhoto =
                    GetString(reader, "aadhar_card_photo"),

                BranchId =
                    GetInt(reader, "branch_id")
            };
        }

        // =========================
        // ADD PARAMETER
        // =========================
        private static void AddParameter(
            DbCommand command,
            string parameterName,
            object value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;

            command.Parameters.Add(parameter);
        }

        // =========================
        // GET STRING
        // =========================
        private static string? GetString(
            DbDataReader reader,
            string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return Convert.ToString(reader.GetValue(ordinal));
        }

        // =========================
        // GET INT
        // =========================
        private static int GetInt(
            DbDataReader reader,
            string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
            {
                return 0;
            }

            return Convert.ToInt32(reader.GetValue(ordinal));
        }

        // =========================
        // GET DATETIME
        // =========================
        private static DateTime? GetDateTime(
            DbDataReader reader,
            string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return Convert.ToDateTime(reader.GetValue(ordinal));
        }
    }
}