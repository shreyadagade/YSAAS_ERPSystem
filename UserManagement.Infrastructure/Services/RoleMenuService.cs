using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Contracts;
using UserManagement.Application.DTOs.Common;
using UserManagement.Application.DTOs.RoleMenu;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Services
{
    public class RoleMenuService : IRoleMenuService
    {
        private readonly IGenericRepository _repository;
        private readonly RoleManager<IdentityRole> _roleManager;

        private const string StoredProcedure = "erpsystem.sp_tblRoleMenus";

        public RoleMenuService(
            IGenericRepository repository, 
            RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _roleManager = roleManager;
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
                throw new BadRequestException(
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

            var roleMenu = result.FirstOrDefault();

            if (roleMenu == null)
            {
                throw new NotFoundException(
                    "Role menu not found.");
            }

            return roleMenu;
        }

        public async Task<int> InsertAsync(CreateRoleMenuDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("Role menu data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.RoleId))
            {
                throw new BadRequestException("Role ID is required.");
            }

            if (dto.MenuId <= 0)
            {
                throw new BadRequestException("Menu ID must be greater than 0.");
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
                throw new BadRequestException(
                    "Role menu ID must be greater than 0.");
            }

            if (dto == null)
            {
                throw new BadRequestException(
                    "Role menu data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.RoleId))
            {
                throw new BadRequestException(
                    "Role ID is required.");
            }

            if (dto.MenuId <= 0)
            {
                throw new BadRequestException(
                    "Menu ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<OperationResultDto>(
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

            var resultCode =
                result.FirstOrDefault()?.ResultCode;

            if (resultCode == 0)
            {
                throw new NotFoundException(
                    "Role menu not found.");
            }

            if (resultCode == 2)
            {
                throw new BadRequestException(
                    "Deleted role menu cannot be updated.");
            }

            return 1;
        }

        public async Task<int> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException(
                    "Role menu ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<OperationResultDto>(
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

            var resultCode =
                result.FirstOrDefault()?.ResultCode;

            if (resultCode == 0)
            {
                throw new NotFoundException(
                    "Role menu not found.");
            }

            if (resultCode == 2)
            {
                throw new BadRequestException(
                    "Role menu is already deleted.");
            }

            return 1;
        }

        public async Task<int> RestoreAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException(
                    "Role menu ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<OperationResultDto>(
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

            var resultCode =
                result.FirstOrDefault()?.ResultCode;

            if (resultCode == 0)
            {
                throw new NotFoundException(
                    "Role menu not found.");
            }

            if (resultCode == 2)
            {
                throw new BadRequestException(
                    "Role menu is already active.");
            }

            return 1;
        }

        public async Task<List<RoleMenuResponseDto>> GetMenusByRoleAsync(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new BadRequestException(
                    "Role ID is required.");
            }

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                throw new NotFoundException(
                    "Role not found.");
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