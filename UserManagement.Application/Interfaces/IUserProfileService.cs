using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.User;

namespace UserManagement.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<ProfileResponseDto> GetProfileAsync(string userId);
        Task<string> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    }
}
