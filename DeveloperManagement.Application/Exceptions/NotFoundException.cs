using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message): base(404, message)
        {
        }
    }
}
