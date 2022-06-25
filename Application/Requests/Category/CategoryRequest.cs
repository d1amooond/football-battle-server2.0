using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Requests
{

    public class CreateDraftCategoryRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public List<CategoryDisplayNameRequest> DisplayNames { get; set; }
    }

    public class UpdateCategoryRequest : CreateCategoryRequest
    {
        public Guid Id { get; set; }
        public CategoryStatuses Status { get; set; }
    }
}
