using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DeveloperManagement.Application.Contracts;
using DeveloperManagement.Application.Interfaces;
using DeveloperManagement.Infrastructure.Persistence.Context;

namespace DeveloperManagement.Infrastructure.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly DeveloperDbContext _context;

        public GenericRepository(DeveloperDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string storedProcedure,
            params StoredProcedureParameter[] parameters) where T : class
        {
            var sqlParameters = parameters.Select(p => new SqlParameter(p.Name,
                    p.Value ?? DBNull.Value)).ToArray();

            var parameterNames = string.Join(", ", sqlParameters.Select(
                    p => $"{p.ParameterName} = {p.ParameterName}"));

            return await _context.Set<T>().FromSqlRaw($"EXEC {storedProcedure} {parameterNames}",
                    sqlParameters).AsNoTracking().ToListAsync();
        }

        public async Task<int> ExecuteNonQueryAsync(string storedProcedure,
            params StoredProcedureParameter[] parameters)
        {
            var sqlParameters = parameters.Select(p => new SqlParameter(p.Name,
                    p.Value ?? DBNull.Value)).ToArray();

            var parameterNames = string.Join(", ", sqlParameters.Select(
                    p => $"{p.ParameterName} = {p.ParameterName}"));

            return await _context.Database.ExecuteSqlRawAsync($"EXEC {storedProcedure} {parameterNames}",
                sqlParameters);
        }
    }
}
