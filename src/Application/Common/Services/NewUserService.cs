using Firewatch.Application.Common.Interfaces;
using Firewatch.Application.Common.Models;
using Firewatch.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Common.Services
{
    public class NewUserService : INewUserService
    {
        private readonly ILogger<NewUserService> _logger;
        private readonly IApplicationDbContext _context;

        public NewUserService(
            ILogger<NewUserService> logger,
            IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result> InitializeNewUser(string personId, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Initializing new person with id {}", personId);
            _context.People.Add(new Person() { Id = personId });
            await _context.SaveChangesAsync(cancellationToken);


            // We have to fetch the Person object from the context we're adding to, otherwise EF
            // will try to save it
            var accountOwner = _context.People.First(p => p.Id == personId);
            var cashAccount = new CashAccount(accountOwner);
            _context.Accounts.Add(cashAccount);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogDebug("Created cash account {} for user {}",
                    cashAccount.Id,
                    accountOwner.Id);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "An error occurred while initializing bank accounts for user {}", accountOwner.Id);
                return Result.Failure(e.Message);
            }

            // We have to fetch the Person object from the context we're adding to, otherwise EF
            // will try to save it again
            var expenseOwner = _context.People.First(p => p.Id == personId);
            var household = new ExpenseCategory(expenseOwner, "Household", 1000, "#B83211");
            household.AddChildCategory("Rent", 900);
            var food = new ExpenseCategory(expenseOwner, "Food", 500, "#238C2C");
            food.AddChildCategory("Groceries", 400);
            food.AddChildCategory("Restaurants", 100);
            _context.ExpenseCategories.AddRange(household, food);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "An error occurred while initializing expense categories for user {}", expenseOwner.Id);
                return Result.Failure(e.Message);
            }

            return Result.Success();
        }


    }
}
