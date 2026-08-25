using UserManagement.Application.Contracts;
using UserManagement.Application.DTOs.Branch;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IGenericRepository _repository;

        private const string StoredProcedure ="erpsystem.sp_tblbranches";

        public BranchService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BranchResponseDto>> GetAllAsync()
        {
            return await _repository.ExecuteQueryAsync<BranchResponseDto>(StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });
        }

        public async Task<BranchResponseDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Branch ID must be greater than 0.");
            }

            var result = await _repository.ExecuteQueryAsync<BranchResponseDto>(
                    StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetById"
                },
                new StoredProcedureParameter
                {
                    Name = "@branch_id",
                    Value = id
                });

                var branch = result.FirstOrDefault();

            if (branch == null)
            {
                throw new NotFoundException("Branch not found.");
            }

            return branch;
        }

        public async Task<int> InsertAsync(CreateBranchDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.BranchName))
            {
                throw new BadRequestException("Branch name is required.");
            }

            return await _repository.ExecuteNonQueryAsync(StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Insert"
                },
                new StoredProcedureParameter
                {
                    Name = "@branch_name",
                    Value = dto.BranchName
                });
        }

        public async Task<int> UpdateAsync(int id,UpdateBranchDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException(
                    "Branch ID must be greater than 0.");
            }

            if (dto == null)
            {
                throw new BadRequestException(
                    "Branch data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.BranchName))
            {
                throw new BadRequestException(
                    "Branch name is required.");
            }
            var existingBranch = await GetByIdAsync(id);

            return await _repository.ExecuteNonQueryAsync(
                StoredProcedure,

                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Update"
                },

                new StoredProcedureParameter
                {
                    Name = "@branch_id",
                    Value = id
                },

                new StoredProcedureParameter
                {
                    Name = "@branch_name",
                    Value = dto.BranchName
                });
        }

        public async Task<int> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Branch ID must be greater than 0.");
            }

            var existingBranch = await GetByIdAsync(id);

            return await _repository.ExecuteNonQueryAsync(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Delete"
                },
                new StoredProcedureParameter
                {
                    Name = "@branch_id",
                    Value = id
                });
        }

        public async Task<int> RestoreAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Branch ID must be greater than 0.");
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
                    Name = "@branch_id",
                    Value = id
                });
        }
    }
}