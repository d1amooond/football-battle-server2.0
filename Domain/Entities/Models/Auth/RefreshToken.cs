using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime? Revoked { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive => this.ExpiresAt > DateTime.UtcNow && this.Revoked != null;
    }
}
