
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;

namespace Blog.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        
        private readonly IMapper mapper;
        private readonly IEmailService emailService;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            IEmailService emailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.emailService = emailService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new AuthUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthUserViewModel authUserViewModel)
        {
            var result = await signInManager.PasswordSignInAsync(authUserViewModel.UserName, authUserViewModel.Password, false, false);

            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(authUserViewModel);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = mapper.Map<RegisterViewModel, IdentityUser>(registerViewModel);

                var result = await userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    var confermationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var varifyLink = Url.Action(nameof(VarifyEmail),
                        this.ControllerContext.RouteData.Values["controller"].ToString(), 
                        new { userId = user.Id, confermationToken }, 
                        Request.Scheme, 
                        Request.Host.ToString());

                    await emailService.SendAsync(user.Email, "Varify Email", $"<a href=\"{varifyLink}\">Varefy Email</a>", true);

                    return RedirectToAction("EmailVarification");
                }
            }

            return View(registerViewModel);
        }

        public async Task<IActionResult> VarifyEmail (string userId, string confermationToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            
            if (user == null) return BadRequest();
            
            await userManager.ConfirmEmailAsync(user, confermationToken);
            await signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EmailVarification() => View();
    }
}
