using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.DTOs.Profile;
using StudentManagement.Application.DTOs.StudentProfile;
using StudentManagement.Application.Interfaces.Repositories.Profile;
using StudentManagement.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace StudentManagement.Infrastructure.Repositories.Profile
{
    public class StudentProfileRepository : IStudentProfileRepository
    {
        private readonly AppDbContext _context;

        public StudentProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET PROFILE
        // =====================================================

        public async Task<StudentProfileDto?> GetProfileByStudentIdAsync(
            int studentId)
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
                "erpsystem.sp_tblstudent_details";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "GetById");

            AddParameter(
                command,
                "@student_id",
                studentId);

            using var reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapProfile(reader);
        }

        // =====================================================
        // CHANGE PROFILE
        // =====================================================

        public async Task<bool> ChangeProfileAsync(
            int studentId,
            ChangeProfileRequestDto request)
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
                "erpsystem.sp_tblstudent_details";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "Update");

            AddParameter(
                command,
                "@student_id",
                studentId);

            AddParameter(
                command,
                "@student_name",
                request.StudentName);

            AddParameter(
                command,
                "@gender",
                request.Gender);

            AddParameter(
                command,
                "@mobile_number",
                request.MobileNumber);

            AddParameter(
                command,
                "@email_address",
                request.EmailAddress);

            AddParameter(
                command,
                "@birth_date",
                request.BirthDate);

            // =================================================
            // IMPORTANT:
            // ProfilePhoto is NOT updated here.
            //
            // Profile photo has a separate API:
            // change-profile-photo
            // =================================================

            AddParameter(
                command,
                "@qualification",
                request.Qualification);

            AddParameter(
                command,
                "@parent_name",
                request.ParentName);

            AddParameter(
                command,
                "@parent_number",
                request.ParentNumber);

            AddParameter(
                command,
                "@last_name",
                request.LastName);

            AddParameter(
                command,
                "@whatsapp_number",
                request.WhatsappNumber);

            AddParameter(
                command,
                "@local_address",
                request.LocalAddress);

            AddParameter(
                command,
                "@permanent_address",
                request.PermanentAddress);

            AddParameter(
                command,
                "@permanent_identification_number",
                request.PermanentIdentificationNumber);

            AddParameter(
                command,
                "@aadhar_card_number",
                request.AadharCardNumber);

            AddParameter(
                command,
                "@aadhar_card_photo",
                request.AadharCardPhoto);

            AddParameter(
                command,
                "@branch_id",
                request.BranchId);

            await command.ExecuteNonQueryAsync();

            return true;
        }

        // =====================================================
        // CHANGE PROFILE PHOTO
        // =====================================================

        public async Task<bool> ChangeProfilePhotoAsync(
            int studentId,
            string profilePhoto)
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
                "erpsystem.sp_tblstudent_details";

            command.CommandType =
                CommandType.StoredProcedure;

            AddParameter(
                command,
                "@Type",
                "ChangeProfilePhoto");

            AddParameter(
                command,
                "@student_id",
                studentId);

            AddParameter(
                command,
                "@profile_photo",
                profilePhoto);

            await command.ExecuteNonQueryAsync();

            return true;
        }

        // =====================================================
        // MAP PROFILE
        // =====================================================

        private static StudentProfileDto MapProfile(
            DbDataReader reader)
        {
            return new StudentProfileDto
            {
                StudentId =
                    Convert.ToInt32(
                        reader["student_id"]),

                StudentCode =
                    reader["student_code"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["student_code"]),

                StudentName =
                    reader["student_name"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["student_name"]),

                LastName =
                    reader["last_name"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["last_name"]),

                Gender =
                    reader["gender"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["gender"]),

                MobileNumber =
                    reader["mobile_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["mobile_number"]),

                WhatsappNumber =
                    reader["whatsapp_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["whatsapp_number"]),

                EmailAddress =
                    reader["email_address"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["email_address"]),

                BirthDate =
                    reader["birth_date"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(
                            reader["birth_date"]),

                ProfilePhoto =
                    reader["profile_photo"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["profile_photo"]),

                Qualification =
                    reader["qualification"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["qualification"]),

                ParentName =
                    reader["parent_name"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["parent_name"]),

                ParentNumber =
                    reader["parent_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["parent_number"]),

                LocalAddress =
                    reader["local_address"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["local_address"]),

                PermanentAddress =
                    reader["permanent_address"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["permanent_address"]),

                PermanentIdentificationNumber =
                    reader["permanent_identification_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["permanent_identification_number"]),

                AadharCardNumber =
                    reader["aadhar_card_number"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["aadhar_card_number"]),

                AadharCardPhoto =
                    reader["aadhar_card_photo"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["aadhar_card_photo"]),

                BranchId =
                    reader["branch_id"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(
                            reader["branch_id"]),

                BranchName =
                    HasColumn(
                        reader,
                        "branch_name") &&
                    reader["branch_name"] != DBNull.Value
                        ? Convert.ToString(
                            reader["branch_name"])
                        : null
            };
        }

        // =====================================================
        // CHECK COLUMN
        // =====================================================

        private static bool HasColumn(
            DbDataReader reader,
            string columnName)
        {
            for (
                int i = 0;
                i < reader.FieldCount;
                i++)
            {
                if (
                    reader.GetName(i)
                        .Equals(
                            columnName,
                            StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
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