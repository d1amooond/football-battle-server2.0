using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum LanguageStatuses
    {
        Archived = -10,
        Undefined = 0,
        Draft = 1,
        InProgress = 2,
        Active = 3,
    }
}
