using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories.Course;
using StudentManagement.Infrastructure.Data;

using CourseEntity = StudentManagement.Domain.Entities.Course.Course;

namespace StudentManagement.Infrastructure.Repositories.Course
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CourseEntity?> GetCourseByIdAsync(int courseId)
        {
            return await _context.Set<CourseEntity>()
                .FirstOrDefaultAsync(x => x.CourseId == courseId);
        }
    }
}