using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.DTOs.Payment;
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


        // =========================================================
        // GET PAYMENT BY ID
        // =========================================================
        public async Task<StudentPayment?> GetByIdAsync(
            int paymentId)
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
                "erpsystem.sp_tblstudent_payments";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetById");

            AddParameter(
                command,
                "@payment_id",
                paymentId);

            using var reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapPayment(reader);
        }


        // =========================================================
        // GET ALL PAYMENTS
        // =========================================================
        public async Task<IEnumerable<StudentPayment>>
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
                "erpsystem.sp_tblstudent_payments";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetAll");

            using var reader =
                await command.ExecuteReaderAsync();

            var payments =
                new List<StudentPayment>();

            while (await reader.ReadAsync())
            {
                payments.Add(
                    MapPayment(reader));
            }

            return payments;
        }


        // =========================================================
        // GET PAYMENT DETAILS BY PAYMENT ID
        // =========================================================
        public async Task<StudentPaymentResponseDto?>
            GetPaymentDetailsByIdAsync(
                int paymentId)
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
                "erpsystem.sp_tblstudent_payments";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetById");

            AddParameter(
                command,
                "@payment_id",
                paymentId);

            using var reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapPaymentResponse(reader);
        }


        // =========================================================
        // GET ALL PAYMENT DETAILS
        // =========================================================
        public async Task<IEnumerable<StudentPaymentResponseDto>>
            GetAllPaymentDetailsAsync()
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
                "erpsystem.sp_tblstudent_payments";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetAll");

            using var reader =
                await command.ExecuteReaderAsync();

            var payments =
                new List<StudentPaymentResponseDto>();

            while (await reader.ReadAsync())
            {
                payments.Add(
                    MapPaymentResponse(reader));
            }

            return payments;
        }


        // =========================================================
        // GET PAYMENT HISTORY BY REGISTRATION ID
        // =========================================================
        public async Task<IEnumerable<StudentPaymentResponseDto>>
            GetPaymentHistoryByRegistrationIdAsync(
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
                "erpsystem.sp_tblstudent_payments";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetByRegistrationId");

            AddParameter(
                command,
                "@registration_id",
                registrationId);

            using var reader =
                await command.ExecuteReaderAsync();

            var payments =
                new List<StudentPaymentResponseDto>();

            while (await reader.ReadAsync())
            {
                payments.Add(
                    MapPaymentResponse(reader));
            }

            return payments;
        }


        // =========================================================
        // GET TOTAL PAID
        // =========================================================
        public async Task<decimal>
            GetTotalPaidAsync(
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

            command.CommandText = @"
                SELECT ISNULL(SUM(payment_amount), 0)
                FROM erpsystem.tblstudent_payments
                WHERE registration_id = @registration_id
                  AND flag = 0";

            command.CommandType =
                CommandType.Text;

            AddParameter(
                command,
                "@registration_id",
                registrationId);

            var result =
                await command.ExecuteScalarAsync();

            return Convert.ToDecimal(result);
        }


        // =========================================================
        // INSERT
        // =========================================================
        public async Task<StudentPayment>
            AddAsync(
                StudentPayment payment)
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
                "erpsystem.sp_tblstudent_payments";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "Insert");

            AddParameter(
                command,
                "@registration_id",
                payment.RegistrationId);

            AddParameter(
                command,
                "@payment_date",
                payment.PaymentDate);

            AddParameter(
                command,
                "@payment_amount",
                payment.PaymentAmount);

            AddParameter(
                command,
                "@payment_mode",
                payment.PaymentMode);

            AddParameter(
                command,
                "@payment_description",
                payment.PaymentDescription);

            AddParameter(
                command,
                "@is_paid",
                payment.IsPaid);

            var result =
                await command.ExecuteScalarAsync();

            if (result != null &&
                result != DBNull.Value)
            {
                payment.PaymentId =
                    Convert.ToInt32(result);
            }

            return payment;
        }


        // =========================================================
        // UPDATE
        // =========================================================
        public async Task UpdateAsync(
            StudentPayment payment)
        {
            await _context.Database
                .ExecuteSqlRawAsync(
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


        // =========================================================
        // DELETE
        // =========================================================
        public async Task DeleteAsync(
            int paymentId)
        {
            await _context.Database
                .ExecuteSqlRawAsync(
                    @"EXEC erpsystem.sp_tblstudent_payments
                      @Type = 'Delete',
                      @payment_id = {0}",
                    paymentId);
        }


        // =========================================================
        // RESTORE
        // =========================================================
        public async Task RestoreAsync(
            int paymentId)
        {
            await _context.Database
                .ExecuteSqlRawAsync(
                    @"EXEC erpsystem.sp_tblstudent_payments
                      @Type = 'Restore',
                      @payment_id = {0}",
                    paymentId);
        }


        // =========================================================
        // MAP ENTITY
        // =========================================================
        private static StudentPayment
            MapPayment(
                DbDataReader reader)
        {
            return new StudentPayment
            {
                PaymentId =
                    GetInt(
                        reader,
                        "payment_id"),

                RegistrationId =
                    GetNullableInt(
                        reader,
                        "registration_id"),

                PaymentDate =
                    GetNullableDateTime(
                        reader,
                        "payment_date"),

                PaymentAmount =
                    GetNullableDouble(
                        reader,
                        "payment_amount"),

                PaymentMode =
                    GetString(
                        reader,
                        "payment_mode"),

                PaymentDescription =
                    GetString(
                        reader,
                        "payment_description"),

                IsPaid =
                    GetNullableInt(
                        reader,
                        "is_paid")
            };
        }


        // =========================================================
        // MAP RESPONSE DTO
        // =========================================================
        private static StudentPaymentResponseDto
            MapPaymentResponse(
                DbDataReader reader)
        {
            return new StudentPaymentResponseDto
            {
                PaymentId =
                    GetInt(
                        reader,
                        "payment_id"),

                RegistrationId =
                    GetInt(
                        reader,
                        "registration_id"),

                StudentName =
                    GetString(
                        reader,
                        "student_name"),

                CourseName =
                    GetString(
                        reader,
                        "course_name"),

                CourseFee =
                    GetDecimal(
                        reader,
                        "course_fee"),

                TotalPaid =
                    GetDecimal(
                        reader,
                        "total_paid"),

                RemainingAmount =
                    GetDecimal(
                        reader,
                        "remaining_amount"),

                PaymentAmount =
                    GetDecimal(
                        reader,
                        "payment_amount"),

                PaymentMode =
                    GetString(
                        reader,
                        "payment_mode"),

                PaymentDate =
                    GetNullableDateTime(
                        reader,
                        "payment_date"),

                IsPaid =
                    GetInt(
                        reader,
                        "is_paid")
            };
        }


        // =========================================================
        // GET STRING
        // =========================================================
        private static string? GetString(
            DbDataReader reader,
            string columnName)
        {
            var value =
                reader[columnName];

            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToString(value);
        }


        // =========================================================
        // GET INT
        // =========================================================
        private static int GetInt(
            DbDataReader reader,
            string columnName)
        {
            var value =
                reader[columnName];

            if (value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(value);
        }


        // =========================================================
        // GET NULLABLE INT
        // =========================================================
        private static int? GetNullableInt(
            DbDataReader reader,
            string columnName)
        {
            var value =
                reader[columnName];

            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(value);
        }


        // =========================================================
        // GET DECIMAL
        // =========================================================
        private static decimal GetDecimal(
            DbDataReader reader,
            string columnName)
        {
            var value =
                reader[columnName];

            if (value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToDecimal(value);
        }


        // =========================================================
        // GET NULLABLE DOUBLE
        // =========================================================
        private static double? GetNullableDouble(
            DbDataReader reader,
            string columnName)
        {
            var value =
                reader[columnName];

            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDouble(value);
        }


        // =========================================================
        // GET NULLABLE DATETIME
        // =========================================================
        private static DateTime? GetNullableDateTime(
            DbDataReader reader,
            string columnName)
        {
            var value =
                reader[columnName];

            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(value);
        }


        // =========================================================
        // ADD PARAMETER
        // =========================================================
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