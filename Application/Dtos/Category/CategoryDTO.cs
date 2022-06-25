using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CategoryDisplayName> DisplayNames { get; set; }
        public CategoryStatuses Status { get; set; }
    }
}
