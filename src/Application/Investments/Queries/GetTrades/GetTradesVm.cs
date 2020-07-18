using System.Collections;
using System.Collections.Generic;

namespace Firewatch.Application.Investments.Queries.GetTrades
{
    public class GetTradesVm
    {
        public IList<TradeDto> Trades { get; set; } = new List<TradeDto>();
    }
}