using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ProfileDTO
    {
        public Guid Id { get; set; }
        public Countries Country { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }


    }
}
