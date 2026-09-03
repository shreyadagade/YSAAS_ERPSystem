using Dapper;
using LeadManagement.Application.Interfaces.Repositories.TrainingCourse;
using LeadManagement.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LeadManagement.Infrastructure.Repositories
{


    public class TrainingCourseRepository : ITrainingCourseRepository
    {
        private readonly IConfiguration _configuration;

        public TrainingCourseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> InsertAsync(TblTrainingCourse course)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Insert");
            parameters.Add("@course_name", course.CourseName);

            var result = await connection.QuerySingleAsync<dynamic>(
                "erpsystem.sp_tbltraining_courses",
                parameters,
                commandType: CommandType.StoredProcedure);

            return (int)result.course_id;
        }

        public async Task<bool> UpdateAsync(TblTrainingCourse course)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Update");
            parameters.Add("@course_id", course.CourseId);
            parameters.Add("@course_name", course.CourseName);

            await connection.QueryFirstOrDefaultAsync(
                "erpsystem.sp_tbltraining_courses",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<bool> DeleteAsync(int courseId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Delete");
            parameters.Add("@course_id", courseId);

            await connection.QueryFirstOrDefaultAsync(
                "erpsystem.sp_tbltraining_courses",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<bool> RestoreAsync(int courseId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "Restore");
            parameters.Add("@course_id", courseId);

            await connection.QueryFirstOrDefaultAsync(
                "erpsystem.sp_tbltraining_courses",
                parameters,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<TblTrainingCourse?> GetByIdAsync(int courseId)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "GetById");
            parameters.Add("@course_id", courseId);

            return await connection.QueryFirstOrDefaultAsync<TblTrainingCourse>(
                "erpsystem.sp_tbltraining_courses",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TblTrainingCourse>> GetAllAsync()
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "GetAll");

            return await connection.QueryAsync<TblTrainingCourse>(
                "erpsystem.sp_tbltraining_courses",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        public async Task<bool> CourseNameExistsAsync(
    string courseName,
    int? courseId = null)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Type", "CheckName");
            parameters.Add("@course_name", courseName);
            parameters.Add("@course_id", courseId);

            return await connection.QuerySingleAsync<bool>(
                "erpsystem.sp_tbltraining_courses",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}

