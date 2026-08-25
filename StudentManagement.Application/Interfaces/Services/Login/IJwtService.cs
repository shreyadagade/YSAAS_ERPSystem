using System;
using System.Collections.Generic;
using System.Text;


    namespace StudentManagement.Application.Interfaces.Services.Login
    {
        public interface IJwtService
        {
            string GenerateToken(
                int studentId,
                string studentCode);
        }
    }
