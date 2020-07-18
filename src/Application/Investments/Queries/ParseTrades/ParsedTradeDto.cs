using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Firewatch.Application.Common.Mappings;
using Firewatch.Application.Common.Models;
using Firewatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Application.Investments.Queries.ParseTrades
{
    public class ParsedTradeDto : IMapFrom<TradeExecution>
    {
        public string Symbol { get; set; }

        public DateTime Date { get; set; }

        public CostModel UnitPrice { get; set; }

        public decimal Quantity { get; set; }

        public bool IsDuplicate { get; set; } = false;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TradeExecution, ParsedTradeDto>()
                .ForMember(dest => dest.IsDuplicate, opt => opt.Ignore());
        }

    }
}
