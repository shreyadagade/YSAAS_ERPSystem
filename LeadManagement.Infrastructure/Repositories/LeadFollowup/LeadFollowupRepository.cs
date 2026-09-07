using Dapper;
using LeadManagement.Application.Interfaces.Repositories.LeadFollowup;
using LeadManagement.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LeadManagement.Infrastructure.Repositories
{
    public class LeadFollowupRepository : ILeadFollowupRepository
    {
        private readonly string _connectionString;

        public LeadFollowupRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> CreateAsync(TblLeadFollowup entity)
        {
            using var connection =
                new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Insert");
            parameters.Add("@lead_id", entity.LeadId);
            parameters.Add("@follow_up_date", entity.FollowUpDate);
            parameters.Add("@follow_up_by", entity.FollowUpBy);
            parameters.Add("@description", entity.Description);
            parameters.Add("@status", entity.Status);
            parameters.Add("@next_followup_date", entity.NextFollowupDate);

            var result =
                await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "erpsystem.sp_tbllead_followups",
                    parameters,
                    commandType: CommandType.StoredProcedure);

            return Convert.ToInt32(result?.followup_id ?? 0);
        }

        public async Task<bool> UpdateAsync(TblLeadFollowup entity)
        {
            using var connection =
                new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Update");
            parameters.Add("@followup_id", entity.LeadFollowupId);
            parameters.Add("@lead_id", entity.LeadId);
            parameters.Add("@follow_up_date", entity.FollowUpDate);
            parameters.Add("@follow_up_by", entity.FollowUpBy);
            parameters.Add("@description", entity.Description);
            parameters.Add("@status", entity.Status);
            parameters.Add("@next_followup_date", entity.NextFollowupDate);

            await connection.ExecuteAsync(
                "erpsystem.sp_tbllead_followups",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<bool> DeleteAsync(int followupId)
        {
            using var connection =
                new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Delete");
            parameters.Add("@followup_id", followupId);

            await connection.ExecuteAsync(
                "erpsystem.sp_tbllead_followups",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<bool> RestoreAsync(int followupId)
        {
            using var connection =
                new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Restore");
            parameters.Add("@followup_id", followupId);

            var result =
                await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "erpsystem.sp_tbllead_followups",
                    parameters,
                    commandType: CommandType.StoredProcedure);

            if (result == null)
                return false;

            return Convert.ToInt32(result.Success) == 1;
        }

        public async Task<TblLeadFollowup?> GetByIdAsync(int followupId)
        {
            using var connection =
                new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "GetById");
            parameters.Add("@followup_id", followupId);

            return await connection.QueryFirstOrDefaultAsync<TblLeadFollowup>(
                "erpsystem.sp_tbllead_followups",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TblLeadFollowup>> GetAllAsync()
        {
            using var connection =
                new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "GetAll");

            return await connection.QueryAsync<TblLeadFollowup>(
                "erpsystem.sp_tbllead_followups",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}