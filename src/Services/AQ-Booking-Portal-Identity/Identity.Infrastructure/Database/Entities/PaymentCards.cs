namespace Identity.Infrastructure.Database.Entities
{
    public class PaymentCards
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public string Expiration { get; set; }
        public string CardHolderName { get; set; }
        public int CardType { get; set; }
    }
}
