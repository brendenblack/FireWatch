using System.Collections;
using System.Collections.Generic;

namespace Firewatch.Application.Investments.Queries.GetTradeExecutions
{
    public class TradeExecutionsVm
    {
        public IList<TradeExecutionDto> Executions { get; set; } = new List<TradeExecutionDto>();
    }
}