using System.Collections.Generic;

namespace Firewatch.Application.Investments.Queries.ParseTrades
{
    public class ParseTradesResponse
    {
        public IList<ParsedTradeDto> Trades { get; set; }
    }
}