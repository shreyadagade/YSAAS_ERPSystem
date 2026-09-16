using Dapper;
using LeadManagement.Application.Interfaces.Repositories.Lead;
using LeadManagement.Domain.Entities.Lead;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LeadManagement.Infrastructure.Repositories.Lead
{
    public class LeadRepository : ILeadRepository
    {
        private readonly IConfiguration _configuration;

        public LeadRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> InsertAsync(TblLead lead)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "INSERT");
            parameters.Add("@candidate_name", lead.CandidateName);
            parameters.Add("@email_address", lead.EmailAddress);
            parameters.Add("@mobile_number", lead.MobileNumber);
            parameters.Add("@training_type", lead.TrainingType);
            parameters.Add("@description", lead.Description);
            parameters.Add("@status", lead.Status);
            parameters.Add("@lead_date", lead.LeadDate);
            parameters.Add("@source_id", lead.SourceId);

            var result = await connection.QuerySingleAsync<dynamic>(
                "erpsystem.sp_tblleads",
                parameters,
                commandType: CommandType.StoredProcedure);

            return (int)result.lead_id;
        }

        public async Task<bool> UpdateAsync(TblLead lead)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "UPDATE");
            parameters.Add("@lead_id", lead.LeadId);
            parameters.Add("@candidate_name", lead.CandidateName);
            parameters.Add("@email_address", lead.EmailAddress);
            parameters.Add("@mobile_number", lead.MobileNumber);
            parameters.Add("@training_type", lead.TrainingType);
            parameters.Add("@description", lead.Description);
            parameters.Add("@status", lead.Status);
            parameters.Add("@lead_date", lead.LeadDate);
            parameters.Add("@source_id", lead.SourceId);

            return await connection.QuerySingleAsync<bool>(
                "erpsystem.sp_tblleads",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> DeleteAsync(int leadId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "DELETE");
            parameters.Add("@lead_id", leadId);

            return await connection.QuerySingleAsync<bool>(
                "erpsystem.sp_tblleads",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> RestoreAsync(int leadId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "RESTORE");
            parameters.Add("@lead_id", leadId);

            return await connection.QuerySingleAsync<bool>(
                "erpsystem.sp_tblleads",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<TblLead?> GetByIdAsync(int leadId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "GETBYID");
            parameters.Add("@lead_id", leadId);

            return await connection.QueryFirstOrDefaultAsync<TblLead>(
                "erpsystem.sp_tblleads",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TblLead>> GetAllAsync()
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "GETALL");

            return await connection.QueryAsync<TblLead>(
                "erpsystem.sp_tblleads",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TblLead>> GetBySourceIdAsync(int sourceId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "GETBYSOURCEID");
            parameters.Add("@source_id", sourceId);

            return await connection.QueryAsync<TblLead>(
                "erpsystem.sp_tblleads",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> EmailExistsAsync(
            string email,
            int? leadId = null)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "CHECKEMAIL");
            parameters.Add("@email_address", email);
            parameters.Add("@lead_id", leadId);

            return await connection.QuerySingleAsync<bool>(
                "erpsystem.sp_tblleads",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> MobileExistsAsync(
            string mobile,
            int? leadId = null)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "CHECKMOBILE");
            parameters.Add("@mobile_number", mobile);
            parameters.Add("@lead_id", leadId);

            return await connection.QuerySingleAsync<bool>(
                "erpsystem.sp_tblleads",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}