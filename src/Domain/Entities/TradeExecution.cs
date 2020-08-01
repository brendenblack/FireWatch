using Firewatch.Domain.Common;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class TradeExecution : AuditableEntity
    {
        public TradeExecution() { }

        public TradeExecution(
            [NotNull] BrokerageAccount account,
            [NotNull] DateTime date,
            [NotNull] string symbol,
            [NotNull] decimal quantity,
            [NotNull] Price unitPrice,
            Price commissions = null,
            Price fees = null,
            TradeActions tradeAction = TradeActions.BUY_TO_OPEN,
            bool isPartial = false,
            IEnumerable<string> routes = null,
            TradeVehicle vehicle = TradeVehicle.STOCK,
            string creationMethod = TradeConstants.CREATION_METHOD_MANUAL)
        {
            AccountId = account.Id;
            Account = account;

            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Date = date;
            Vehicle = vehicle;
            Quantity = quantity;
            UnitPrice = unitPrice;
            IsPartialExecution = isPartial;

            Action = tradeAction;
            Intent = (Action == TradeActions.BUY_TO_OPEN || Action == TradeActions.SELL_TO_OPEN) ? TradeIntents.OPENING : TradeIntents.CLOSING;
            ActionType = (Action == TradeActions.BUY_TO_OPEN || Action == TradeActions.BUY_TO_CLOSE) ? TradeActionTypes.BUY: TradeActionTypes.SELL;
                     
            Routes = (routes == null) ? new HashSet<string>() : routes.ToHashSet();
            Commissions = commissions ?? new Price();
            Fees = fees ?? new Price();

            CreationMethod = creationMethod;
        }

        public int Id { get; private set; }

        public int AccountId { get; private set; }

        public BrokerageAccount Account { get; private set; }

        public TradeVehicle Vehicle { get; private set; }

        public DateTime Date { get; private set; }

        public TradeActions Action { get; private set; }

        /// <summary>
        /// Indicates whether the intent of this trade is to grow (<see cref="TradeIntents.OPENING"/>) or 
        /// shrink (<see cref="TradeIntents.CLOSING"/>) a position.
        /// </summary>
        public TradeIntents Intent { get; private set; }

        /// <summary>
        /// Whether this trade was buying or selling shares.
        /// </summary>
        public TradeActionTypes ActionType { get; private set; }

        /// <summary>
        /// The identifying symbol of the asset being traded.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// How many units of the underlying asset are being traded.
        /// </summary>
        public decimal Quantity { get; private set; }

        /// <summary>
        /// What routes were used to fill this order.
        /// </summary>
        public IReadOnlyCollection<string> Routes { get; private set; } = new HashSet<string>();

        /// <summary>
        /// The average price that this order executed at.
        /// </summary>
        public Price UnitPrice { get; private set; }

        /// <summary>
        /// Calculates the net value, including fees & commissions.
        /// </summary>
        public decimal TotalValue
        {
            get
            {
                decimal value;

                if (Vehicle == TradeVehicle.OPTION)
                {
                    value = Quantity * -1 * UnitPrice.Amount * 100;
                }
                else
                {
                    value = Quantity * -1 * UnitPrice.Amount;
                }

                value -= Fees.Amount;
                value -= Commissions.Amount;

                return value;
            }
        }

        /// <summary>
        /// How much the broker charged in commissions to execute this order. The amount will be negative for
        /// commissions taken, or positive for a refund.
        /// </summary>
        public Price Commissions { get; private set; } = new Price();

        /// <summary>
        /// How much the broker and routes charged in fees to execute this order. The amount will
        /// be negative for a charged fee, and positive for a refund.
        /// </summary>
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

        /// <summary>
        /// Whether this execution was a partial fill by the platform.
        /// </summary>
        public bool IsPartialExecution { get; private set; }

        public override string ToString()
        {
            return $"{ActionType} {Symbol} x {Quantity} @ {UnitPrice.Amount:C2}";
        }
    }
}
