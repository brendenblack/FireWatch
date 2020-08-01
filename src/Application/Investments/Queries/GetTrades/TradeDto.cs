using Firewatch.Application.Common.Mappings;
using Firewatch.Application.Common.Models;
using Firewatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Application.Investments.Queries.GetTrades
{
    public class TradeDto : IMapFrom<Trade>
    {
        public DateTime Open { get; set; }

        public DateTime Close { get; set; }

        public string Vehicle { get; set; }

        public string Side { get; set; }

        public string Symbol { get; set; }

        public string State { get; set; }

        public decimal Position { get; set; }

        public int ExecutionCount { get; set; }

        /// <summary>
        /// How many total shares were exchanged during this trade.
        /// </summary>
        public decimal Volume { get; set; }

        public decimal NetProfitAndLoss { get; set; }

        public decimal GrossProfitAndLoss { get; set; }

        public bool IsClosed { get; set; }

        public bool IsIntraDay { get; set; }


        public IList<TradeExecutionDto> Executions { get; set; }


    }
}
