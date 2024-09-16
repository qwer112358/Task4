using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models.Entities;

namespace UserManagement.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                {
					await _userManager.DeleteAsync(user);
				}
            }
            return RedirectToHome();
        }

        private IActionResult RedirectToHome() => RedirectToAction("Index");

	}


}
