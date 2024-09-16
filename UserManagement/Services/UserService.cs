using Microsoft.AspNetCore.Identity;
using UserManagement.Models.Entities;
using UserManagement.ViewModels;

namespace UserManagement.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserService(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public ApplicationUser CreateNewUser(RegisterView model) => new ApplicationUser
		{
			Name = model.Name,
			UserName = model.Email,
			Email = model.Email,
			RegistrationDate = DateTime.UtcNow,
			LastLoginDate = DateTime.UtcNow
		};

		public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password) =>
			await _userManager.CreateAsync(user, password);
	}
}
