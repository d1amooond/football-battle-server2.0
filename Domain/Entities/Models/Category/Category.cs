using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public List<CategoryDisplayName> DisplayNames { get; set; }
        public CategoryStatuses Status { get; set; }
    }
}
