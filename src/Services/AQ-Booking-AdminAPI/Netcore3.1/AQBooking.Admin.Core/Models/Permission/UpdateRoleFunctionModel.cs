using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.Permission
{
    public class UpdateRoleFunctionModel
    {
        public int RoleId { get; set; }
        public int PageId { get; set; }
        public int FunctionId { get; set; }
    }
}
