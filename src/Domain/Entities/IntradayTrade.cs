using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Entities
{
    /// <summary>
    /// Represents a trade when the position is opened and closed in the same trading session.
    /// </summary>
    public class IntradayTrade : Trade
    {
        public IntradayTrade(string symbol) : base(symbol)
        {

        }

        public override void AddExecutions(params TradeExecution[] executions)
        {
            base.AddExecutions(executions);
        }
    }
}
