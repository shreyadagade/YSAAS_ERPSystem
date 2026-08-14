using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.DTOs.Registration;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Domain.Entities.Registration;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.Infrastructure.Repositories.Registration
{
    public class StudentQualificationRepository
        : IStudentQualificationRepository
    {
        private readonly AppDbContext _context;

        public StudentQualificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentQualification?> GetByIdAsync(
            int qualificationId)
        {
            var result = await _context.StudentQualifications
                .FromSqlRaw(
                    @"EXEC erpsystem.sp_tblstudent_qualifications
                      @Type = 'GetById',
                      @qualification_id = {0}",
                    qualificationId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<StudentQualification>> GetAllAsync()
        {
            var results = await _context.StudentQualifications
                .FromSqlRaw(
                    @"EXEC erpsystem.sp_tblstudent_qualifications
                      @Type = 'GetAll'")
                .AsNoTracking()
                .ToListAsync();

            return results;
        }

        public async Task<StudentQualification> AddAsync(
            StudentQualification qualification)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_qualifications
                  @Type = 'Insert',
                  @student_id = {0},
                  @qualification = {1},
                  @passing_year = {2},
                  @university = {3},
                  @medium = {4},
                  @percentage = {5}",
                qualification.StudentId,
                qualification.Qualification,
                qualification.PassingYear,
                qualification.University,
                qualification.Medium,
                qualification.Percentage);

            return qualification;
        }

        public async Task UpdateAsync(
            StudentQualification qualification)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_qualifications
                  @Type = 'Update',
                  @qualification_id = {0},
                  @student_id = {1},
                  @qualification = {2},
                  @passing_year = {3},
                  @university = {4},
                  @medium = {5},
                  @percentage = {6}",
                qualification.QualificationId,
                qualification.StudentId,
                qualification.Qualification,
                qualification.PassingYear,
                qualification.University,
                qualification.Medium,
                qualification.Percentage);
        }

        public async Task DeleteAsync(int qualificationId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_qualifications
                  @Type = 'Delete',
                  @qualification_id = {0}",
                qualificationId);
        }

        public async Task RestoreAsync(int qualificationId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC erpsystem.sp_tblstudent_qualifications
                  @Type = 'Restore',
                  @qualification_id = {0}",
                qualificationId);
        }
    }
}