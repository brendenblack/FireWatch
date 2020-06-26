using TeixeiraSoftware.Finance;

namespace Firewatch.Domain.Entities
{
    public class Cost
    {
        public Cost() { }

        public Cost(decimal amount, string currencyCode)
        {
            this.Amount = amount;
            this.Currency = Currency.ByAlphabeticCode(currencyCode);
        }

        public Cost(decimal amount, Currency currency)
        {
            this.Amount = amount;
            this.Currency = currency;
        }

        public decimal Amount { get; set; } = 0.0m;

        public Currency Currency { get; set; } = Currency.USD;
    }
}
