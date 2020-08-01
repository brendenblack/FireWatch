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

        public TradeVehicle Vehicle { get; private set; }

        public TradeSides Side { get; private set; }

        public DateTime Open => (Executions.Count() > 0) ? Executions.Select(e => e.Date).Min() : DateTime.MinValue;

        public DateTime Close => (Executions.Count() > 0) ? Executions.Select(e => e.Date).Max() : DateTime.MinValue;

        public TradeState State => (Position == 0) ? TradeState.CLOSED : TradeState.OPEN;

        public decimal Position => Executions.Select(e => e.Quantity).Sum();

        public decimal AverageEntry => (Executions.Count() == 0) ? 0 : Executions.Where(e => e.Intent == TradeIntents.OPENING).Select(e => e.UnitPrice.Amount).Sum();

        public decimal AverageExit => (Executions.Count() == 0) ? 0 : Executions.Where(e => e.Intent == TradeIntents.CLOSING).Select(e => e.UnitPrice.Amount).Sum();

        public decimal LargestPosition
        {
            get
            {
                decimal max = 0;
                decimal running = 0;
                
                foreach (var execution in Executions)
                {
                    running += execution.Quantity;
                    max = Math.Max(max, running);
                }

                return max;
            }
        }

        public int ExecutionCount => Executions.Count();

        public decimal Volume => (Executions.Count() == 0) ? 0 : Executions.Select(e => Math.Abs(e.Quantity)).Sum();

        private decimal CalculateProfitAndLoss(bool includeFees = false, bool includeCommissions = false)
        {
            decimal pnl = 0;
            int inversionFactor = (Vehicle == TradeVehicle.OPTION) ? -100 : -1;

            foreach (var exec in Executions)
            {
                
                // the product is negated because a buy order has a positive quantity, and a sell order has a negative quantity
                pnl += exec.UnitPrice.Amount * exec.Quantity * inversionFactor;

                if (includeCommissions)
                    pnl += exec.Commissions.Amount;
                
                if (includeFees)
                    pnl += exec.Fees.Amount;
            }

            return pnl;
        }

        /// <summary>
        /// Total returns from this trade, including fees and commissions.
        /// </summary>
        /// <remarks></remarks>
        public decimal NetProfitAndLoss => CalculateProfitAndLoss(true, true);
       
        /// <summary>
        /// Total returns from this trade, before fees and commissions.
        /// </summary>
        public decimal GrossProfitAndLoss => CalculateProfitAndLoss(false, false);

        public IEnumerable<TradeExecution> Executions { get; } = new List<TradeExecution>();

        public virtual void AddExecutions(params TradeExecution[] executions)
        {
            foreach (var execution in executions)
            {
                if (!execution.Symbol.Equals(Symbol, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Executions.Count() == 0 || execution.Date < Executions.Select(e => e.Date).Min())
                {
                    if (execution.Action == TradeActions.SELL_TO_OPEN)
                    {
                        this.Side = TradeSides.SHORT;
                    }
                    else if (execution.Action == TradeActions.BUY_TO_OPEN)
                    {
                        this.Side = TradeSides.LONG;
                    }
                }

                ((List<TradeExecution>)Executions).Add(execution);
                // all executions must be made on the same vehicle, so we'll just overwrite
                // with whatever the last one is. 
                // TODO: validation, throw an exception?
                this.Vehicle = execution.Vehicle;
            }
        }

        public string Symbol { get; }

        public bool IsClosed => State == TradeState.CLOSED;

        public bool IsIntraDay => IsClosed && Executions.Select(e => e.Date.Date).Distinct().Count() == 1;
    }
}
