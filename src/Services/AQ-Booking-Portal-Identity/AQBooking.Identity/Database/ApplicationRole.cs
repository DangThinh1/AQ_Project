using Microsoft.AspNetCore.Identity;

namespace AQBooking.Identity.Database
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string DomainFid { get; set; }
    }
}
