namespace Identity.Infrastructure.Database.Entities
{
    public class UserRoles
    {
        public int RoleFid { get; set; }
        public int UserFid { get; set; }

        public virtual Users User { get; set; }
        public virtual Roles Role { get; set; }
    }
}
