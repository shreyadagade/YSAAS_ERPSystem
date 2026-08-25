using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Contracts;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Persistence.Context;

namespace UserManagement.Infrastructure.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string storedProcedure,
            params StoredProcedureParameter[] parameters)
            where T : class
        {
            var sqlParameters = parameters.Select(p => new SqlParameter(p.Name,p.Value ?? DBNull.Value)).ToArray();

            var parameterNames = string.Join(", ", sqlParameters.Select(p => $"{p.ParameterName} = {p.ParameterName}"));

            return await _context.Set<T>().FromSqlRaw($"EXEC {storedProcedure} {parameterNames}",sqlParameters)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> ExecuteNonQueryAsync(string storedProcedure,
            params StoredProcedureParameter[] parameters)
        {
            var sqlParameters = parameters.Select(p => new SqlParameter(p.Name, p.Value ?? DBNull.Value)).ToArray();

            var parameterNames = string.Join(", ",sqlParameters.Select(p =>$"{p.ParameterName} = {p.ParameterName}"));

            return await _context.Database.ExecuteSqlRawAsync($"EXEC {storedProcedure} {parameterNames}",
                sqlParameters);
        }

        //public async Task<List<T>> ExecuteRawQueryAsync<T>(string sql,params StoredProcedureParameter[] parameters)
        //    where T : class
        //{
        //    var sqlParameters = parameters
        //        .Select(p => new SqlParameter(
        //            p.Name,
        //            p.Value ?? DBNull.Value))
        //        .ToArray();

        //    return await _context.Set<T>()
        //        .FromSqlRaw(sql, sqlParameters)
        //        .AsNoTracking()
        //        .ToListAsync();
        //}
    }
}