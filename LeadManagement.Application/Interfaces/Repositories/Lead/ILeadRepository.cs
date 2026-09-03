using LeadManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.Interfaces.Repositories.Lead
{


    public interface ILeadRepository
    {
        Task<int> InsertAsync(TblLead lead);

        Task<bool> UpdateAsync(TblLead lead);

        Task<bool> DeleteAsync(int leadId);

        Task<bool> RestoreAsync(int leadId);

        Task<TblLead?> GetByIdAsync(int leadId);

        Task<IEnumerable<TblLead>> GetAllAsync();
        Task<bool> EmailExistsAsync(string email, int? leadId = null);
        Task<bool> MobileExistsAsync(string mobile, int? leadId = null);
    }
}

