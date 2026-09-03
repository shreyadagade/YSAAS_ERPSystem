using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories.Course;
using StudentManagement.Infrastructure.Data;
using System.Data;

namespace StudentManagement.Infrastructure.Repositories.Course
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CourseExistsAsync(int courseId)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT COUNT(1)
                FROM erpsystem.tbltraining_courses
                WHERE course_id = @course_id
                  AND flag = 1";

            command.CommandType = CommandType.Text;

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@course_id";
            parameter.Value = courseId;

            command.Parameters.Add(parameter);

            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result) > 0;
        }
    }
}