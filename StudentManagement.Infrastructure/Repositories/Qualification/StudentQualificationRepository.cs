using StudentManagement.Application.Interfaces.Repositories.Qualification;
using StudentManagement.Domain.Entities.Student;
using StudentManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace StudentManagement.Infrastructure.Repositories.Qualification
    {
        public class StudentQualificationRepository
            : IStudentQualificationRepository
        {
            private readonly AppDbContext _context;

            public StudentQualificationRepository(
                AppDbContext context)
            {
                _context = context;
            }

            // =====================================================
            // GET BY ID
            // =====================================================
            public async Task<StudentQualification?> GetByIdAsync(
                int qualificationId)
            {
                var connection =
                    _context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command =
                    connection.CreateCommand();

                command.CommandText =
                    "erpsystem.sp_tblstudent_qualifications";

                command.CommandType =
                    CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@Type",
                    "GetById");

                AddParameter(
                    command,
                    "@qualification_id",
                    qualificationId);

                using var reader =
                    await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    return null;
                }

                return MapQualification(reader);
            }

            // =====================================================
            // GET ALL
            // =====================================================
            public async Task<IEnumerable<StudentQualification>>
                GetAllAsync()
            {
                var connection =
                    _context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command =
                    connection.CreateCommand();

                command.CommandText =
                    "erpsystem.sp_tblstudent_qualifications";

                command.CommandType =
                    CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@Type",
                    "GetAll");

                using var reader =
                    await command.ExecuteReaderAsync();

                var qualifications =
                    new List<StudentQualification>();

                while (await reader.ReadAsync())
                {
                    qualifications.Add(
                        MapQualification(reader));
                }

                return qualifications;
            }

            // =====================================================
            // INSERT
            // =====================================================
            public async Task<StudentQualification> AddAsync(
                StudentQualification qualification)
            {
                var connection =
                    _context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command =
                    connection.CreateCommand();

                command.CommandText =
                    "erpsystem.sp_tblstudent_qualifications";

                command.CommandType =
                    CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@Type",
                    "Insert");

                AddParameter(
                    command,
                    "@student_id",
                    qualification.StudentId);

                AddParameter(
                    command,
                    "@qualification",
                    qualification.Qualification);

                AddParameter(
                    command,
                    "@passing_year",
                    qualification.PassingYear);

                AddParameter(
                    command,
                    "@university",
                    qualification.University);

                AddParameter(
                    command,
                    "@medium",
                    qualification.Medium);

                AddParameter(
                    command,
                    "@percentage",
                    qualification.Percentage);

                await command.ExecuteNonQueryAsync();

                // Stored procedure does not return
                // qualification_id.
                //
                // Therefore retrieve the newly inserted
                // qualification using the student_id
                // and qualification details.

                return await GetInsertedQualificationAsync(
                    qualification);
            }

            // =====================================================
            // UPDATE
            // =====================================================
            public async Task UpdateAsync(
                StudentQualification qualification)
            {
                var connection =
                    _context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command =
                    connection.CreateCommand();

                command.CommandText =
                    "erpsystem.sp_tblstudent_qualifications";

                command.CommandType =
                    CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@Type",
                    "Update");

                AddParameter(
                    command,
                    "@qualification_id",
                    qualification.QualificationId);

                AddParameter(
                    command,
                    "@student_id",
                    qualification.StudentId);

                AddParameter(
                    command,
                    "@qualification",
                    qualification.Qualification);

                AddParameter(
                    command,
                    "@passing_year",
                    qualification.PassingYear);

                AddParameter(
                    command,
                    "@university",
                    qualification.University);

                AddParameter(
                    command,
                    "@medium",
                    qualification.Medium);

                AddParameter(
                    command,
                    "@percentage",
                    qualification.Percentage);

                await command.ExecuteNonQueryAsync();
            }

            // =====================================================
            // DELETE
            // =====================================================
            public async Task DeleteAsync(
                int qualificationId)
            {
                var connection =
                    _context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command =
                    connection.CreateCommand();

                command.CommandText =
                    "erpsystem.sp_tblstudent_qualifications";

                command.CommandType =
                    CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@Type",
                    "Delete");

                AddParameter(
                    command,
                    "@qualification_id",
                    qualificationId);

                await command.ExecuteNonQueryAsync();
            }

            // =====================================================
            // RESTORE
            // =====================================================
            public async Task RestoreAsync(
                int qualificationId)
            {
                var connection =
                    _context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command =
                    connection.CreateCommand();

                command.CommandText =
                    "erpsystem.sp_tblstudent_qualifications";

                command.CommandType =
                    CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@Type",
                    "Restore");

                AddParameter(
                    command,
                    "@qualification_id",
                    qualificationId);

                await command.ExecuteNonQueryAsync();
            }

            // =====================================================
            // GET INSERTED QUALIFICATION
            // =====================================================
            private async Task<StudentQualification>
                GetInsertedQualificationAsync(
                    StudentQualification qualification)
            {
                var connection =
                    _context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command =
                    connection.CreateCommand();

                command.CommandText = @"
                SELECT TOP 1
                    qualification_id,
                    student_id,
                    qualification,
                    passing_year,
                    university,
                    medium,
                    percentage
                FROM erpsystem.tblstudent_qualifications
                WHERE student_id = @student_id
                  AND qualification = @qualification
                  AND flag = 0
                ORDER BY qualification_id DESC";

                command.CommandType =
                    CommandType.Text;

                AddParameter(
                    command,
                    "@student_id",
                    qualification.StudentId);

                AddParameter(
                    command,
                    "@qualification",
                    qualification.Qualification);

                using var reader =
                    await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    throw new Exception(
                        "Qualification was created but could not be retrieved.");
                }

                return MapQualification(reader);
            }

            // =====================================================
            // MAP DATABASE RESULT
            // =====================================================
            private static StudentQualification
                MapQualification(
                    DbDataReader reader)
            {
                return new StudentQualification
                {
                    QualificationId =
                        Convert.ToInt32(
                            reader["qualification_id"]),

                    StudentId =
                        reader["student_id"] == DBNull.Value
                            ? null
                            : Convert.ToInt32(
                                reader["student_id"]),

                    Qualification =
                        reader["qualification"] == DBNull.Value
                            ? null
                            : Convert.ToString(
                                reader["qualification"]),

                    PassingYear =
                        reader["passing_year"] == DBNull.Value
                            ? null
                            : Convert.ToInt32(
                                reader["passing_year"]),

                    University =
                        reader["university"] == DBNull.Value
                            ? null
                            : Convert.ToString(
                                reader["university"]),

                    Medium =
                        reader["medium"] == DBNull.Value
                            ? null
                            : Convert.ToString(
                                reader["medium"]),

                    Percentage =
                        reader["percentage"] == DBNull.Value
                            ? null
                            : Convert.ToDouble(
                                reader["percentage"])
                };
            }

            // =====================================================
            // ADD SQL PARAMETER
            // =====================================================
            private static void AddParameter(
                DbCommand command,
                string parameterName,
                object? value)
            {
                var parameter =
                    command.CreateParameter();

                parameter.ParameterName =
                    parameterName;

                parameter.Value =
                    value ?? DBNull.Value;

                command.Parameters.Add(parameter);
            }
        }
    }
