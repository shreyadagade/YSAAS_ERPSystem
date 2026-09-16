using Dapper;
using LeadManagement.Application.Interfaces.Repositories.LeadSource;
using LeadManagement.Domain.Entities.LeadSource;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LeadManagement.Infrastructure.Repositories.LeadSource
{
    public class LeadSourceRepository : ILeadSourceRepository
    {
        private readonly IConfiguration _configuration;

        public LeadSourceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<int> InsertAsync(TblLeadSource source)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Insert");
            parameters.Add("@source_name", source.SourceName);

            return await connection.QuerySingleAsync<int>(
                "erpsystem.sp_tbllead_sources",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<bool> UpdateAsync(TblLeadSource source)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Update");
            parameters.Add("@source_id", source.SourceId);
            parameters.Add("@source_name", source.SourceName);

            return await connection.QuerySingleAsync<bool>(
                "erpsystem.sp_tbllead_sources",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int sourceId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Delete");
            parameters.Add("@source_id", sourceId);

            return await connection.QuerySingleAsync<bool>(
                "erpsystem.sp_tbllead_sources",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // RESTORE
        // =====================================================

        public async Task<bool> RestoreAsync(int sourceId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Restore");
            parameters.Add("@source_id", sourceId);

            return await connection.QuerySingleAsync<bool>(
                "erpsystem.sp_tbllead_sources",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<TblLeadSource?> GetByIdAsync(int sourceId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "GetById");
            parameters.Add("@source_id", sourceId);

            return await connection.QuerySingleOrDefaultAsync<TblLeadSource>(
                "erpsystem.sp_tbllead_sources",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<TblLeadSource>> GetAllAsync()
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "GetAll");

            return await connection.QueryAsync<TblLeadSource>(
                "erpsystem.sp_tbllead_sources",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // CHECK DUPLICATE SOURCE NAME
        // =====================================================

        public async Task<bool> SourceNameExistsAsync(
            string sourceName,
            int? sourceId = null)
        {
            using var connection = CreateConnection();

            const string sql = @"
                SELECT COUNT(1)
                FROM erpsystem.tbllead_sources
                WHERE LOWER(LTRIM(RTRIM(source_name))) =
                      LOWER(LTRIM(RTRIM(@SourceName)))
                  AND ISNULL(flag, 0) = 1
                  AND (@SourceId IS NULL OR source_id <> @SourceId);";

            var count = await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    SourceName = sourceName,
                    SourceId = sourceId
                });

            return count > 0;
        }
    }
}