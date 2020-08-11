using System;

namespace Firewatch.Application.Investments.Queries.GetJournal
{
    public class JournalVm
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public decimal AverageReturn { get; set; }

        public decimal AverageWinner { get; set; }

        public decimal AverageLoser { get; set; }
    }
}