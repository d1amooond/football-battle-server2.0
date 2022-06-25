using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UserDTO
    {
        public string Username { get; set; }
        public RoleDTO Role { get; set; }
        public ProfileDTO Profile { get; set; }
    }
}
