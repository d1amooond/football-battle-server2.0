using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum Roles
    {
        None = 0,
        User = 1 << 0, // 1
        Support = 1 << 1, // 2
        SupportManager = 1 << 2, // 4
        Reviewer = 1 << 3, // 8
        ReviewManager = 1 << 4, // 16
        Translator = 1 << 5, // 32
        TranslateManager = 1 << 6, // 64
        Admin = 1 << 7, // 128
        SysAdmin = 1 << 8, // 256

        SupportRoles = Support | SupportManager,
        ReviewRoles = Reviewer | ReviewManager,
        TranslateRoles = TranslateManager | Translator,
        InternalRoles = Admin | SysAdmin,
    }
}
