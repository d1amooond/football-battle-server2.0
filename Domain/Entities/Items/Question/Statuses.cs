using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum Statuses
    {
        Undefined = 0,
        Active = 1,
        Draft = 2,
        Overdue = 3,
        Cancelled = 4
    }
}
