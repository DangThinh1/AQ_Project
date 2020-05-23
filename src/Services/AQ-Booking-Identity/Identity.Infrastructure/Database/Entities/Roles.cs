using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Database.Entities
{
    public class Roles : IdentityRole<int>
    {
        public string DomainFid { get; set; }
        public virtual List<UserRoles> UserRoles { get; set; }
        public virtual List<RoleControls> SubordinateRoles { get; set; }
    }
}
