using Application.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Extensions;

namespace Application.Extensions
{
    public static class RoleExtension
    {
        public static RoleDTO ToDTO(this Role role)
        {
            return new RoleDTO
            {
                Id = role.Id.AsGuid(),
                Type = role.Type,
            };
        }
    }
}
