using StudentManagement.Application.Interfaces.Repositories.Payment;
using StudentManagement.Domain.Entities.Payment;
using StudentManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Infrastructure.Repositories.Payment
    {
        public class StudentPaymentRepository
            : IStudentPaymentRepository
        {
            private readonly AppDbContext _context;

            public StudentPaymentRepository(
                AppDbContext context)
            {
                _context = context;
            }

            // =====================================================
            // GET BY ID
            // =====================================================
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

            // =====================================================
            // GET ALL
            // =====================================================
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

            // =====================================================
            // GET PAYMENT HISTORY BY REGISTRATION ID
            // =====================================================
            public async Task<IEnumerable<StudentPayment>>
                GetByRegistrationIdAsync(
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
                    new List<StudentPayment>();

                while (await reader.ReadAsync())
                {
                    payments.Add(
                        MapPayment(reader));
                }

                return payments;
            }

            // =====================================================
            // INSERT
            // =====================================================
            public async Task<StudentPayment> AddAsync(
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

                if (result == null ||
                    result == DBNull.Value)
                {
                    throw new Exception(
                        "Payment was created but payment ID was not returned.");
                }

                var paymentId =
                    Convert.ToInt32(result);

                var createdPayment =
                    await GetByIdAsync(paymentId);

                if (createdPayment == null)
                {
                    throw new Exception(
                        "Payment was created but could not be retrieved.");
                }

                return createdPayment;
            }

            // =====================================================
            // UPDATE
            // =====================================================
            public async Task UpdateAsync(
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
                    "Update");

                AddParameter(
                    command,
                    "@payment_id",
                    payment.PaymentId);

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

                await command.ExecuteNonQueryAsync();
            }

            // =====================================================
            // DELETE
            // =====================================================
            public async Task DeleteAsync(
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
                    "Delete");

                AddParameter(
                    command,
                    "@payment_id",
                    paymentId);

                await command.ExecuteNonQueryAsync();
            }

            // =====================================================
            // RESTORE
            // =====================================================
            public async Task RestoreAsync(
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
                    "Restore");

                AddParameter(
                    command,
                    "@payment_id",
                    paymentId);

                await command.ExecuteNonQueryAsync();
            }

            // =====================================================
            // MAP DATABASE RESULT → ENTITY
            // =====================================================
            private static StudentPayment MapPayment(
                DbDataReader reader)
            {
                return new StudentPayment
                {
                    PaymentId =
                        GetInt(reader, "payment_id"),

                    RegistrationId =
                        GetNullableInt(
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
                        GetNullableDouble(
                            reader,
                            "course_fee"),

                    TotalPaid =
                        GetNullableDouble(
                            reader,
                            "total_paid"),

                    RemainingAmount =
                        GetNullableDouble(
                            reader,
                            "remaining_amount"),

                    PaymentAmount =
                        GetNullableDouble(
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

            // =====================================================
            // HELPER METHODS
            // =====================================================
            private static int GetInt(
                DbDataReader reader,
                string columnName)
            {
                return Convert.ToInt32(
                    reader[columnName]);
            }

            private static int? GetNullableInt(
                DbDataReader reader,
                string columnName)
            {
                if (reader[columnName] == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(
                    reader[columnName]);
            }

            private static double? GetNullableDouble(
                DbDataReader reader,
                string columnName)
            {
                if (reader[columnName] == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDouble(
                    reader[columnName]);
            }

            private static string? GetString(
                DbDataReader reader,
                string columnName)
            {
                if (reader[columnName] == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToString(
                    reader[columnName]);
            }

            private static DateTime? GetNullableDateTime(
                DbDataReader reader,
                string columnName)
            {
                if (reader[columnName] == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDateTime(
                    reader[columnName]);
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


    