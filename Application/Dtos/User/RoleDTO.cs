using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public Roles Type { get; set; }
    }
}
