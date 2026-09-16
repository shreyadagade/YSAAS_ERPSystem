
using Dapper;
using LeadManagement.Application.Interfaces.Repositories.EnquiryFollowup;
using LeadManagement.Domain.Entities.EnquiryFollowup;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace LeadManagement.Infrastructure.Repositories.EnquiryFollowup
{
    public class EnquiryFollowupRepository : IEnquiryFollowupRepository
    {
        private readonly IConfiguration _configuration;

        public EnquiryFollowupRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        // =====================================================
        // INSERT
        // =====================================================

        public async Task<int> InsertAsync(TblEnquiryFollowup followup)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "INSERT");
            parameters.Add("@enquiry_id", followup.EnquiryId);
            parameters.Add("@source_id", followup.SourceId);
            parameters.Add("@follow_up_date", followup.FollowUpDate);
            parameters.Add("@follow_up_by", followup.FollowUpBy);
            parameters.Add("@description", followup.Description);
            parameters.Add("@status", followup.Status);
            parameters.Add("@next_followup_date", followup.NextFollowupDate);

            var result = await connection.QuerySingleAsync<dynamic>(
                "erpsystem.sp_tblenquiry_followups",
                parameters,
                commandType: CommandType.StoredProcedure);

            return (int)result.followup_id;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<bool> UpdateAsync(TblEnquiryFollowup followup)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "UPDATE");
            parameters.Add("@followup_id", followup.FollowupId);
            parameters.Add("@enquiry_id", followup.EnquiryId);
            parameters.Add("@source_id", followup.SourceId);
            parameters.Add("@follow_up_date", followup.FollowUpDate);
            parameters.Add("@follow_up_by", followup.FollowUpBy);
            parameters.Add("@description", followup.Description);
            parameters.Add("@status", followup.Status);
            parameters.Add("@next_followup_date", followup.NextFollowupDate);

            await connection.QueryFirstOrDefaultAsync(
                "erpsystem.sp_tblenquiry_followups",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int followupId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "DELETE");
            parameters.Add("@followup_id", followupId);

            await connection.QueryFirstOrDefaultAsync(
                "erpsystem.sp_tblenquiry_followups",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        // =====================================================
        // RESTORE
        // =====================================================

        public async Task<bool> RestoreAsync(int followupId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "RESTORE");
            parameters.Add("@followup_id", followupId);

            await connection.QueryFirstOrDefaultAsync(
                "erpsystem.sp_tblenquiry_followups",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<TblEnquiryFollowup?> GetByIdAsync(
            int followupId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "GETBYID");
            parameters.Add("@followup_id", followupId);

            return await connection.QueryFirstOrDefaultAsync<TblEnquiryFollowup>(
                "erpsystem.sp_tblenquiry_followups",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<TblEnquiryFollowup>> GetAllAsync()
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "GETALL");

            return await connection.QueryAsync<TblEnquiryFollowup>(
                "erpsystem.sp_tblenquiry_followups",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // GET BY ENQUIRY ID
        // =====================================================

        public async Task<IEnumerable<TblEnquiryFollowup>> GetByEnquiryIdAsync(
            int enquiryId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "GETBYENQUIRYID");
            parameters.Add("@enquiry_id", enquiryId);

            return await connection.QueryAsync<TblEnquiryFollowup>(
                "erpsystem.sp_tblenquiry_followups",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}

