using System.Collections.Generic;

namespace Firewatch.Application.Investments.Queries.GetMetadata
{
    public class InvestmentsMetadataVm
    {
        public Dictionary<string, string[]> SupportedFormats { get; set; } = new Dictionary<string, string[]>();

        public string[] FinancialAssetTypes { get; set; }
    }
}