using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum CategoryStatuses
    {
        Archived = -10,
        Undefined = 0,
        Draft = 1,
        InProgress = 2,
        InTranslateReview = 3,
        Active = 4,
    }
}
