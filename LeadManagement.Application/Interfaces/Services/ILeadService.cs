using LeadManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.Interfaces.Services
{


    public interface ILeadService
    {
        Task<int> CreateAsync(LeadDto lead);

        Task<bool> UpdateAsync(LeadDto lead);

        Task<bool> DeleteAsync(int leadId);

        Task<bool> RestoreAsync(int leadId);

        Task<LeadDto?> GetByIdAsync(int leadId);

        Task<IEnumerable<LeadDto>> GetAllAsync();
    }
}

