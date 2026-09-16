using Dapper;
using LeadManagement.Application.Interfaces.Repositories.Qualification;
using LeadManagement.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LeadManagement.Infrastructure.Repositories
{
    public class QualificationRepository : IQualificationRepository
    {
        private readonly IConfiguration _configuration;

        public QualificationRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> InsertAsync(
            TblQualification qualification)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Insert");
            parameters.Add(
                "@qualification",
                qualification.Qualification);

            await connection.ExecuteAsync(
                "erpsystem.sp_tblqualifications",
                parameters,
                commandType: CommandType.StoredProcedure);

            // Existing SP does not return the generated ID.
            // Get the latest active qualification ID.
            var qualificationId = await connection.QuerySingleAsync<int>(
                @"SELECT TOP 1 qualification_id
                  FROM erpsystem.tblqualifications
                  WHERE qualification = @qualification
                    AND flag = 0
                  ORDER BY qualification_id DESC",
                new
                {
                    qualification = qualification.Qualification
                });

            return qualificationId;
        }

        public async Task<bool> UpdateAsync(
            TblQualification qualification)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Update");
            parameters.Add(
                "@qualification_id",
                qualification.QualificationId);
            parameters.Add(
                "@qualification",
                qualification.Qualification);

            await connection.ExecuteAsync(
                "erpsystem.sp_tblqualifications",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<bool> DeleteAsync(int qualificationId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Delete");
            parameters.Add("@qualification_id", qualificationId);

            await connection.ExecuteAsync(
                "erpsystem.sp_tblqualifications",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<bool> RestoreAsync(int qualificationId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Restore");
            parameters.Add("@qualification_id", qualificationId);

            await connection.ExecuteAsync(
                "erpsystem.sp_tblqualifications",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<TblQualification?> GetByIdAsync(
            int qualificationId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "GetById");
            parameters.Add("@qualification_id", qualificationId);

            return await connection.QueryFirstOrDefaultAsync<TblQualification>(
                "erpsystem.sp_tblqualifications",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TblQualification>> GetAllAsync()
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "GetAll");

            return await connection.QueryAsync<TblQualification>(
                "erpsystem.sp_tblqualifications",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}