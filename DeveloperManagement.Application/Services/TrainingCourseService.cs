using DeveloperManagement.Application.Contracts;
using DeveloperManagement.Application.DTOs.Course;
using DeveloperManagement.Application.Exceptions;
using DeveloperManagement.Application.Interfaces;

namespace DeveloperManagement.Application.Services
{
    public class TrainingCourseService : ITrainingCourseService
    {
        private readonly IGenericRepository _repository;

        private const string StoredProcedure =
            "erpsystem.sp_tbltraining_courses";

        public TrainingCourseService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TrainingCourseResponseDto>> GetAllAsync()
        {
            return await _repository.ExecuteQueryAsync<TrainingCourseResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });
        }

        public async Task<TrainingCourseResponseDto> GetByIdAsync(
            int courseId)
        {
            if (courseId <= 0)
            {
                throw new BadRequestException(
                    "Course ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<TrainingCourseResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetById"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@course_id",
                        Value = courseId
                    });

            var course = result.FirstOrDefault();

            if (course == null)
            {
                throw new NotFoundException(
                    $"Course with ID {courseId} was not found.");
            }

            return course;
        }

        public async Task<TrainingCourseResponseDto> CreateAsync(
            CreateTrainingCourseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CourseName))
            {
                throw new BadRequestException(
                    "Course name is required.");
            }

            if (dto.CourseName.Trim().Length > 100)
            {
                throw new BadRequestException(
                    "Course name cannot exceed 100 characters.");
            }

            if (dto.FeesAmount.HasValue &&
                dto.FeesAmount < 0)
            {
                throw new BadRequestException(
                    "Fees amount cannot be negative.");
            }

            if (dto.InstallmentPercentage.HasValue &&
                (dto.InstallmentPercentage < 0 ||
                 dto.InstallmentPercentage > 100))
            {
                throw new BadRequestException(
                    "Installment percentage must be between 0 and 100.");
            }

            var duplicateResult = await _repository.ExecuteQueryAsync<CheckNameResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "CheckName"
                },
                new StoredProcedureParameter
                {
                    Name = "@course_name",
                    Value = dto.CourseName.Trim()
                });

            var duplicateCourse = duplicateResult.FirstOrDefault();

            if (duplicateCourse != null && duplicateCourse.IsDuplicate)
            {
                throw new ConflictException(
                    "Course name already exists.");
            }
            var result =
                    await _repository.ExecuteQueryAsync<TrainingCourseResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Insert"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@course_name",
                        Value = dto.CourseName.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@fees_amount",
                        Value = dto.FeesAmount
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@fees_change_date",
                        Value = dto.FeesChangeDate
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@installment_percentage",
                        Value = dto.InstallmentPercentage
                    });

            var course = result.FirstOrDefault();

            if (course == null)
            {
                throw new Exception(
                    "Course creation failed.");
            }

            return course;
        }

        public async Task<TrainingCourseResponseDto> UpdateAsync(int courseId,
            UpdateTrainingCourseDto dto)
        {
            if (courseId <= 0)
            {
                throw new BadRequestException(
                    "Course ID must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.CourseName))
            {
                throw new BadRequestException(
                    "Course name is required.");
            }

            if (dto.CourseName.Trim().Length > 100)
            {
                throw new BadRequestException(
                    "Course name cannot exceed 100 characters.");
            }

            if (dto.FeesAmount.HasValue &&
                dto.FeesAmount < 0)
            {
                throw new BadRequestException(
                    "Fees amount cannot be negative.");
            }

            if (dto.InstallmentPercentage.HasValue &&
                (dto.InstallmentPercentage < 0 ||
                 dto.InstallmentPercentage > 100))
            {
                throw new BadRequestException(
                    "Installment percentage must be between 0 and 100.");
            }

            var duplicateResult = await _repository.ExecuteQueryAsync<CheckNameResponseDto>(
                StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "CheckName"
            },
            new StoredProcedureParameter
            {
                Name = "@course_name",
                Value = dto.CourseName.Trim()
            },
            new StoredProcedureParameter
            {
                Name = "@course_id",
                Value = courseId
            });

            var duplicateCourse = duplicateResult.FirstOrDefault();

            if (duplicateCourse != null && duplicateCourse.IsDuplicate)
            {
                throw new ConflictException(
                    "Course name already exists.");
            }

            var result =
                await _repository.ExecuteQueryAsync<TrainingCourseResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Update"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@course_id",
                        Value = courseId
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@course_name",
                        Value = dto.CourseName.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@fees_amount",
                        Value = dto.FeesAmount
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@fees_change_date",
                        Value = dto.FeesChangeDate
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@installment_percentage",
                        Value = dto.InstallmentPercentage
                    });

            var course = result.FirstOrDefault();

            if (course == null)
            {
                throw new NotFoundException(
                    $"Course with ID {courseId} was not found.");
            }

            return course;
        }

        public async Task<int> DeleteAsync(int courseId)
        {
            if (courseId <= 0)
            {
                throw new BadRequestException(
                    "Course ID must be greater than 0.");
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
                    Name = "@course_id",
                    Value = courseId
                });
        }

        public async Task<int> RestoreAsync(int courseId)
        {
            if (courseId <= 0)
            {
                throw new BadRequestException(
                    "Course ID must be greater than 0.");
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
                    Name = "@course_id",
                    Value = courseId
                });
        }
    }
}