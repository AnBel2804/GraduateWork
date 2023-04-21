using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Graduate_Work.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduate_Work.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = Roles.Role_Administrator)]
    public class PackageTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var packageTypes = _unitOfWork.PackageType.GetAll();
            return View(packageTypes);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchString)
        {
            var packageTypes = _unitOfWork.PackageType.GetAll();
            ViewData["SearchString"] = SearchString;
            if (!String.IsNullOrEmpty(SearchString))
                packageTypes = packageTypes.Where(c => c.NameOfType == SearchString);
            return View(packageTypes);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(PackageType packageType)
        {
            if (string.IsNullOrEmpty(packageType.NameOfType))
                ModelState.AddModelError(nameof(packageType.NameOfType), "Назва типу є пустою");

            if (string.IsNullOrEmpty(Convert.ToString(packageType.InterestRate)))
                ModelState.AddModelError(nameof(packageType.InterestRate), "Відсоткова ставка є пустою");

            if (!string.IsNullOrEmpty(packageType.NameOfType) && packageType.NameOfType.Length < 3)
                ModelState.AddModelError(nameof(packageType.NameOfType), "Назва типу не можк бути коротшою за 3 символи");

            if (!string.IsNullOrEmpty(packageType.NameOfType) && packageType.NameOfType.Length > 15)
                ModelState.AddModelError(nameof(packageType.NameOfType), "Назва типу не може бути довшою за 15 символів");

            if (!string.IsNullOrEmpty(Convert.ToString(packageType.InterestRate)) && packageType.InterestRate < 0)
                ModelState.AddModelError(nameof(packageType.InterestRate), "Відсоткова ставка не може бути меншою за 0");

            if (ModelState.IsValid)
            {
                _unitOfWork.PackageType.Add(packageType);
                _unitOfWork.Save();
                TempData["success"] = "Новий тип успішно додано";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Новий тип не було додано";
            return View(packageType);
        }
    }
}
