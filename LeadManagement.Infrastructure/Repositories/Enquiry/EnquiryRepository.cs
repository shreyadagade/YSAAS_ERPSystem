using Dapper;
using LeadManagement.Application.DTOs.Enquiry;
using LeadManagement.Application.Interfaces.Repositories.Enquiry;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LeadManagement.Infrastructure.Repositories.Enquiry
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly IConfiguration _configuration;

        public EnquiryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<int> CreateAsync(EnquiryDto enquiry)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "INSERT");
            parameters.Add("@enquiry_date", enquiry.EnquiryDate);
            parameters.Add("@candidate_name", enquiry.CandidateName);
            parameters.Add("@gender", enquiry.Gender);
            parameters.Add("@local_address", enquiry.LocalAddress);
            parameters.Add("@email_address", enquiry.EmailAddress);
            parameters.Add("@mobile_number", enquiry.MobileNumber);
            parameters.Add("@birth_date", enquiry.BirthDate);
            parameters.Add("@qualification", enquiry.Qualification);
            parameters.Add("@lead_sources", enquiry.LeadSources);
            parameters.Add("@enquiry_fors", enquiry.EnquiryFors);
            parameters.Add("@interested_topics", enquiry.InterestedTopics);
            parameters.Add("@status", enquiry.Status);
            parameters.Add("@branch_id", enquiry.BranchId);

            return await connection.QuerySingleAsync<int>(
                "erpsystem.sp_tblenquiries",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<bool> UpdateAsync(EnquiryDto enquiry)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "UPDATE");
            //parameters.Add("@enquiry_id", enquiry.EnquiryId);
            parameters.Add("@enquiry_date", enquiry.EnquiryDate);
            parameters.Add("@candidate_name", enquiry.CandidateName);
            parameters.Add("@gender", enquiry.Gender);
            parameters.Add("@local_address", enquiry.LocalAddress);
            parameters.Add("@email_address", enquiry.EmailAddress);
            parameters.Add("@mobile_number", enquiry.MobileNumber);
            parameters.Add("@birth_date", enquiry.BirthDate);
            parameters.Add("@qualification", enquiry.Qualification);
            parameters.Add("@lead_sources", enquiry.LeadSources);
            parameters.Add("@enquiry_fors", enquiry.EnquiryFors);
            parameters.Add("@interested_topics", enquiry.InterestedTopics);
            parameters.Add("@status", enquiry.Status);
            parameters.Add("@branch_id", enquiry.BranchId);

            await connection.ExecuteAsync(
                "erpsystem.sp_tblenquiries",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<EnquiryDto?> GetByIdAsync(int enquiryId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "GETBYID");
            parameters.Add("@enquiry_id", enquiryId);

            return await connection.QueryFirstOrDefaultAsync<EnquiryDto>(
                "erpsystem.sp_tblenquiries",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<EnquiryDto>> GetAllAsync()
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "GETALL");

            return await connection.QueryAsync<EnquiryDto>(
                "erpsystem.sp_tblenquiries",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int enquiryId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "DELETE");
            parameters.Add("@enquiry_id", enquiryId);

            await connection.ExecuteAsync(
                "erpsystem.sp_tblenquiries",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        // =====================================================
        // RESTORE
        // =====================================================

        public async Task<bool> RestoreAsync(int enquiryId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "RESTORE");
            parameters.Add("@enquiry_id", enquiryId);

            await connection.ExecuteAsync(
                "erpsystem.sp_tblenquiries",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        // =====================================================
        // GET CANDIDATES
        // =====================================================

        public async Task<IEnumerable<CandidateDropdownDto>> GetCandidatesAsync()
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Action", "GETCANDIDATES");

            return await connection.QueryAsync<CandidateDropdownDto>(
                "erpsystem.sp_tblenquiries",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}