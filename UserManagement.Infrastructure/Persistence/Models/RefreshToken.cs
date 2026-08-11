using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Infrastructure.Persistence.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiryDate { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime CreatedDate { get; set; }
    }
}
