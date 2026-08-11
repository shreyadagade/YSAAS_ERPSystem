using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.Auth;

namespace UserManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterUserDto dto);

        Task<LoginResponseDto> LoginAsync(LoginDto dto);

        Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenDto dto);

        Task LogoutAsync(string refreshToken);


    }
}
