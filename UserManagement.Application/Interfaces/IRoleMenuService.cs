using UserManagement.Application.DTOs.RoleMenu;

namespace UserManagement.Application.Interfaces
{
    public interface IRoleMenuService
    {
        Task<List<RoleMenuResponseDto>> GetAllAsync();

        Task<RoleMenuResponseDto?> GetByIdAsync(int id);

        Task<int> InsertAsync(CreateRoleMenuDto dto);

        Task<int> UpdateAsync(int id, UpdateRoleMenuDto dto);

        Task<int> DeleteAsync(int id);

        Task<int> RestoreAsync(int id);

        Task<List<RoleMenuResponseDto>> GetMenusByRoleAsync(string roleId);
    }
}