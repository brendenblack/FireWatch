using Firewatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoList> TodoLists { get; set; }

        DbSet<TodoItem> TodoItems { get; set; }

        DbSet<Person> People { get;  }

        DbSet<Account> Accounts { get; }

        DbSet<ExpenseCategory> ExpenseCategories { get; }

        DbSet<TradeExecution> TradeExecutions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
