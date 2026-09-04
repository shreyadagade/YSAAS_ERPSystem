using LeadManagement.Application.DTOs.TrainingCourse;
using LeadManagement.Application.Interfaces.Repositories.TrainingCourse;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace LeadManagement.Application.Services
{
    public class TrainingCourseService : ITrainingCourseService
    {
        private readonly ITrainingCourseRepository _courseRepository;
        private readonly ILogger<TrainingCourseService> _logger;

        public TrainingCourseService(
            ITrainingCourseRepository courseRepository,
            ILogger<TrainingCourseService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task<int> CreateAsync(TrainingCourseDto course)
        {
            _logger.LogInformation(
                "Creating training course. CourseName: {CourseName}",
                course.CourseName);

            // 1. Course name validation
            if (string.IsNullOrWhiteSpace(course.CourseName))
            {
                _logger.LogWarning(
                    "Course creation failed: Course name is required.");

                throw new ArgumentException("Course name is required.");
            }

            // 2. Maximum length validation
            if (course.CourseName.Length > 100)
            {
                _logger.LogWarning(
                    "Course creation failed: Course name exceeds 100 characters.");

                throw new ArgumentException(
                    "Course name cannot exceed 100 characters.");
            }

            // 3. Duplicate course name validation
            if (await _courseRepository.CourseNameExistsAsync(course.CourseName))
            {
                _logger.LogWarning(
                    "Course creation failed: Duplicate course name {CourseName}",
                    course.CourseName);

                throw new Exception(
                    "A course with this name already exists.");
            }

            var entity = new TblTrainingCourse
            {
                CourseName = course.CourseName.Trim()
            };

            // 4. Save
            var courseId = await _courseRepository.InsertAsync(entity);

            _logger.LogInformation(
                "Training course created successfully. CourseId: {CourseId}",
                courseId);

            return courseId;
        }

        public async Task<bool> UpdateAsync(TrainingCourseDto course)
        {
            _logger.LogInformation(
                "Updating training course. CourseId: {CourseId}",
                course.CourseId);

            // 1. Course ID validation
            if (course.CourseId <= 0)
            {
                _logger.LogWarning(
                    "Course update failed: Invalid CourseId.");

                throw new ArgumentException("Invalid course ID.");
            }

            // 2. Course name validation
            if (string.IsNullOrWhiteSpace(course.CourseName))
                throw new ArgumentException("Course name is required.");

            // 3. Maximum length validation
            if (course.CourseName.Length > 100)
                throw new ArgumentException(
                    "Course name cannot exceed 100 characters.");

            // 4. Duplicate course name validation
         
            if (await _courseRepository.CourseNameExistsAsync(course.CourseName))
            {
                _logger.LogWarning(
                    "Course creation failed: Duplicate course name {CourseName}",
                    course.CourseName);

                throw new Exception(
                    "A course with this name already exists.");
            }

            var entity = new TblTrainingCourse
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName.Trim()
            };

            var result = await _courseRepository.UpdateAsync(entity);

            if (result)
            {
                _logger.LogInformation(
                    "Training course updated successfully. CourseId: {CourseId}",
                    course.CourseId);
            }
            else
            {
                _logger.LogWarning(
                    "Course update failed or course not found. CourseId: {CourseId}",
                    course.CourseId);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(int courseId)
        {
            _logger.LogInformation(
                "Deleting training course. CourseId: {CourseId}",
                courseId);

            if (courseId <= 0)
                throw new ArgumentException("Invalid course ID.");

            var result = await _courseRepository.DeleteAsync(courseId);

            if (result)
            {
                _logger.LogInformation(
                    "Training course deleted successfully. CourseId: {CourseId}",
                    courseId);
            }
            else
            {
                _logger.LogWarning(
                    "Course delete failed or course not found. CourseId: {CourseId}",
                    courseId);
            }

            return result;
        }

        public async Task<bool> RestoreAsync(int courseId)
        {
            _logger.LogInformation(
                "Restoring training course. CourseId: {CourseId}",
                courseId);

            if (courseId <= 0)
                throw new ArgumentException("Invalid course ID.");

            var result = await _courseRepository.RestoreAsync(courseId);

            if (result)
            {
                _logger.LogInformation(
                    "Training course restored successfully. CourseId: {CourseId}",
                    courseId);
            }
            else
            {
                _logger.LogWarning(
                    "Course restore failed or course not found. CourseId: {CourseId}",
                    courseId);
            }

            return result;
        }

        public async Task<TrainingCourseDto?> GetByIdAsync(int courseId)
        {
            _logger.LogInformation(
                "Getting training course by ID. CourseId: {CourseId}",
                courseId);

            if (courseId <= 0)
                throw new ArgumentException("Invalid course ID.");

            var entity = await _courseRepository.GetByIdAsync(courseId);

            if (entity == null)
            {
                _logger.LogWarning(
                    "Training course not found. CourseId: {CourseId}",
                    courseId);

                return null;
            }

            return new TrainingCourseDto
            {
                CourseId = entity.CourseId,
                CourseName = entity.CourseName,
                //Flag = entity.Flag,
                //InsertedAt = entity.InsertedAt,
                //UpdatedAt = entity.UpdatedAt,
                //DeletedAt = entity.DeletedAt,
                //RestoredAt = entity.RestoredAt
            };
        }

        public async Task<IEnumerable<TrainingCourseDto>> GetAllAsync()
        {
            _logger.LogInformation(
                "Getting all active training courses.");

            var entities = await _courseRepository.GetAllAsync();

            _logger.LogInformation(
                "Retrieved {Count} training courses.",
                entities.Count());

            return entities.Select(entity => new TrainingCourseDto
            {
                CourseId = entity.CourseId,
                CourseName = entity.CourseName,
                //Flag = entity.Flag,
                //InsertedAt = entity.InsertedAt,
                //UpdatedAt = entity.UpdatedAt,
                //DeletedAt = entity.DeletedAt,
                //RestoredAt = entity.RestoredAt
            });
        }
    }
}

