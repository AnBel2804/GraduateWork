using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Graduate_Work.Areas.User.Controllers
{
    [Area("User")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Login()
        {
            return View(new LoginVM());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM logimVM)
        {
            if (string.IsNullOrEmpty(logimVM.Email))
                ModelState.AddModelError(nameof(logimVM.Email), "E-mail є пустим");

            if (!string.IsNullOrEmpty(logimVM.Email) && !Regex.IsMatch(logimVM.Email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                ModelState.AddModelError(nameof(logimVM.Email), "Невірний формат E-mail");

            if (string.IsNullOrEmpty(logimVM.Password))
                ModelState.AddModelError(nameof(logimVM.Password), "Пароль є пустим");

            if (!string.IsNullOrEmpty(logimVM.Password) && logimVM.Password.Length < 8)
                ModelState.AddModelError(nameof(logimVM.Password), "Пароль має містити не менше 8 символів");

            if (!ModelState.IsValid) return View(logimVM);

            var user = await _userManager.FindByEmailAsync(logimVM.Email);

            if (user != null)
            {
                var passCheck = await _userManager.CheckPasswordAsync(user, logimVM.Password);
                if (passCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, logimVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Error"] = "Помилка входу! Перевірте дані";
                return View(logimVM);
            }

            TempData["Error"] = "Користувача не знайдено! Перевірте дані";
            return View(logimVM);
        }
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (string.IsNullOrEmpty(registerVM.Login))
                ModelState.AddModelError(nameof(registerVM.Login), "Логін є пустим");

            if (string.IsNullOrEmpty(registerVM.FirstName))
                ModelState.AddModelError(nameof(registerVM.FirstName), "Ім`я є пустим");

            if (string.IsNullOrEmpty(registerVM.LastName))
                ModelState.AddModelError(nameof(registerVM.LastName), "Прізвище є пустим");

            if (string.IsNullOrEmpty(registerVM.Email))
                ModelState.AddModelError(nameof(registerVM.Email), "E-mail є пустим");

            if (string.IsNullOrEmpty(registerVM.Password))
                ModelState.AddModelError(nameof(registerVM.Password), "Пароль є пустим");

            if (string.IsNullOrEmpty(registerVM.ConfirmPassword))
                ModelState.AddModelError(nameof(registerVM.ConfirmPassword), "Підтвердження паролю є пустим");

            if (!string.IsNullOrEmpty(registerVM.Login) && registerVM.Login.Length < 4)
                ModelState.AddModelError(nameof(registerVM.Login), "Логін не може бути коротшим за 4 символи");

            if (!string.IsNullOrEmpty(registerVM.Login) && registerVM.Login.Length > 25)
                ModelState.AddModelError(nameof(registerVM.Login), "Логін не може бути довшим за 25 символів");

            if (!string.IsNullOrEmpty(registerVM.FirstName) && registerVM.FirstName.Length < 4)
                ModelState.AddModelError(nameof(registerVM.FirstName), "Ім`я не може бути коротшим за 4 символи");

            if (!string.IsNullOrEmpty(registerVM.FirstName) && registerVM.FirstName.Length > 25)
                ModelState.AddModelError(nameof(registerVM.FirstName), "Ім`я не може бути довшим за 25 символів");

            if (!string.IsNullOrEmpty(registerVM.LastName) && registerVM.LastName.Length < 4)
                ModelState.AddModelError(nameof(registerVM.LastName), "Прізвище не може бути коротшим за 4 символи");

            if (!string.IsNullOrEmpty(registerVM.LastName) && registerVM.LastName.Length > 25)
                ModelState.AddModelError(nameof(registerVM.LastName), "Прізвище не може бути довшим за 25 символів");

            if (!string.IsNullOrEmpty(registerVM.Password) && registerVM.Password.Length < 8)
                ModelState.AddModelError(nameof(registerVM.Password), "Пароль має містити не менше 8 символів");

            if (!string.IsNullOrEmpty(registerVM.Email) && !Regex.IsMatch(registerVM.Email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                ModelState.AddModelError(nameof(registerVM.Email), "Невірний формат E-mail");

            var user = await _userManager.FindByEmailAsync(registerVM.Email);
            if (!string.IsNullOrEmpty(registerVM.Email) && user != null)
                ModelState.AddModelError(nameof(registerVM.Email), "Ця пошта вже використовується");

            if (!string.IsNullOrEmpty(registerVM.ConfirmPassword) && registerVM.Password != registerVM.ConfirmPassword)
                ModelState.AddModelError(nameof(registerVM.ConfirmPassword), "Паролі не співпадають");

            if (!ModelState.IsValid) return View(registerVM);

            var newUser = new IdentityUser()
            {
                Email = registerVM.Email,
                UserName = registerVM.Login
            };

            var newUserResponce = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponce.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, Utility.Roles.Role_Customer);
                var customer = new Models.Customer()
                {
                    User = newUser,
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName
                };
                _unitOfWork.Customer.Add(customer);
                _unitOfWork.Save();
                return View("SuccessRegistration");
            }
            else
            {
                ModelState.AddModelError(nameof(registerVM.Login), "Користувача з таким логіном вже зареєстровано");
                return View(registerVM);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
