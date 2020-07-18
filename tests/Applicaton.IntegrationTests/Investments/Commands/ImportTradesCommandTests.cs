using Firewatch.Application.Common.Models;
using Firewatch.Application.Investments.Commands.ImportTrades;
using Firewatch.Domain.Constants;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Firewatch.Application.IntegrationTests.Investments.Commands
{
    using static Testing;
    public class ImportTradesCommandTests : TestBase
    {
        //[Test]
        //public async Task ShouldDoSomething()
        //{
        //    var userId = await RunAsDefaultUserAsync();
        //    var command = new ImportTradesCommand
        //    {
        //        OwnerId = userId,
        //        RequestorId = userId,
        //        Trades = new List<ImportTradeModel>
        //        {
        //            new ImportTradeModel
        //            {
        //                Date = new DateTime(),
        //                Action = TradeConstants.BUY_TO_OPEN,
        //                Symbol = "AMD",
        //                Quantity = 50,
        //                UnitPrice = new CostModel(53.2m, "USD")
        //            }
        //        }
        //    };

        //    var result = await SendAsync(command);
        //}


    }
}
