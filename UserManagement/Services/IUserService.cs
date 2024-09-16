using Microsoft.AspNetCore.Identity;
using UserManagement.Models.Entities;
using UserManagement.ViewModels;

namespace UserManagement.Services
{
	public interface IUserService
	{
		ApplicationUser CreateNewUser(RegisterView model);
		Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password);
	}
}
