using System;
using System.Collections.Generic;
using System.Text;


namespace LeadManagement.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(
            string userId,
            string username,
            string role);
    }
}