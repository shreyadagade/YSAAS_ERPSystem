using UserManagement.Application.Contracts;
using UserManagement.Application.DTOs.RoleMenu;
using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Services
{
    public class RoleMenuService : IRoleMenuService
    {
        private readonly IGenericRepository _repository;
        private const string StoredProcedure = "erpsystem.sp_tblRoleMenus";

        public RoleMenuService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoleMenuResponseDto>> GetAllAsync()
        {
            return await _repository.ExecuteQueryAsync<RoleMenuResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });
        }

        public async Task<RoleMenuResponseDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Role menu ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<RoleMenuResponseDto>(
                    StoredProcedure,

                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetById"
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@role_menu_id",
                        Value = id
                    });

            return result.FirstOrDefault();
        }

        public async Task<int> InsertAsync(CreateRoleMenuDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException(
                    "Role menu data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.RoleId))
            {
                throw new ArgumentException(
                    "Role ID is required.");
            }

            if (dto.MenuId <= 0)
            {
                throw new ArgumentException(
                    "Menu ID must be greater than 0.");
            }

            return await _repository.ExecuteNonQueryAsync(
                StoredProcedure,

                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Insert"
                },

                new StoredProcedureParameter
                {
                    Name = "@role_id",
                    Value = dto.RoleId
                },

                new StoredProcedureParameter
                {
                    Name = "@menu_id",
                    Value = dto.MenuId
                });
        }

        public async Task<int> UpdateAsync(int id,UpdateRoleMenuDto dto)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Role menu ID must be greater than 0.");
            }

            if (dto == null)
            {
                throw new ArgumentException(
                    "Role menu data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.RoleId))
            {
                throw new ArgumentException(
                    "Role ID is required.");
            }

            if (dto.MenuId <= 0)
            {
                throw new ArgumentException(
                    "Menu ID must be greater than 0.");
            }

            return await _repository.ExecuteNonQueryAsync(
                StoredProcedure,

                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Update"
                },

                new StoredProcedureParameter
                {
                    Name = "@role_menu_id",
                    Value = id
                },

                new StoredProcedureParameter
                {
                    Name = "@role_id",
                    Value = dto.RoleId
                },

                new StoredProcedureParameter
                {
                    Name = "@menu_id",
                    Value = dto.MenuId
                });
        }

        public async Task<int> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Role menu ID must be greater than 0.");
            }

            return await _repository.ExecuteNonQueryAsync(
                StoredProcedure,

                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Delete"
                },

                new StoredProcedureParameter
                {
                    Name = "@role_menu_id",
                    Value = id
                });
        }

        public async Task<int> RestoreAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Role menu ID must be greater than 0.");
            }

            return await _repository.ExecuteNonQueryAsync(
                StoredProcedure,

                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Restore"
                },

                new StoredProcedureParameter
                {
                    Name = "@role_menu_id",
                    Value = id
                });
        }

        public async Task<List<RoleMenuResponseDto>> GetMenusByRoleAsync(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentException(
                    "Role ID is required.");
            }

            return await _repository.ExecuteQueryAsync<RoleMenuResponseDto>(
                StoredProcedure,

                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetMenusByRole"
                },

                new StoredProcedureParameter
                {
                    Name = "@role_id",
                    Value = roleId
                });
        }
    }
}