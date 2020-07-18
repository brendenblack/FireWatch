using Firewatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firewatch.Domain.Entities
{
    /// <summary>
    /// A trade represents a series of <see cref="TradeExecution"/>s that are related on a single ticker symbol.
    /// </summary>
    public class Trade
    {
        private Trade() { }

        public Trade(string symbol)
        {
            this.Symbol = symbol;
        }

        public DateTime Open => (Executions.Count() > 0) ? Executions.Select(e => e.Date).Min() : DateTime.MinValue;

        public DateTime Close => (Executions.Count() > 0) ? Executions.Select(e => e.Date).Max() : DateTime.MinValue;

        public TradePositionStatus Status => (PositionSize == 0) ? TradePositionStatus.CLOSED : TradePositionStatus.OPEN;

        public decimal PositionSize => Executions.Select(e => e.Quantity).Sum();

        public int ExecutionCount => Executions.Count();

        public decimal Volume => (Executions.Count() == 0) ? 0 : Executions.Select(e => Math.Abs(e.Quantity)).Sum();

        /// <summary>
        /// Total returns from this trade, including fees and commissions.
        /// </summary>
        /// <remarks></remarks>
        public decimal NetProfitAndLoss
        {
            get
            {
                decimal pnl = 0;

                foreach (var exec in Executions)
                {
                    // the product is negated because a buy order has a positive quantity, and a sell order has a negative quantity
                    pnl += exec.UnitPrice.Amount * exec.Quantity * -1;
                    pnl -= exec.Commissions.Amount;
                    pnl -= exec.Fees.Amount;
                }

                return pnl;
            }
        }

        /// <summary>
        /// Total returns from this trade, before fees and commissions.
        /// </summary>
        public decimal GrossProfitAndLoss
        {
            get
            {
                decimal pnl = 0;

                foreach (var exec in Executions)
                {
                    pnl += exec.UnitPrice.Amount * exec.Quantity * -1;
                }

                return pnl;
            }
        }

        public IEnumerable<TradeExecution> Executions { get; } = new List<TradeExecution>();

        public void AddExecutions(params TradeExecution[] executions)
        {
            foreach (var execution in executions)
            {
                if (!execution.Symbol.Equals(Symbol, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                ((List<TradeExecution>)Executions).Add(execution);
            }
        }

        public string Symbol { get; }
    }
}
