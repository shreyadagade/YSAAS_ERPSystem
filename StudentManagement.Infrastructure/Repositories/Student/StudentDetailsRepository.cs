using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories.Student;
using StudentManagement.Domain.Entities.Student;
using StudentManagement.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace StudentManagement.Infrastructure.Repositories.Student
{
    public class StudentDetailsRepository : IStudentDetailsRepository
    {
        private readonly AppDbContext _context;

        public StudentDetailsRepository(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET BY ID
        // =====================================================
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

        // =====================================================
        // GET ALL
        // =====================================================
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

        // =====================================================
        // INSERT
        // =====================================================
        public async Task<StudentDetails> AddAsync(StudentDetails student)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = "erpsystem.sp_tblstudent_details";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@Type", "Insert");

            AddParameter(command, "@student_name", student.StudentName);
            AddParameter(command, "@gender", student.Gender);
            AddParameter(command, "@mobile_number", student.MobileNumber);
            AddParameter(command, "@email_address", student.EmailAddress);

            // Password will be generated later for student login.
            AddParameter(command, "@password", student.Password);

            AddParameter(command, "@birth_date", student.BirthDate);
            AddParameter(command, "@profile_photo", student.ProfilePhoto);
            AddParameter(command, "@qualification", student.Qualification);
            AddParameter(command, "@parent_name", student.ParentName);
            AddParameter(command, "@parent_number", student.ParentNumber);
            AddParameter(command, "@student_code", student.StudentCode);
            AddParameter(command, "@last_name", student.LastName);
            AddParameter(command, "@whatsapp_number", student.WhatsappNumber);
            AddParameter(command, "@local_address", student.LocalAddress);
            AddParameter(command, "@permanent_address", student.PermanentAddress);

            AddParameter(
                command,
                "@permanent_identification_number",
                student.PermanentIdentificationNumber);

            AddParameter(
                command,
                "@aadhar_card_number",
                student.AadharCardNumber);

            AddParameter(
                command,
                "@aadhar_card_photo",
                student.AadharCardPhoto);

            AddParameter(command, "@branch_id", student.BranchId);

            await command.ExecuteNonQueryAsync();

            // The current stored procedure does not return
            // the generated student_id.
            //
            // Therefore retrieve the inserted student using email.
            return await GetStudentByEmailAsync(
                student.EmailAddress!);
        }

        // =====================================================
        // UPDATE
        // =====================================================
        public async Task UpdateAsync(StudentDetails student)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = "erpsystem.sp_tblstudent_details";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@Type", "Update");

            AddParameter(command, "@student_id", student.StudentId);
            AddParameter(command, "@student_name", student.StudentName);
            AddParameter(command, "@gender", student.Gender);
            AddParameter(command, "@mobile_number", student.MobileNumber);
            AddParameter(command, "@email_address", student.EmailAddress);
            AddParameter(command, "@password", student.Password);
            AddParameter(command, "@birth_date", student.BirthDate);
            AddParameter(command, "@profile_photo", student.ProfilePhoto);
            AddParameter(command, "@qualification", student.Qualification);
            AddParameter(command, "@parent_name", student.ParentName);
            AddParameter(command, "@parent_number", student.ParentNumber);
            AddParameter(command, "@student_code", student.StudentCode);
            AddParameter(command, "@last_name", student.LastName);
            AddParameter(command, "@whatsapp_number", student.WhatsappNumber);
            AddParameter(command, "@local_address", student.LocalAddress);
            AddParameter(command, "@permanent_address", student.PermanentAddress);

            AddParameter(
                command,
                "@permanent_identification_number",
                student.PermanentIdentificationNumber);

            AddParameter(
                command,
                "@aadhar_card_number",
                student.AadharCardNumber);

            AddParameter(
                command,
                "@aadhar_card_photo",
                student.AadharCardPhoto);

            AddParameter(command, "@branch_id", student.BranchId);

            await command.ExecuteNonQueryAsync();
        }

        // =====================================================
        // DELETE
        // =====================================================
        public async Task DeleteAsync(int studentId)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = "erpsystem.sp_tblstudent_details";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@Type", "Delete");
            AddParameter(command, "@student_id", studentId);

            await command.ExecuteNonQueryAsync();
        }

        // =====================================================
        // RESTORE
        // =====================================================
        public async Task RestoreAsync(int studentId)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = "erpsystem.sp_tblstudent_details";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@Type", "Restore");
            AddParameter(command, "@student_id", studentId);

            await command.ExecuteNonQueryAsync();
        }

        // =====================================================
        // GET STUDENT BY EMAIL
        // =====================================================
        private async Task<StudentDetails> GetStudentByEmailAsync(
            string email)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT
                    s.student_id,
                    s.student_name,
                    s.gender,
                    s.mobile_number,
                    s.email_address,
                    s.password,
                    s.birth_date,
                    s.profile_photo,
                    s.qualification,
                    s.parent_name,
                    s.parent_number,
                    s.student_code,
                    s.last_name,
                    s.whatsapp_number,
                    s.local_address,
                    s.permanent_address,
                    s.permanent_identification_number,
                    s.aadhar_card_number,
                    s.aadhar_card_photo,
                    s.branch_id,
                    b.branch_name
                FROM erpsystem.tblstudent_details s
                LEFT JOIN erpsystem.tblbranches b
                    ON s.branch_id = b.branch_id
                WHERE s.email_address = @email
                  AND s.flag = 0";

            command.CommandType = CommandType.Text;

            AddParameter(command, "@email", email);

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                throw new Exception(
                    "Student was created but could not be retrieved.");
            }

            return MapStudent(reader);
        }

        // =====================================================
        // MAP DATABASE RESULT TO ENTITY
        // =====================================================
        private static StudentDetails MapStudent(
            DbDataReader reader)
        {
            return new StudentDetails
            {
                StudentId =
                    Convert.ToInt32(reader["student_id"]),

                StudentName =
                    reader["student_name"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["student_name"]),

                Gender =
                    reader["gender"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["gender"]),

                MobileNumber =
                    reader["mobile_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["mobile_number"]),

                EmailAddress =
                    reader["email_address"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["email_address"]),

                Password =
                    reader["password"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["password"]),

                BirthDate =
                    reader["birth_date"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["birth_date"]),

                ProfilePhoto =
                    reader["profile_photo"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["profile_photo"]),

                Qualification =
                    reader["qualification"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["qualification"]),

                ParentName =
                    reader["parent_name"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["parent_name"]),

                ParentNumber =
                    reader["parent_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["parent_number"]),

                StudentCode =
                    reader["student_code"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["student_code"]),

                LastName =
                    reader["last_name"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["last_name"]),

                WhatsappNumber =
                    reader["whatsapp_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["whatsapp_number"]),

                LocalAddress =
                    reader["local_address"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["local_address"]),

                PermanentAddress =
                    reader["permanent_address"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["permanent_address"]),

                PermanentIdentificationNumber =
                    reader["permanent_identification_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["permanent_identification_number"]),

                AadharCardNumber =
                    reader["aadhar_card_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["aadhar_card_number"]),

                AadharCardPhoto =
                    reader["aadhar_card_photo"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["aadhar_card_photo"]),

                BranchId =
                    reader["branch_id"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(
                            reader["branch_id"]),

                BranchName =
                    HasColumn(reader, "branch_name") &&
                    reader["branch_name"] != DBNull.Value
                        ? Convert.ToString(
                            reader["branch_name"])
                        : null
            };
        }

        // =====================================================
        // CHECK WHETHER COLUMN EXISTS
        // =====================================================
        private static bool HasColumn(
            DbDataReader reader,
            string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(
                    reader.GetName(i),
                    columnName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // =====================================================
        // ADD SQL PARAMETER
        // =====================================================
        private static void AddParameter(
            DbCommand command,
            string parameterName,
            object? value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = parameterName;

            parameter.Value =
                value ?? DBNull.Value;

            command.Parameters.Add(parameter);
        }

// =====================================================
// GET STUDENT BY STUDENT CODE - LOGIN
// =====================================================
public async Task<StudentDetails?> GetByStudentCodeAsync(
    string studentCode)
        {
            var connection =
                _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command =
                connection.CreateCommand();

            command.CommandText =
                "erpsystem.sp_tblstudent_details";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetByStudentCode");

            AddParameter(
                command,
                "@student_code",
                studentCode);

            using var reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapStudent(reader);
        }



    }
}