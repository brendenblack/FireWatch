using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Application.Common.Interfaces
{
    /// <summary>
    /// Responsible for reading the contents of various trade report formats and returning <see cref="TradeExecution"/> records.
    /// </summary>
    public interface ITradeParserService
    {
        FinancialAssetTypes[] SupportedFinancialAssets { get; }

        string Format { get; }

        IEnumerable<TradeExecution> ParseForOwner(Person owner, string contents);


    }
}
