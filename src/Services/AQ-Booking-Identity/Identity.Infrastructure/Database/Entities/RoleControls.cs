namespace Identity.Infrastructure.Database.Entities
{
    public class RoleControls
    {
        public int SuperiorFid { get; set; }
        public int SubordinateFid { get; set; }
        public Roles SubordinateRole { get; set; }
    }
}
