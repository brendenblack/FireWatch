using AutoMapper;
using Firewatch.Application.Common.Mappings;
using Firewatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;

namespace Firewatch.Application.Common.Models
{
    public class CostModel : IMapFrom<Price>
    {
        public CostModel() 
            : this(0, "USD") { }

        public CostModel(Price cost) 
            : this(cost.Amount, cost.Currency.AlphabeticCode) { }
        
        public CostModel(decimal amount, Currency currency)
            : this(amount, currency.AlphabeticCode)
        {

        }

        public CostModel(decimal amount, string currency)
        {
            this.Amount = amount;
            this.Currency = currency;
        }

        public decimal Amount { get; set; } = 0.0m;

        public string Currency { get; set; } = "USD";


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Price, CostModel>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(s => s.Currency.AlphabeticCode));
        }
    }
}
