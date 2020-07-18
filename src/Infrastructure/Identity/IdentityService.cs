using Firewatch.Application.Common.Interfaces;
using Firewatch.Application.Common.Models;
using Firewatch.Domain.Entities;
using Firewatch.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Firewatch.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly INewUserService _newUserService;

        public IdentityService(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context,
            INewUserService newUserService)
        {
            _userManager = userManager;
            _context = context;
            _newUserService = newUserService;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }
        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            await InitializePersonForUser(user);

            return (result.ToApplicationResult(), user.Id);
        }

        /// <summary>
        /// Ensures that a <see cref="Person"/> record has been created as well as setting up
        /// basic account features like <see cref="ExpenseCategory" /> listings.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> InitializePersonForUser(ApplicationUser user)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Id == user.Id);
            if (person == null)
            {
                person = new Person() { Id = user.Id };
                _context.Add(person);
                await _context.SaveChangesAsync();
            }

            await _newUserService.InitializeNewUser(person.Id);

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<bool> IsUserInRole(string userId, string role)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}
