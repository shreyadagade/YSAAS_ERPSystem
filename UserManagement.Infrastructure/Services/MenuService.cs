using UserManagement.Application.Contracts;
using UserManagement.Application.DTOs.Menu;
using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Services
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository _repository;

        private const string StoredProcedure = "erpsystem.sp_tblmenus";

        public MenuService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MenuResponseDto>> GetAllAsync()
        {
            return await _repository.ExecuteQueryAsync<MenuResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });
        }

        public async Task<MenuResponseDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Menu ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<MenuResponseDto>(
                    StoredProcedure,

                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetById"
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@menu_id",
                        Value = id
                    });

            return result.FirstOrDefault();
        }

        public async Task<int> InsertAsync(CreateMenuDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException(
                    "Menu data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.MenuName))
            {
                throw new ArgumentException(
                    "Menu name is required.");
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
                    Name = "@menu_name",
                    Value = dto.MenuName.Trim()
                },

                new StoredProcedureParameter
                {
                    Name = "@menu_url",
                    Value = dto.MenuUrl
                },

                new StoredProcedureParameter
                {
                    Name = "@parent_menu_id",
                    Value = dto.ParentMenuId
                },

                new StoredProcedureParameter
                {
                    Name = "@icon",
                    Value = dto.Icon
                },

                new StoredProcedureParameter
                {
                    Name = "@display_order",
                    Value = dto.DisplayOrder
                });
        }

        public async Task<int> UpdateAsync(int id,UpdateMenuDto dto)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Menu ID must be greater than 0.");
            }

            if (dto == null)
            {
                throw new ArgumentException(
                    "Menu data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.MenuName))
            {
                throw new ArgumentException(
                    "Menu name is required.");
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
                    Name = "@menu_id",
                    Value = id
                },

                new StoredProcedureParameter
                {
                    Name = "@menu_name",
                    Value = dto.MenuName.Trim()
                },

                new StoredProcedureParameter
                {
                    Name = "@menu_url",
                    Value = dto.MenuUrl
                },

                new StoredProcedureParameter
                {
                    Name = "@parent_menu_id",
                    Value = dto.ParentMenuId
                },

                new StoredProcedureParameter
                {
                    Name = "@icon",
                    Value = dto.Icon
                },

                new StoredProcedureParameter
                {
                    Name = "@display_order",
                    Value = dto.DisplayOrder
                });
        }

        public async Task<int> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Menu ID must be greater than 0.");
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
                    Name = "@menu_id",
                    Value = id
                });
        }

        public async Task<int> RestoreAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Menu ID must be greater than 0.");
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
                    Name = "@menu_id",
                    Value = id
                });
        }
    }
}