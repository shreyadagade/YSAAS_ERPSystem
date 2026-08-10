using Dapper;
using LeadManagement.Application.Interfaces.Repositories;
using LeadManagement.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LeadManagement.Infrastructure.Repositories
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

        public async Task<int> InsertAsync(TblEnquiryFollowup followup)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "INSERT");
            parameters.Add("@enquiry_id", followup.EnquiryId);
            parameters.Add("@follow_up_date", followup.FollowUpDate);
            parameters.Add("@follow_up_by", followup.FollowUpBy);
            parameters.Add("@description", followup.Description);

            var result = await connection.QuerySingleAsync<dynamic>(
                "erpsystem.sp_tblenquiry_followups",
                parameters,
                commandType: CommandType.StoredProcedure);

            return (int)result.followup_id;
        }

        public async Task<bool> UpdateAsync(TblEnquiryFollowup followup)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "UPDATE");
            parameters.Add("@followup_id", followup.FollowupId);
            parameters.Add("@enquiry_id", followup.EnquiryId);
            parameters.Add("@follow_up_date", followup.FollowUpDate);
            parameters.Add("@follow_up_by", followup.FollowUpBy);
            parameters.Add("@description", followup.Description);

            await connection.QueryFirstOrDefaultAsync(
                "erpsystem.sp_tblenquiry_followups",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

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

        public async Task<TblEnquiryFollowup?> GetByIdAsync(int followupId)
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
    }
}

