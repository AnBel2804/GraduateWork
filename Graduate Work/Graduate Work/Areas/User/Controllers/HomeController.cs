using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Graduate_Work.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(
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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View(new LoginVM());
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

            if (string.IsNullOrEmpty(registerVM.Email))
                ModelState.AddModelError(nameof(registerVM.Email), "E-mail є пустим");

            if (!string.IsNullOrEmpty(registerVM.Email) && !Regex.IsMatch(registerVM.Email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                ModelState.AddModelError(nameof(registerVM.Email), "Невірний формат E-mail");

            var user = await _userManager.FindByEmailAsync(registerVM.Email);
            if (!string.IsNullOrEmpty(registerVM.Email) && user != null)
                ModelState.AddModelError(nameof(registerVM.Email), "Ця пошта вже використовується");

            if (string.IsNullOrEmpty(registerVM.Password))
                ModelState.AddModelError(nameof(registerVM.Password), "Пароль є пустим");

            if (!string.IsNullOrEmpty(registerVM.Password) && registerVM.Password.Length < 8)
                ModelState.AddModelError(nameof(registerVM.Password), "Пароль має містити не менше 8 символів");

            if (string.IsNullOrEmpty(registerVM.ConfirmPassword))
                ModelState.AddModelError(nameof(registerVM.ConfirmPassword), "Підтвердження паролю є пустим");

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
                await _userManager.AddToRoleAsync(newUser, Utility.Roles.Role_User);
                var customer = new Customer()
                {
                    User = newUser
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
    }
}
