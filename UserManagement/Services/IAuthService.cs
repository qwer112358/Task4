using Microsoft.AspNetCore.Identity;
using UserManagement.Models.Entities;
using UserManagement.ViewModels;

namespace UserManagement.Services
{
	public interface IAuthService
	{
		Task<SignInResult> SignInAsync(LoginView model);
		Task SignOutAsync();
		Task<ApplicationUser?> FindByEmailAsync(string email);
		Task UpdateLastLoginDateAsync(ApplicationUser user);
	}
}
