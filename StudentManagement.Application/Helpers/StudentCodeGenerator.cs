
namespace StudentManagement.Application.Helpers
{
    public static class StudentCodeGenerator
    {
        public static string GenerateStudentCode(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            if (studentId < 10)
            {
                return $"CTIS{studentId:D3}";
            }

            return $"CTIS{studentId:D4}";
        }
    }
}

