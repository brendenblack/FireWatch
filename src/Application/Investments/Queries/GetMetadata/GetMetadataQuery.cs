using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Investments.Queries.GetMetadata
{
    public class GetMetadataQuery : IRequest<InvestmentsMetadataVm>
    {
    }

    public class GetMetadataHandler : IRequestHandler<GetMetadataQuery, InvestmentsMetadataVm>
    {
        private readonly IEnumerable<ITradeParserService> _parsers;

        public GetMetadataHandler(IEnumerable<ITradeParserService> parsers)
        {
            _parsers = parsers;
        }

        public async Task<InvestmentsMetadataVm> Handle(GetMetadataQuery request, CancellationToken cancellationToken)
        {
            var vm = new InvestmentsMetadataVm();

            foreach (var parser in _parsers)
            {
                vm.SupportedFormats.Add(parser.Format, parser.SupportedFinancialAssets.Select(a => a.ToString()).ToArray());
            }

            vm.FinancialAssetTypes = Enum.GetValues(typeof(FinancialAssetTypes))
                .Cast<FinancialAssetTypes>()
                .Select(f => f.ToString())
                .ToArray();

            return vm;
        }
    }
}
