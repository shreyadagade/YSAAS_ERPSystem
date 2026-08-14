using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Domain.Entities.Registration;
using StudentManagement.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace StudentManagement.Infrastructure.Repositories.Registration
{
    public class StudentPaymentRepository : IStudentPaymentRepository
    {
        private readonly AppDbContext _context;

        public StudentPaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<StudentPayment?> GetByIdAsync(int paymentId)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = "erpsystem.sp_tblstudent_payments";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@Type", "GetById");
            AddParameter(command, "@payment_id", paymentId);

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapPayment(reader);
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<StudentPayment>> GetAllAsync()
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = "erpsystem.sp_tblstudent_payments";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@Type", "GetAll");

            using var reader = await command.ExecuteReaderAsync();

            var payments = new List<StudentPayment>();

            while (await reader.ReadAsync())
            {
                payments.Add(MapPayment(reader));
            }

            return payments;
        }

        // =========================
        // INSERT
        // =========================
        public async Task<StudentPayment> AddAsync(StudentPayment payment)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_payments
                  @Type = 'Insert',
                  @registration_id = {0},
                  @payment_date = {1},
                  @payment_amount = {2},
                  @payment_mode = {3},
                  @payment_description = {4},
                  @is_paid = {5}",
                payment.RegistrationId,
                payment.PaymentDate,
                payment.PaymentAmount,
                payment.PaymentMode,
                payment.PaymentDescription,
                payment.IsPaid);

            return payment;
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(StudentPayment payment)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_payments
                  @Type = 'Update',
                  @payment_id = {0},
                  @registration_id = {1},
                  @payment_date = {2},
                  @payment_amount = {3},
                  @payment_mode = {4},
                  @payment_description = {5},
                  @is_paid = {6}",
                payment.PaymentId,
                payment.RegistrationId,
                payment.PaymentDate,
                payment.PaymentAmount,
                payment.PaymentMode,
                payment.PaymentDescription,
                payment.IsPaid);
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(int paymentId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_payments
                  @Type = 'Delete',
                  @payment_id = {0}",
                paymentId);
        }

        // =========================
        // RESTORE
        // =========================
        public async Task RestoreAsync(int paymentId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_payments
                  @Type = 'Restore',
                  @payment_id = {0}",
                paymentId);
        }

        // =========================
        // MAP DATABASE RESULT
        // =========================
        private static StudentPayment MapPayment(DbDataReader reader)
        {
            return new StudentPayment
            {
                PaymentId = GetInt(reader, "payment_id"),

                RegistrationId = GetNullableInt(
                    reader,
                    "registration_id"),

                PaymentDate = GetNullableDateTime(
                    reader,
                    "payment_date"),

                PaymentAmount = GetNullableDouble(
                    reader,
                    "payment_amount"),

                PaymentMode = GetString(
                    reader,
                    "payment_mode"),

                PaymentDescription = GetString(
                    reader,
                    "payment_description"),

                IsPaid = GetNullableInt(
                    reader,
                    "is_paid")
            };
        }

        // =========================
        // GET STRING
        // =========================
        private static string? GetString(
            DbDataReader reader,
            string columnName)
        {
            var value = reader[columnName];

            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToString(value);
        }

        // =========================
        // GET INT
        // =========================
        private static int GetInt(
            DbDataReader reader,
            string columnName)
        {
            var value = reader[columnName];

            if (value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(value);
        }

        // =========================
        // GET NULLABLE INT
        // =========================
        private static int? GetNullableInt(
            DbDataReader reader,
            string columnName)
        {
            var value = reader[columnName];

            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(value);
        }

        // =========================
        // GET NULLABLE DATE
        // =========================
        private static DateTime? GetNullableDateTime(
            DbDataReader reader,
            string columnName)
        {
            var value = reader[columnName];

            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(value);
        }

        // =========================
        // GET NULLABLE DOUBLE
        // =========================
        private static double? GetNullableDouble(
            DbDataReader reader,
            string columnName)
        {
            var value = reader[columnName];

            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDouble(value);
        }

        // =========================
        // ADD PARAMETER
        // =========================
        private static void AddParameter(
            DbCommand command,
            string parameterName,
            object? value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;

            command.Parameters.Add(parameter);
        }
    }
}