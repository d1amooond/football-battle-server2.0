using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class UserExtension
    {
        public static UserDTO ToDTO(this Domain.Entities.User user)
            => new()
            {
                Username = user.Username,
            };
    }
}
