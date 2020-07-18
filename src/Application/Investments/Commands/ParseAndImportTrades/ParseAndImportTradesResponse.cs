using System.Collections;
using System.Collections.Generic;

namespace Firewatch.Application.Investments.Commands.ParseAndImportTrades
{
    public class ParseAndImportTradesResponse
    {
        public int Duplicates { get; set; }

        public IList<int> CreatedIds { get; set; }
    }
}