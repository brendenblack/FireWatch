using Firewatch.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Application.Investments.Commands.ImportTrades
{
    public class ImportTradeModel
    {
        public DateTime Date { get; set; }

        public string Action { get; set; }

        public string Symbol { get; set; }

        public decimal Quantity { get; set; }

        public CostModel UnitPrice { get; set; } = new CostModel();

        public CostModel Commissions { get; set; } = new CostModel();

        public CostModel Fees { get; set; } = new CostModel();
    }
}
