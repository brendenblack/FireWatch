using Firewatch.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Firewatch.Application.IntegrationTests.Investments.Commands
{
    using static Testing;
    public class AddTradesCommandTests : TestBase
    {
        [Test]
        public async Task ShouldDoSomething()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
        }
    }
}
