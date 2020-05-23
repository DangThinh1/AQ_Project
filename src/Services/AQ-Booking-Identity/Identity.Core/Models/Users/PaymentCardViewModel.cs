namespace Identity.Core.Models.Users
{
    public class PaymentCardViewModel
    {
        public string Id { get; set; }
        public string SecurityNumber { get; set; }
        public string Expiration { get; set; }
        public string CardHolderName { get; set; }
        public int CardType { get; set; }
    }
}
