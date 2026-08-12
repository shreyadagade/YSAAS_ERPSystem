using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message): base(401, message)
        {
        }
    }
}
