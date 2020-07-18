using Firewatch.Application.Common.Models;
using System.Threading.Tasks;

namespace Firewatch.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);

        Task<bool> IsUserInRole(string userId, string role);
    }
}
