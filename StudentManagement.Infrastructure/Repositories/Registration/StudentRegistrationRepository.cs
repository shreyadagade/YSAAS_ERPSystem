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

        public StudentRegistrationRepository(
            AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<StudentRegistration?> GetByIdAsync(
            int registrationId)
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

            if (await reader.ReadAsync())
            {
                return MapRegistration(reader);
            }

            return null;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<StudentRegistration>>
            GetAllAsync()
        {
            var registrations =
                new List<StudentRegistration>();

            var connection =
                _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command =
                connection.CreateCommand();

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
            var connection =
                _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            int registrationId = 0;

            // =================================================
            // INSERT REGISTRATION
            // =================================================

            using (var command =
                connection.CreateCommand())
            {
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

                // =============================================
                // Execute INSERT
                // =============================================

                using (var reader =
                    await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        registrationId =
                            Convert.ToInt32(
                                reader["registration_id"]);
                    }
                }

                // IMPORTANT:
                // Reader is completely disposed here
                // before GetByIdAsync() is called.
            }

            // =================================================
            // CHECK INSERT RESULT
            // =================================================

            if (registrationId <= 0)
            {
                throw new InvalidOperationException(
                    "Registration could not be created.");
            }

            // =================================================
            // GET CREATED REGISTRATION
            // =================================================

            var createdRegistration =
                await GetByIdAsync(
                    registrationId);

            if (createdRegistration == null)
            {
                throw new InvalidOperationException(
                    "Registration was created but could not be retrieved.");
            }

            return createdRegistration;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task UpdateAsync(
            StudentRegistration registration)
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
                "erpsystem.sp_tblstudent_registrations";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "Update");

            AddParameter(
                command,
                "@registration_id",
                registration.RegistrationId);

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

            await command.ExecuteNonQueryAsync();
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task DeleteAsync(
            int registrationId)
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
                "erpsystem.sp_tblstudent_registrations";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "Delete");

            AddParameter(
                command,
                "@registration_id",
                registrationId);

            await command.ExecuteNonQueryAsync();
        }

        // =====================================================
        // RESTORE
        // =====================================================

        public async Task<bool> RestoreAsync(
            int registrationId)
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
                "erpsystem.sp_tblstudent_registrations";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "Restore");

            AddParameter(
                command,
                "@registration_id",
                registrationId);

            using var reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var success =
                    reader["Success"];

                return Convert.ToInt32(success) == 1;
            }

            return false;
        }

        // =====================================================
        // MAP DATABASE RESULT
        // =====================================================

        private static StudentRegistration
            MapRegistration(
                DbDataReader reader)
        {
            return new StudentRegistration
            {
                RegistrationId =
                    reader["registration_id"] != DBNull.Value
                        ? Convert.ToInt32(
                            reader["registration_id"])
                        : 0,

                StudentId =
                    reader["student_id"] != DBNull.Value
                        ? Convert.ToInt32(
                            reader["student_id"])
                        : null,

                StudentName =
                    reader["student_name"] != DBNull.Value
                        ? reader["student_name"].ToString()
                        : null,

                RegistrationDate =
                    reader["registration_date"] != DBNull.Value
                        ? Convert.ToDateTime(
                            reader["registration_date"])
                        : null,

                Discount =
                    reader["discount"] != DBNull.Value
                        ? Convert.ToDouble(
                            reader["discount"])
                        : null,

                CourseId =
                    reader["course_id"] != DBNull.Value
                        ? Convert.ToInt32(
                            reader["course_id"])
                        : null,

                CourseName =
                    reader["course_name"] != DBNull.Value
                        ? reader["course_name"].ToString()
                        : null,

                FeesAmount =
                    reader["fees_amount"] != DBNull.Value
                        ? Convert.ToDouble(
                            reader["fees_amount"])
                        : null,

                FeesChangeDate =
                    reader["fees_change_date"] != DBNull.Value
                        ? Convert.ToDateTime(
                            reader["fees_change_date"])
                        : null,

                InstallmentPercentage =
                    reader["installment_percentage"] != DBNull.Value
                        ? Convert.ToDouble(
                            reader["installment_percentage"])
                        : null,

                CurrentStatus =
                    reader["current_status"] != DBNull.Value
                        ? reader["current_status"].ToString()
                        : null
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