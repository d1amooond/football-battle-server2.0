using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PageList
    {
        public bool HasNextPage { get; set; } = false;
        public bool HasPreviousPage { get; set; } = false;
        public int CurrentPage { get; set; } = 0;
        public int Pages { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public dynamic Items { get; set; }
    }
}
