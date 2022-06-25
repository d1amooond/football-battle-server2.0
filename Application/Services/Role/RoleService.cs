using Application.General;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Extensions;

namespace Application.Services
{
    public class RoleService
    {

        private readonly Context app;
        public RoleService(Context app)
        {
            this.app = app;
        }

        public async Task<bool> HasPermissions(Guid? roleId, int permittedRoles)
        {
            var hasPermissions = false;
            try
            {
                if (!roleId.HasValue) return hasPermissions;

                var role = await this.app.Repository.User.GetById<Role>(roleId.Value.AsObjectId(), "roles");
                if ((((int)role.Type) & permittedRoles) == (int)role.Type)
                {
                    hasPermissions = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return hasPermissions;
        }
    }
}
