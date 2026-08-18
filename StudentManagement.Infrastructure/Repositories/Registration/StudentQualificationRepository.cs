using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Domain.Entities.Registration;
using StudentManagement.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace StudentManagement.Infrastructure.Repositories.Registration
{
    public class StudentQualificationRepository
        : IStudentQualificationRepository
    {
        private readonly AppDbContext _context;

        public StudentQualificationRepository(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET BY ID
        // =====================================================
        public async Task<StudentQualification?> GetByIdAsync(
            int qualificationId)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText =
                "erpsystem.sp_tblstudent_qualifications";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetById");

            AddParameter(
                command,
                "@qualification_id",
                qualificationId);

            using var reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapQualification(reader);
        }

        // =====================================================
        // GET ALL
        // =====================================================
        public async Task<IEnumerable<StudentQualification>> GetAllAsync()
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText =
                "erpsystem.sp_tblstudent_qualifications";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetAll");

            using var reader =
                await command.ExecuteReaderAsync();

            var qualifications =
                new List<StudentQualification>();

            while (await reader.ReadAsync())
            {
                qualifications.Add(
                    MapQualification(reader));
            }

            return qualifications;
        }

        // =====================================================
        // INSERT
        // =====================================================
        public async Task<StudentQualification> AddAsync(
            StudentQualification qualification)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_qualifications
                  @Type = 'Insert',
                  @student_id = {0},
                  @qualification = {1},
                  @passing_year = {2},
                  @university = {3},
                  @medium = {4},
                  @percentage = {5}",
                qualification.StudentId,
                qualification.Qualification,
                qualification.PassingYear,
                qualification.University,
                qualification.Medium,
                qualification.Percentage);

            return qualification;
        }

        // =====================================================
        // UPDATE
        // =====================================================
        public async Task UpdateAsync(
            StudentQualification qualification)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_qualifications
                  @Type = 'Update',
                  @qualification_id = {0},
                  @student_id = {1},
                  @qualification = {2},
                  @passing_year = {3},
                  @university = {4},
                  @medium = {5},
                  @percentage = {6}",
                qualification.QualificationId,
                qualification.StudentId,
                qualification.Qualification,
                qualification.PassingYear,
                qualification.University,
                qualification.Medium,
                qualification.Percentage);
        }

        // =====================================================
        // DELETE
        // =====================================================
        public async Task DeleteAsync(int qualificationId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_qualifications
                  @Type = 'Delete',
                  @qualification_id = {0}",
                qualificationId);
        }

        // =====================================================
        // RESTORE
        // =====================================================
        public async Task RestoreAsync(int qualificationId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_qualifications
                  @Type = 'Restore',
                  @qualification_id = {0}",
                qualificationId);
        }

        // =====================================================
        // MAP DATABASE RESULT TO ENTITY
        // =====================================================
        private static StudentQualification MapQualification(
            DbDataReader reader)
        {
            return new StudentQualification
            {
                QualificationId =
                    GetInt(
                        reader,
                        "qualification_id"),

                StudentId =
                    GetIntNullable(
                        reader,
                        "student_id"),

                Qualification =
                    GetString(
                        reader,
                        "qualification"),

                PassingYear =
                    GetIntNullable(
                        reader,
                        "passing_year"),

                University =
                    GetString(
                        reader,
                        "university"),

                Medium =
                    GetString(
                        reader,
                        "medium"),

                Percentage =
                    GetDoubleNullable(
                        reader,
                        "percentage")
            };
        }

        // =====================================================
        // GET STRING
        // =====================================================
        private static string? GetString(
            DbDataReader reader,
            string columnName)
        {
            var value = reader[columnName];

            return value == DBNull.Value
                ? null
                : Convert.ToString(value);
        }

        // =====================================================
        // GET INT
        // =====================================================
        private static int GetInt(
            DbDataReader reader,
            string columnName)
        {
            var value = reader[columnName];

            return value == DBNull.Value
                ? 0
                : Convert.ToInt32(value);
        }

        // =====================================================
        // GET NULLABLE INT
        // =====================================================
        private static int? GetIntNullable(
            DbDataReader reader,
            string columnName)
        {
            var value = reader[columnName];

            return value == DBNull.Value
                ? null
                : Convert.ToInt32(value);
        }

        // =====================================================
        // GET NULLABLE DOUBLE
        // =====================================================
        private static double? GetDoubleNullable(
            DbDataReader reader,
            string columnName)
        {
            var value = reader[columnName];

            return value == DBNull.Value
                ? null
                : Convert.ToDouble(value);
        }

        // =====================================================
        // ADD SQL PARAMETER
        // =====================================================
        private static void AddParameter(
            DbCommand command,
            string parameterName,
            object? value)
        {
            var parameter =
                command.CreateParameter();

            parameter.ParameterName =
                parameterName;

            parameter.Value =
                value ?? DBNull.Value;

            command.Parameters.Add(parameter);
        }
    }
}