using System.Collections.Generic;

namespace Firewatch.Application.Investments.Commands.ImportTrades
{
    public class ImportTradesResponse
    {
        public List<int> CreatedIds { get; set; } = new List<int>();

        public int DuplicateCount { get; set; }
    }
}
