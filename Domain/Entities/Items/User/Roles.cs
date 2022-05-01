using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum Roles
    {
        //internal 1-10
        SysAdmin = 1,
        Admin = 2,
        ReviewManager = 3,
        SupportManager = 4,
        Support = 4,
        TranslateManager = 5,
        Translator = 6,

        //common
        User = 11,
        Guest = 12,
    }
}
