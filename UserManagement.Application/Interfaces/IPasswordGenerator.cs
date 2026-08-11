using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.Interfaces
{
    public interface IPasswordGenerator
    {
        string GeneratePassword();
    }
}
