using TeixeiraSoftware.Finance;

namespace Firewatch.Domain.Entities
{
    public class Price
    {
        public Price() { }

        public Price(decimal amount, string currencyCode)
        {
            this.Amount = amount;
            this.Currency = Currency.ByAlphabeticCode(currencyCode);
        }

        public Price(decimal amount, Currency currency)
        {
            this.Amount = amount;
            this.Currency = currency;
        }

        public decimal Amount { get; set; } = 0.0m;

        public Currency Currency { get; set; } = Currency.USD;
    }
}
