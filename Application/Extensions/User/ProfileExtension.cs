using Application.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Extensions;

namespace Application.Extensions
{
    public static class ProfileExtension
    {
        public static ProfileDTO ToDTO(this Profile profile)
        {
            return new ProfileDTO
            {
                Id = profile.Id.AsGuid(),
                Country = profile.Country,
                Email = profile.Email,
                Mobile = profile.Mobile,
            };
        }
    }
}
