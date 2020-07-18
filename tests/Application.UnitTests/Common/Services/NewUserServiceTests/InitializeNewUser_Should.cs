using Firewatch.Application.Common.Services;
using Firewatch.Domain.Entities;
using Firewatch.Infrastructure.Persistence;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;


namespace Firewatch.Application.UnitTests.Common.Services.NewUserServiceTests
{

    public class InitializeNewUser_Should
    {
        public InitializeNewUser_Should()
        {
            _db = ApplicationDbContextFactory.Create();
            sut = new NewUserService(NUnitTestLogger.Create<NewUserService>(), _db);
        }

        private readonly NewUserService sut;
        private readonly ApplicationDbContext _db;

        [Test]
        public async Task ReturnSuccess()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            _db.People.Add(person);
            await _db.SaveChangesAsync();

            var result = await sut.InitializeNewUser(person.Id);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
