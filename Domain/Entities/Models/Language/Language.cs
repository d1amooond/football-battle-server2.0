using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Language : BaseEntity
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public LanguageStatuses Status { get; set; }
    }
}
