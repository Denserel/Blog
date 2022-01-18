using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public AuthController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new AuthUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthUserViewModel authUserViewModel)
        {
            var result = await signInManager.PasswordSignInAsync(authUserViewModel.UserName, authUserViewModel.Password, false, false);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
