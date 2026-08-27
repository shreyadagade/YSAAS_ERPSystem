using StudentManagement.Domain.Entities.Student;

namespace StudentManagement.Application.Interfaces.Repositories.Student
{
    public interface IStudentDetailsRepository
    {
        // =====================================================
        // GET BY ID
        // =====================================================

        Task<StudentDetails?> GetByIdAsync(
            int studentId);

        // =====================================================
        // GET ALL
        // =====================================================

        Task<IEnumerable<StudentDetails>> GetAllAsync();

        // =====================================================
        // CHECK DUPLICATE EMAIL
        // =====================================================

        Task<bool> ExistsByEmailAsync(
            string emailAddress);

        // =====================================================
        // CHECK DUPLICATE AADHAAR
        // =====================================================

        Task<bool> ExistsByAadharAsync(
            string aadharCardNumber);

        // =====================================================
        // CHECK DUPLICATE PERMANENT IDENTIFICATION NUMBER
        // =====================================================

        Task<bool> ExistsByPermanentIdentificationNumberAsync(
            string permanentIdentificationNumber);

        // =====================================================
        // CHECK DUPLICATE WHATSAPP NUMBER
        // =====================================================

        Task<bool> ExistsByWhatsappNumberAsync(
            string whatsappNumber);

        // =====================================================
        // CHECK DUPLICATE MOBILE NUMBER
        // =====================================================

        Task<bool> ExistsByMobileNumberAsync(
            string mobileNumber);

        // =====================================================
        // CREATE
        // =====================================================

        Task<StudentDetails> AddAsync(
            StudentDetails student);

        // =====================================================
        // UPDATE
        // =====================================================

        Task UpdateAsync(
            StudentDetails student);

        // =====================================================
        // DELETE
        // =====================================================

        Task DeleteAsync(
            int studentId);

        // =====================================================
        // RESTORE
        // =====================================================

        Task RestoreAsync(
            int studentId);

        // =====================================================
        // LOGIN
        // =====================================================

        Task<StudentDetails?> GetByStudentCodeAsync(
            string studentCode);
    }
}