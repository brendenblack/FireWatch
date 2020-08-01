using Castle.Core.Logging;
using FakeItEasy;
using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Entities;
using Firewatch.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Firewatch.Application.UnitTests.Common.Services.TradeFactoryTests
{
    /// <summary>
    /// Provides a facility to easily create <see cref="TradeExecution"/> records from a TradeLog file by
    /// leveraging <see cref="TradeLogTradeParserService"/>.
    /// </summary>
    /// <remarks>
    /// While this does break encapsulation of unit tests, it significantly reduces the burden of constructing
    /// test objects by hand and instead shifts it to a data-driven approach that is much easier to maintain.
    /// </remarks>
    public abstract class TradeFactoryTestBase
    {
        /// <summary>
        /// Returns all <see cref="TradeExecution"/>s described in the specified file.
        /// </summary>
        /// <param name="filename">The name of the TradeLog file to read, relative to Common/Services/TradeFactoryTests</param>
        /// <returns></returns>
        public IEnumerable<TradeExecution> PopulateTradeExecutionsWithFile(string filename)
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(dirName, "Common/Services/TradeFactoryTests/", filename);
            var contents = File.ReadAllText(file);

            var service = new TradeLogTradeParserService(NUnitTestLogger.Create<TradeLogTradeParserService>(), A.Fake<IApplicationDbContext>());
            
            return service.ParseForOwner(new Person(), contents);
        }
    }
}
