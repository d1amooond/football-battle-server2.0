using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public Roles Role { get; set; }
        public int Coins { get; set; }
        public int Rating { get; set; }
        public string Country { get; set; }
    }
}
