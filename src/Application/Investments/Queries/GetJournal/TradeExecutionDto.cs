using Firewatch.Application.Common.Mappings;
using Firewatch.Application.Common.Models;
using Firewatch.Domain.Entities;
using System;

namespace Firewatch.Application.Investments.Queries.GetJournal
{
    public class TradeExecutionDto : IMapFrom<TradeExecution>
    {
        public DateTime Date { get; set; }

        public string Intent { get; set; }

        public string Action { get; set; }

        public string ActionType { get; set; }

        public string Symbol { get; set; }

        public string Vehicle { get; set; }

        public decimal Quantity { get; set; }

        public CostModel UnitPrice { get; set; }

        public CostModel Commissions { get; set; }

        public CostModel Fees { get; set; }
    }
}
