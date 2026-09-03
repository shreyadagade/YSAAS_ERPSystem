using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.Interfaces.Repositories.Login
{
    public interface ILoginRepository
    {
        Task<(bool Success, string UserId, string UserName, string Role)>
            ValidateLoginAsync(
                string userName,
                string password);
    }
}
