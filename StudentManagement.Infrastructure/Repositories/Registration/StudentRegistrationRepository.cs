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

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapRegistration(reader);
        }

        // =====================================================
        // GET ALL
        // =====================================================
        public async Task<IEnumerable<StudentRegistration>>
            GetAllAsync()
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
            var connection =
                _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            int registrationId;

            // -------------------------------------------------
            // INSERT
            // -------------------------------------------------
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

                /*
                 * The INSERT stored procedure returns:
                 *
                 * registration_id
                 * Message
                 *
                 * We read registration_id first.
                 */

                using (var reader =
                    await command.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync())
                    {
                        throw new Exception(
                            "Registration was created but registration ID was not returned.");
                    }

                    registrationId =
                        Convert.ToInt32(
                            reader["registration_id"]);
                }
            }

            /*
             * IMPORTANT:
             *
             * The DataReader is now CLOSED.
             *
             * Therefore we can safely call GetByIdAsync()
             * using the same database connection.
             */

            var createdRegistration =
                await GetByIdAsync(registrationId);

            if (createdRegistration == null)
            {
                throw new Exception(
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
        public async Task RestoreAsync(
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

            await command.ExecuteNonQueryAsync();
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

                StudentName =
                    reader["student_name"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["student_name"]),

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

                CourseName =
                    reader["course_name"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["course_name"]),

                FeesAmount =
                    reader["fees_amount"] == DBNull.Value
                        ? null
                        : Convert.ToDouble(
                            reader["fees_amount"]),

                FeesChangeDate =
                    reader["fees_change_date"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(
                            reader["fees_change_date"]),

                InstallmentPercentage =
                    reader["installment_percentage"] == DBNull.Value
                        ? null
                        : Convert.ToDouble(
                            reader["installment_percentage"]),

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
