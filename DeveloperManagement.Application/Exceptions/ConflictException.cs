using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Exceptions
{
    public class ConflictException : AppException
    {
        public ConflictException(string message): base(409, message)
        {
        }
    }
}
