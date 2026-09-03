using LeadManagement.Application.DTOs.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.Interfaces.Services.Login
{
    public interface ILoginService
    {
        Task<LoginResponseDto?> LoginAsync(
            LoginRequestDto request);
    }
}
