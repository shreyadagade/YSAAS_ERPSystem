using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Domain.Entities.Registration;
using StudentManagement.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace StudentManagement.Infrastructure.Repositories.Registration
{
    public class StudentRegistrationRepository
        : IStudentRegistrationRepository
    {
        private readonly AppDbContext _context;

        public StudentRegistrationRepository(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET BY ID
        // =====================================================
        public async Task<StudentRegistration?> GetByIdAsync(
            int registrationId)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText =
                "erpsystem.sp_tblstudent_registrations";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetById");

            AddParameter(
                command,
                "@registration_id",
                registrationId);

            using var reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapRegistration(reader);
        }

        // =====================================================
        // GET ALL
        // =====================================================
        public async Task<IEnumerable<StudentRegistration>> GetAllAsync()
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText =
                "erpsystem.sp_tblstudent_registrations";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetAll");

            using var reader =
                await command.ExecuteReaderAsync();

            var registrations =
                new List<StudentRegistration>();

            while (await reader.ReadAsync())
            {
                registrations.Add(
                    MapRegistration(reader));
            }

            return registrations;
        }

        // =====================================================
        // INSERT
        // =====================================================
        public async Task<StudentRegistration> AddAsync(
            StudentRegistration registration)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText =
                "erpsystem.sp_tblstudent_registrations";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "Insert");

            AddParameter(
                command,
                "@student_id",
                registration.StudentId);

            AddParameter(
                command,
                "@registration_date",
                registration.RegistrationDate);

            AddParameter(
                command,
                "@discount",
                registration.Discount);

            AddParameter(
                command,
                "@course_id",
                registration.CourseId);

            AddParameter(
                command,
                "@current_status",
                registration.CurrentStatus);

            using var reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var registrationId =
                    reader["registration_id"];

                if (registrationId != DBNull.Value)
                {
                    registration.RegistrationId =
                        Convert.ToInt32(registrationId);
                }
            }

            return registration;
        }

        // =====================================================
        // UPDATE
        // =====================================================
        public async Task UpdateAsync(
            StudentRegistration registration)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_registrations
                  @Type = 'Update',
                  @registration_id = {0},
                  @student_id = {1},
                  @registration_date = {2},
                  @discount = {3},
                  @course_id = {4},
                  @current_status = {5}",
                registration.RegistrationId,
                registration.StudentId,
                registration.RegistrationDate,
                registration.Discount,
                registration.CourseId,
                registration.CurrentStatus);
        }

        // =====================================================
        // DELETE
        // =====================================================
        public async Task DeleteAsync(int registrationId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_registrations
                  @Type = 'Delete',
                  @registration_id = {0}",
                registrationId);
        }

        // =====================================================
        // RESTORE
        // =====================================================
        public async Task RestoreAsync(int registrationId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_registrations
                  @Type = 'Restore',
                  @registration_id = {0}",
                registrationId);
        }

        // =====================================================
        // MAP DATABASE RESULT TO ENTITY
        // =====================================================
        private static StudentRegistration MapRegistration(
            DbDataReader reader)
        {
            return new StudentRegistration
            {
                RegistrationId =
                    Convert.ToInt32(
                        reader["registration_id"]),

                StudentId =
                    reader["student_id"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(
                            reader["student_id"]),

                RegistrationDate =
                    reader["registration_date"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(
                            reader["registration_date"]),

                Discount =
                    reader["discount"] == DBNull.Value
                        ? null
                        : Convert.ToDouble(
                            reader["discount"]),

                CourseId =
                    reader["course_id"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(
                            reader["course_id"]),

                CurrentStatus =
                    reader["current_status"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["current_status"])
            };
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