using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firewatch.Application.Common.Services
{
    public class TradeFactory
    {
        public IEnumerable<Trade> ConstructIntradayTradesFromExecutions(IEnumerable<TradeExecution> executions)
        {
            var groupedExecutions = executions.GroupBy(e => new { e.Date.Date, e.Symbol })
                .Select(g => new { g.Key.Date, g.Key.Symbol, Executions = g.ToList() });

            var trades = new List<Trade>();
            foreach (var group in groupedExecutions)
            {
                decimal positionSize = 0;
                Trade trade = new Trade(group.Symbol);
                foreach (var execution in group.Executions.OrderBy(e => e.Date))
                {
                    // iterate through all executions in chronological order

                    if (positionSize == 0 && execution.Status == TradeStatus.CLOSE)
                    {
                        // first trade appears to be closing out a swing position, ignore it
                        continue;
                    }

                    trade.AddExecutions(execution);
                    positionSize += execution.Quantity;

                    if (execution.Status == TradeStatus.CLOSE && positionSize == 0)
                    {
                        // the latest trade was a SELLTOCLOSE or BUYTOCLOSE (cover) order that caused
                        // the position size to be 0, closing this group of executions as one trade.

                        // add the trade to the trades collection and start a new one
                        trades.Add(trade);
                        trade = new Trade(group.Symbol);
                    }

                    // note: if the final trade for a given date & symbol does not close out a position,
                    // it will not be added to the trades collection; this is by design, as we only want to fetch 
                    // intraday trades.
                }
            }

            return trades;
        }
    }
}
