using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class CategoryDisplayNameRequest
    {
        public Guid LanguageId { get; set; }
        public string LanguageShortCode { get; set; }
        public string Name { get; set; }
    }
}
