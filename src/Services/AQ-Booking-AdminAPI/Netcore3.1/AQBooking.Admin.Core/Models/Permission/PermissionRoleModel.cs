using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.Permission
{
    public class PermissionRoleModel
    {
        public string RoleName { get; set; }
        public string RoleNameResKey { get; set; }
        public List<PermissionPageFunctionModel> LstModule { get; set; }
    }
}
