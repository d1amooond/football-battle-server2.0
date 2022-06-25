using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class CreateLanguageRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ShortCode { get; set; }
    }

    public class UpdateLanguageRequest : CreateLanguageRequest
    {
        public Guid Id {get; set; }
        public LanguageStatuses Status { get; set; }
    }
}
