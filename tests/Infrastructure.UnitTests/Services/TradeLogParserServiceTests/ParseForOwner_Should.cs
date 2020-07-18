using FakeItEasy;
using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Entities;
using Firewatch.Infrastructure.Services;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Firewatch.Infrastructure.UnitTests.Services.TradeLogParserServiceTests
{
    public class ParseForOwner_Should
    {
        public ParseForOwner_Should()
        {
            _sut = new TradeLogTradeParserService(NUnitTestLogger.Create<TradeLogTradeParserService>(), A.Fake<IApplicationDbContext>());
        }

        [OneTimeSetUp]
        public void ReadLocalTestFile()
        {
            var filename = "U3111111_20200316_20200501.tlg";
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(dirName, filename);

            _contents = File.ReadAllText(file);
        }

        private readonly TradeLogTradeParserService _sut;
        private string _contents;

        [Test]
        public void ShouldDo()
        {
            var trades = _sut.ParseForOwner(new Person { Id = Guid.NewGuid().ToString() }, _contents);

            trades.ShouldNotBeEmpty();
        }
    }
}
