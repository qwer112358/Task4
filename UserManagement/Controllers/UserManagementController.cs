using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserManagement.Models.Entities;

namespace UserManagement.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccount _accountManager;

        public UserManagementController(UserManager<ApplicationUser> userManager, IAccount accountManager)
        {
            _userManager = userManager;
            _accountManager = accountManager;
        }

        public async Task<IActionResult> Index() => View(await _userManager.Users.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> BlockUsers(string[] userIds)
        {
            foreach (var id in userIds)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is not null)
                {
					user.IsBlocked = true;
                    await _userManager.UpdateAsync(user);
                    if (IsCurrentUser(user))
                        await _accountManager.Logout();
                }
            }
            return RedirectToHome();
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUsers(string[] userIds)
        {
            foreach (var id in userIds)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is not null)
                {
					user.IsBlocked = false;
					await _userManager.UpdateAsync(user);
				}
            }
            return RedirectToHome();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(string[] userIds)
        {
            foreach (var id in userIds)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is not null)
					await _userManager.DeleteAsync(user);
            }
            return RedirectToHome();
        }

        private IActionResult RedirectToHome() => RedirectToAction("Index");

        private bool IsCurrentUser(ApplicationUser user) => User.FindFirstValue(ClaimTypes.NameIdentifier) == user.Id;
	}
}
