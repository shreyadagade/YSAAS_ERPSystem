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
        Task<bool> UpdateUserAsync(UpdateUserDto dto);
        Task<bool> DeleteUserAsync(DeleteUserDto dto);
    }
}
