using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message): base(400, message)
        {
        }
    }
}
