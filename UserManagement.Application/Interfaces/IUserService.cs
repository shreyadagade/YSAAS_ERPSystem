using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.User;

namespace UserManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> ChangeUserStatusAsync(ChangeUserStatusDto dto);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<string> UpdateUserAsync(string userId,UpdateUserDto dto);
        Task<bool> DeleteUserAsync(string userId);
    }
}
