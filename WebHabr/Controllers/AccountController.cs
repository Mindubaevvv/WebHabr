using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebHabr.Models;

namespace WebHabr.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Manage()
        {
            var user = _userManager.GetUserAsync(User).Result;
            return View(user); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            // Выйти из системы
            await _signInManager.SignOutAsync();

            // Если returnUrl не был передан, перенаправить на главную страницу
            returnUrl ??= Url.Action("Index", "Home");

            // Перенаправить на returnUrl (основная страница или другая)
            return Redirect(returnUrl);
        }



        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Roles = new[] { "User", "Author", "Admin" };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));

                    await _userManager.AddToRoleAsync(user, model.Role);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            ViewBag.Roles = new[] { "User", "Author", "Admin" };
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
            return View(model);
        }

    }
}
