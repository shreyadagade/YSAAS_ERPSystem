using System.Data;
using Dapper;
using LeadManagement.Application.DTOs.Lead;
using LeadManagement.Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LeadManagement.Infrastructure.Repositories
{
    public class LeadImportRepository : ILeadImportRepository
    {
        private readonly string _connectionString;

        public LeadImportRepository(
            IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");
        }

        public async Task<Dictionary<string, int>>
            GetActiveSourcesAsync()
        {
            const string sql = @"
                SELECT
                    source_id,
                    source_name
                FROM erpsystem.tbllead_sources
                WHERE ISNULL(flag, 0) = 1
                  AND DeletedAt IS NULL
                  AND source_name IS NOT NULL;
            ";

            await using var connection =
                new SqlConnection(_connectionString);

            var rows =
                await connection.QueryAsync<(int SourceId, string SourceName)>(
                    sql);

            return rows
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.SourceName))
                .GroupBy(
                    x => x.SourceName.Trim(),
                    StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    x => x.Key,
                    x => x.First().SourceId,
                    StringComparer.OrdinalIgnoreCase);
        }

        public async Task<HashSet<string>>
            GetExistingEmailsAsync(
                IEnumerable<string> emails)
        {
            var emailList =
                emails
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .ToList();

            if (!emailList.Any())
            {
                return new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);
            }

            const string sql = @"
                SELECT email_address
                FROM erpsystem.tblleads
                WHERE DeletedAt IS NULL
                  AND email_address IS NOT NULL
                  AND email_address IN @Emails;
            ";

            await using var connection =
                new SqlConnection(_connectionString);

            var result =
                await connection.QueryAsync<string>(
                    sql,
                    new
                    {
                        Emails = emailList
                    });

            return result
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToHashSet(
                    StringComparer.OrdinalIgnoreCase);
        }

        public async Task<HashSet<string>>
            GetExistingMobilesAsync(
                IEnumerable<string> mobiles)
        {
            var mobileList =
                mobiles
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

            if (!mobileList.Any())
            {
                return new HashSet<string>();
            }

            const string sql = @"
                SELECT mobile_number
                FROM erpsystem.tblleads
                WHERE DeletedAt IS NULL
                  AND mobile_number IS NOT NULL
                  AND mobile_number IN @Mobiles;
            ";

            await using var connection =
                new SqlConnection(_connectionString);

            var result =
                await connection.QueryAsync<string>(
                    sql,
                    new
                    {
                        Mobiles = mobileList
                    });

            return result
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToHashSet();
        }

        public async Task BulkInsertAsync(
            IEnumerable<LeadExcelDto> leads,
            Dictionary<string, int> sourceMap)
        {
            var leadList =
                leads.ToList();

            if (!leadList.Any())
                return;

            var table =
                new DataTable();

            table.Columns.Add(
                "candidate_name",
                typeof(string));

            table.Columns.Add(
                "email_address",
                typeof(string));

            table.Columns.Add(
                "mobile_number",
                typeof(string));

            table.Columns.Add(
                "status",
                typeof(string));

            table.Columns.Add(
                "source_id",
                typeof(int));

            table.Columns.Add(
                "CreatedAt",
                typeof(DateTime));

            foreach (var lead in leadList)
            {
                var row =
                    table.NewRow();

                row["candidate_name"] =
                    (object?)lead.CandidateName
                    ?? DBNull.Value;

                row["email_address"] =
                    (object?)lead.EmailAddress
                    ?? DBNull.Value;

                row["mobile_number"] =
                    (object?)lead.MobileNumber
                    ?? DBNull.Value;

                row["status"] =
                    (object?)lead.Status
                    ?? DBNull.Value;

                row["source_id"] =
                    sourceMap[
                        lead.SourceName!.Trim()];

                row["CreatedAt"] =
                    DateTime.Now;

                table.Rows.Add(row);
            }

            await using var connection =
                new SqlConnection(_connectionString);

            await connection.OpenAsync();

            await using var transaction =
                await connection.BeginTransactionAsync();

            try
            {
                using var bulkCopy =
                    new SqlBulkCopy(
                        connection,
                        SqlBulkCopyOptions.CheckConstraints,
                        (SqlTransaction)transaction);

                bulkCopy.DestinationTableName =
                    "erpsystem.tblleads";

                bulkCopy.BatchSize = 1000;

                bulkCopy.BulkCopyTimeout = 300;

                bulkCopy.ColumnMappings.Add(
                    "candidate_name",
                    "candidate_name");

                bulkCopy.ColumnMappings.Add(
                    "email_address",
                    "email_address");

                bulkCopy.ColumnMappings.Add(
                    "mobile_number",
                    "mobile_number");

                bulkCopy.ColumnMappings.Add(
                    "status",
                    "status");

                bulkCopy.ColumnMappings.Add(
                    "source_id",
                    "source_id");

                bulkCopy.ColumnMappings.Add(
                    "CreatedAt",
                    "CreatedAt");

                await bulkCopy.WriteToServerAsync(
                    table);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}