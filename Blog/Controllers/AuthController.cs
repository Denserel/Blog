
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IMapper mapper;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender,
            IMapper mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.mapper = mapper;
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
        
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = mapper.Map<RegisterViewModel, IdentityUser>(registerViewModel);

                var result = await userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    var message = "Welcome to bobs blog";

                    await emailSender.SendEmailAsync(registerViewModel.Email, "Welcome", message);
                    await signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(registerViewModel);
        }
    }
}
