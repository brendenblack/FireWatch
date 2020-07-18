using AutoMapper;
using Firewatch.Application.Common.Mappings;
using Firewatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Application.Accounts.Queries.GetAccounts
{
    public class AccountDto : IMapFrom<Account>
    {
        public AccountDto()
        {
        }
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Type { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.Type, o => o.MapFrom(src => src.AccountType));
        }
    }
}
