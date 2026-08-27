using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories.ForgotPassword;
using StudentManagement.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace StudentManagement.Infrastructure.Repositories.ForgotPassword
{
    public class ForgotPasswordRepository
        : IForgotPasswordRepository
    {
        private readonly AppDbContext _context;

        public ForgotPasswordRepository(
            AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // CHECK EMAIL
        // =====================================================

        public async Task<bool> StudentExistsByEmailAsync(
            string emailAddress)
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
                "CheckEmail");

            AddParameter(
                command,
                "@email_address",
                emailAddress.Trim());

            var result =
                await command.ExecuteScalarAsync();

            if (result == null ||
                result == DBNull.Value)
            {
                return false;
            }

            return Convert.ToInt32(result) > 0;
        }

        // =====================================================
        // RESET PASSWORD
        // =====================================================

        public async Task<bool> ResetPasswordAsync(
            string emailAddress,
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
                "ResetPassword");

            AddParameter(
                command,
                "@email_address",
                emailAddress.Trim());

            AddParameter(
                command,
                "@password",
                passwordHash);

            var result =
                await command.ExecuteScalarAsync();

            if (result == null ||
                result == DBNull.Value)
            {
                return false;
            }

            return Convert.ToInt32(result) == 1;
        }

        // =====================================================
        // ADD PARAMETER
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