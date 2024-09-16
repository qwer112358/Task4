using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    public interface IAccount
    {
        public IActionResult Login();
        public IActionResult Register();
        public Task<IActionResult> Logout();
    }
}
