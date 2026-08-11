using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.Contracts
{
    public class StoredProcedureParameter
    {
        public string Name { get; set; } = string.Empty;

        public object? Value { get; set; }
    }
}
