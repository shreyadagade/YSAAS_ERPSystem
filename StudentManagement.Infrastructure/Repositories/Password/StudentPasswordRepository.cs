using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories.Password;
using StudentManagement.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace StudentManagement.Infrastructure.Repositories.Password
{
    public class StudentPasswordRepository
        : IStudentPasswordRepository
    {
        private readonly AppDbContext _context;

        public StudentPasswordRepository(
            AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET CURRENT PASSWORD HASH
        // =====================================================

        public async Task<string?>
            GetPasswordHashByStudentIdAsync(
                int studentId)
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
                "GetPassword");

            AddParameter(
                command,
                "@student_id",
                studentId);

            // =================================================
            // DEBUG INFORMATION
            // =================================================

            Console.WriteLine(
                "======================================");

            Console.WriteLine(
                $"StudentId received by repository: {studentId}");

            Console.WriteLine(
                $"Stored Procedure: {command.CommandText}");

            Console.WriteLine(
                "Type: GetPassword");

            // =================================================
            // EXECUTE STORED PROCEDURE
            // =================================================

            var result =
                await command.ExecuteScalarAsync();

            // =================================================
            // DEBUG DATABASE RESULT
            // =================================================

            Console.WriteLine(
                $"Database password result: [{result}]");

            Console.WriteLine(
                $"Result type: {result?.GetType().Name}");

            Console.WriteLine(
                "======================================");

            // =================================================
            // CHECK RESULT
            // =================================================

            if (result == null ||
                result == DBNull.Value)
            {
                return null;
            }

            return Convert.ToString(result);
        }

        // =====================================================
        // UPDATE PASSWORD
        // =====================================================

        public async Task<bool>
            UpdatePasswordAsync(
                int studentId,
                string passwordHash)
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
                "ChangePassword");

            AddParameter(
                command,
                "@student_id",
                studentId);

            AddParameter(
                command,
                "@password",
                passwordHash);

            var rowsAffected =
                await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
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