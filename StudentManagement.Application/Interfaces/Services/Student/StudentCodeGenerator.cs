namespace StudentManagement.Application.Interfaces.Services.Student
{
    public static class StudentCodeGenerator
    {
        public static string GenerateStudentCode(int studentId)
        {
            return $"CTIS{studentId:D3}".Replace(
                $"CTIS{studentId:D3}",
                $"CTIS{studentId}");
        }
    }
}

