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
        /// <summary>
        /// Groups <see cref="TradeExecution"/>s together from open to close of a positon on a single symbol. 
        /// <para>
        /// If the execution that opened a position is not provided, then that sequence of executions will be
        /// ignored.
        /// </para>
        /// </summary>
        /// <param name="executions"></param>
        /// <returns></returns>
        public IEnumerable<Trade> ConstructTradesFromExecutions(IEnumerable<TradeExecution> executions)
        {
            var trades = new List<Trade>();

            // group by account, date, symbol
            var groupedExecutions = executions.GroupBy(e => new { e.AccountId, e.Date.Date, e.Symbol })
               .Select(g => new { g.Key.AccountId, g.Key.Date, g.Key.Symbol, Executions = g.OrderBy(e => e.Date).ToList() });

            foreach (var group in groupedExecutions)
            {
                // Initialize the first trade for this account & date & symbol
                var trade = new Trade(group.Symbol);

                // Initialize a tracker that will monitor the current position size, and
                // when it returns to 0 that means the trade is closed.
                //decimal positionSizeTracker = 0;

                for (int i = 0; i < group.Executions.Count; i++)
                {
                    var execution = group.Executions[i];

                    if (trade.Position == 0 && execution.Intent == TradeIntents.CLOSING)
                    {
                        // first trade appears to be closing out an untracked position, so we'll 
                        // just go ahead and ignore it
                        continue;
                    }

                    trade.AddExecutions(execution);

                    if (i == group.Executions.Count - 1)
                    {
                        // This is the last execution, so we should add this trade to the collection whether
                        // it is open or closed.
                        trades.Add(trade);
                    }
                    else if (execution.Intent == TradeIntents.CLOSING && trade.Position == 0 && i < group.Executions.Count)
                    {
                        // The latest trade was a SELLTOCLOSE or BUYTOCLOSE (cover) order that caused
                        // the position size to be 0, which closes this group of executions as one trade.
                        // Add the trade to the collection an initialize a new one
                        trades.Add(trade);
                        trade = new Trade(group.Symbol);
                    }
                }
            }
            
            return trades;
        }
    }
}
