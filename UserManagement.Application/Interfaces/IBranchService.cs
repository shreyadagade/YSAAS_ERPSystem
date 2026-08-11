using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.Branch;

namespace UserManagement.Application.Interfaces
{
    public interface IBranchService
    {
        Task<List<BranchResponseDto>> GetAllAsync();

        Task<BranchResponseDto?> GetByIdAsync(int id);

        Task<int> InsertAsync(CreateBranchDto dto);

        Task<int> UpdateAsync(UpdateBranchDto dto);

        Task<int> DeleteAsync(int id);

        Task<int> RestoreAsync(int id);
    }
}
