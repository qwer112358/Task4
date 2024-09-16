using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models.Entities;
using UserManagement.Services;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IUserService _userService;

		public AccountController(IAuthService authService, IUserService userService)
		{
			_authService = authService;
			_userService = userService;
		}

		public IActionResult Login() => View();

		public IActionResult Register() => View();

		public async Task<IActionResult> Logout()
		{
			await _authService.SignOutAsync();
			return RedirectToHome();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterView model) =>
			ModelState.IsValid
				? await TryRegisterUserAsync(_userService.CreateNewUser(model), model)
				: View(model);

		[HttpPost]
		public async Task<IActionResult> Login(LoginView model) =>
			ModelState.IsValid
				? await HandleLoginAsync(model)
				: View(model);

		[HttpPost]
		private async Task<IActionResult> HandleLoginAsync(LoginView model)
		{
			var user = await _authService.FindByEmailAsync(model.Email!);
			var errorMessage = ValidateUser(user);
			if (!string.IsNullOrEmpty(errorMessage))
				return ShowErrorsAndView(model, errorMessage);

			await _authService.UpdateLastLoginDateAsync(user!);
			return await HandleSignIn(model);
		}

		private async Task<IActionResult> HandleSignIn(LoginView model)
		{
			var signInResult = await _authService.SignInAsync(model);
			return signInResult.Succeeded
				? RedirectToHome()
				: HandleFailedSignIn(signInResult, model);
		}

		private IActionResult HandleFailedSignIn(Microsoft.AspNetCore.Identity.SignInResult result, LoginView model) =>
			result.IsLockedOut
				? View("Lockout")
				: ShowErrorsAndView(model, "Invalid login attempt");

		private async Task<IActionResult> TryRegisterUserAsync(ApplicationUser user, RegisterView model)
		{
			try
			{
				var result = await _userService.RegisterUserAsync(user, model.Password!);
				return await ProcessRegistrationResultAsync(result, user, model);
			}
			catch (DbUpdateException)
			{
				return ShowErrorsAndView(model, "An account with this email address already exists.");
			}
		}

		private async Task<IActionResult> ProcessRegistrationResultAsync(IdentityResult result, ApplicationUser user, RegisterView model) =>
			result.Succeeded
				? await SignInAndRedirectAsync(user, model.Password!)
				: ShowErrorsAndView(model, result.Errors.Select(e => e.Description).ToArray());

		private async Task<IActionResult> SignInAndRedirectAsync(ApplicationUser user, string password)
		{
			await _authService.SignInAsync(new LoginView { Email = user.Email, Password = password, RememberMe = false });
			return RedirectToHome();
		}

		private IActionResult ShowErrorsAndView<TModel>(TModel model, params string[] errors)
		{
			foreach (var error in errors)
				ModelState.AddModelError(string.Empty, error);
			return View(model);
		}

		private string ValidateUser(ApplicationUser? user) =>
			user switch
			{
				null => "Invalid login attempt",
				{ IsBlocked: true } => "Your account is blocked.",
				_ => string.Empty,
			};

		private IActionResult RedirectToHome() => RedirectToAction(nameof(HomeController.Index), "Home");
	}
}