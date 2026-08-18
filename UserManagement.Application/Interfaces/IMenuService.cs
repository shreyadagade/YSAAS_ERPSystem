using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.Menu;

namespace UserManagement.Application.Interfaces
{
    public interface IMenuService
    {
        Task<List<MenuResponseDto>> GetAllAsync();

        Task<MenuResponseDto?> GetByIdAsync(int id);

        Task<int> InsertAsync(CreateMenuDto dto);

        Task<int> UpdateAsync(int id, UpdateMenuDto dto);

        Task<int> DeleteAsync(int id);

        Task<int> RestoreAsync(int id);
    }
}
