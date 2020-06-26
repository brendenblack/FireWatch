using Firewatch.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class Trade : AuditableEntity
    {
        public Trade() { }

        public Trade(
            Person owner,
            TradeActions action,
            DateTime date,
            [NotNull] string symbol,
            decimal quantity,
            [NotNull] Cost unitPrice,
            Cost commissions,
            Cost fees,
            IEnumerable<string> exchanges = null)
        {
            OwnerId = owner.Id;
            Owner = owner;
            Action = action;
            Date = date;
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Quantity = quantity;
            UnitPrice = unitPrice;
            //Exchanges = (exchanges == null) ? exchanges.ToHashSet() : new HashSet<string>();
            Commissions = commissions ?? new Cost();
            Fees = fees ?? new Cost();
        }

        public int Id { get; set; }

        public string OwnerId { get; private set; }

        public Person Owner { get; private set; }

        public DateTime Date { get; private set; }

        public TradeActions Action { get; private set; }

        public string Symbol { get; private set; }

        public decimal Quantity { get; private set; }

        public IReadOnlyCollection<string> Exchanges { get; private set; } = new HashSet<string>();

        public Cost UnitPrice { get; private set; }

        public decimal Total
        {
            get
            {
                // TODO: handle sell to open and buy to close
                if (Action == TradeActions.BUYTOOPEN)
                {
                    return Quantity * UnitPrice.Amount + (Fees.Amount * -1) + (Commissions.Amount * -1);
                }
                else if (Action == TradeActions.SELLTOCLOSE)
                {
                    return Quantity * UnitPrice.Amount - Fees.Amount - Commissions.Amount;
                }
                else
                {
                    return -1;
                }
            }
        }

        public Cost Commissions { get; private set; } = new Cost();

        public Cost Fees { get; private set; } = new Cost();

        public IReadOnlyCollection<string> Tags { get; } = new HashSet<string>();

        public void AddTag(string tag)
        {
            ((HashSet<string>)this.Tags).Add(tag);
        }

        public void RemoveTag(string tag)
        {
            ((HashSet<string>)this.Tags).Remove(tag);
        }
    }

    public enum TradeActions { BUYTOOPEN, BUYTOCLOSE, SELLTOOPEN, SELLTOCLOSE };
}
