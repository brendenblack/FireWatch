using Firewatch.Application.Common.Mappings;
using Firewatch.Application.Common.Models;
using Firewatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Application.Investments.Queries.GetTradeExecutions
{
    public class TradeExecutionDto : IMapFrom<TradeExecution>
    {
        public DateTime Date { get; set; }

        public string Action { get; set; }

        public string ActionType { get; set; }

        public string Symbol { get; set; }

        public decimal Quantity { get; set; }

        public CostModel UnitPrice { get; set; }

        public CostModel Commissions { get; set; }

        public CostModel Fees { get; set; }
    }
}
