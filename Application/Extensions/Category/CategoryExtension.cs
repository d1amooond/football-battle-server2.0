using Application.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Extensions;

namespace Application.Extensions
{
    public static class CategoryExtension
    {
        public static CategoryDTO ToDTO(this Category category)
        {
            return new CategoryDTO
            {
                Name = category.Name,
                Id = category.Id.AsGuid(),
                DisplayNames = category.DisplayNames,
                Status = category.Status,
            };
        }
    }
}
