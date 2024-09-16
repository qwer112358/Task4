using Microsoft.AspNetCore.Identity;
using UserManagement.Models.Entities;
using UserManagement.ViewModels;

namespace UserManagement.Services
{
	public class AuthService : IAuthService
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public async Task<SignInResult> SignInAsync(LoginView model) =>
			await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);

		public async Task SignOutAsync() => await _signInManager.SignOutAsync();

		public async Task<ApplicationUser?> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

		public async Task UpdateLastLoginDateAsync(ApplicationUser user)
		{
			user.LastLoginDate = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);
		}
	}
}
