using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.Contracts;

namespace UserManagement.Application.Interfaces
{
    public interface IGenericRepository
    {
        Task<List<T>> ExecuteQueryAsync<T>(string storedProcedure,params StoredProcedureParameter[] parameters)
            where T : class;

        Task<int> ExecuteNonQueryAsync(string storedProcedure, params StoredProcedureParameter[] parameters);
    }
}
