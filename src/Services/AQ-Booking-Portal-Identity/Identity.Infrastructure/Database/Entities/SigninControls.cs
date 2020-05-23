namespace Identity.Infrastructure.Database.Entities
{
    public class SigninControls
    {
        public int Id { get; set; }
        public string CurrentDomainUid { get; set; }
        public string CurrentDomainName { get; set; }
        public string ToGoDomainUid { get; set; }
        public string ToGoDomainName { get; set; }
        public string ToGoDomainCallbackUrl { get; set; }
    }
}