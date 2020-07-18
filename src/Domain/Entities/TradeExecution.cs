using Firewatch.Domain.Common;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class TradeExecution : AuditableEntity
    {
        public TradeExecution() { }

        public TradeExecution(
            BrokerageAccount account,
            string action,
            [NotNull] DateTime date,
            [NotNull] string symbol,
            decimal quantity,
            [NotNull] Price unitPrice,
            Price commissions,
            Price fees,
            IEnumerable<string> exchanges = null)
        {
            AccountId = account.Id;
            Account = account;
            Action = action?.ToLower() ?? "unknown";
            Date = date;
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Quantity = quantity;
            UnitPrice = unitPrice;
            //Exchanges = (exchanges == null) ? exchanges.ToHashSet() : new HashSet<string>();
            Commissions = commissions ?? new Price();
            Fees = fees ?? new Price();
        }

        public int Id { get; set; }

        public int AccountId { get; }

        public BrokerageAccount Account { get; }

        public DateTime Date { get; private set; }

        public string Action { get; private set; }

        public TradeAction ActionType
        {
            get
            {
                var action = Action.ToUpper();
                if (action.Contains("SELL"))
                {
                    return TradeAction.SELL;
                }
                else
                {
                    return TradeAction.BUY;
                }
            }
        }

        public string Symbol { get; private set; }

        public decimal Quantity { get; private set; }

        public IReadOnlyCollection<string> Exchanges { get; private set; } = new HashSet<string>();

        public Price UnitPrice { get; private set; }

        /// <summary>
        /// Calculates the net value, including fees & commissions.
        /// </summary>
        /// <remarks>
        /// This method currently only supports buy-to-open and sell-to-close calculations.
        /// </remarks>
        public decimal TotalValue
        {
            get
            {
                // TODO: handle sell to open and buy to close
                if (Action == TradeConstants.BUY_TO_OPEN)
                {
                    return Quantity * UnitPrice.Amount + (Fees.Amount * -1) + (Commissions.Amount * -1);
                }
                else if (Action == TradeConstants.SELL_TO_CLOSE)
                {
                    return Quantity * UnitPrice.Amount - Fees.Amount - Commissions.Amount;
                }
                else
                {
                    return -1;
                }
            }
        }

        public Price Commissions { get; private set; } = new Price();

        public Price Fees { get; private set; } = new Price();

        public IReadOnlyCollection<string> Tags { get; } = new HashSet<string>();

        public void AddTag(string tag)
        {
            ((HashSet<string>)this.Tags).Add(tag);
        }

        public void RemoveTag(string tag)
        {
            ((HashSet<string>)this.Tags).Remove(tag);
        }

        public string CreationMethod { get; set; }
    }
}
