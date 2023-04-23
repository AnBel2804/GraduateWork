using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Graduate_Work.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Core;
using System.Text.RegularExpressions;

namespace Graduate_Work.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = Roles.Role_Administrator)]
    public class PackageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var packages = _unitOfWork.Package.GetAll(null, "PackageType", "SenderInfo", "ReciverInfo");
            return View(packages);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchString)
        {
            var packages = _unitOfWork.Package.GetAll(null, "PackageType", "SenderInfo", "ReciverInfo");
            ViewData["SearchString"] = SearchString;
            var searchPackages = new List<Package>();
            if (!String.IsNullOrEmpty(SearchString))
            {
                foreach (var package in packages)
                { 
                    if (package.SenderInfo.Sender.User.UserName == SearchString)
                        searchPackages.Add(package);
                    if (package.ReciverInfo.Reciver.User.UserName == SearchString)
                        searchPackages.Add(package);
                }
                return View(searchPackages);
            }
            return View(packages);
        }

        public IActionResult Add()
        {
            return View(new GeneralNewPackageVM());
        }

        [HttpPost]
        public IActionResult Add(GeneralNewPackageVM generalNewPackageVM)
        {
            var customers = _unitOfWork.Customer.GetAll(null, "User");
            var departments = _unitOfWork.Department.GetAll(null, "Routes");
            var packageTypes = _unitOfWork.PackageType.GetAll();
            var routes = _unitOfWork.Route.GetAll(null, "Departments");

            if (string.IsNullOrEmpty(generalNewPackageVM.SenderPhone))
                ModelState.AddModelError(nameof(generalNewPackageVM.SenderPhone), "Номер телефону відправника є пустим");

            if (string.IsNullOrEmpty(Convert.ToString(generalNewPackageVM.SenderDepartmentId)))
                ModelState.AddModelError(nameof(generalNewPackageVM.SenderDepartmentId), "Відділення відправника є пустим");

            if (string.IsNullOrEmpty(generalNewPackageVM.ReciverPhone))
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverPhone), "Номер телефону отримувача є пустим");

            if (string.IsNullOrEmpty(Convert.ToString(generalNewPackageVM.ReciverDepartmentId)))
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverDepartmentId), "Відділення отримувача є пустим");

            if (string.IsNullOrEmpty(generalNewPackageVM.PackageTypeName))
                ModelState.AddModelError(nameof(generalNewPackageVM.PackageTypeName), "Назва типу є пустою");

            if (!string.IsNullOrEmpty(generalNewPackageVM.SenderPhone) && !Regex.IsMatch(generalNewPackageVM.SenderPhone, @"^\+380\d{9}$"))
                ModelState.AddModelError(nameof(generalNewPackageVM.SenderPhone), "Невірний формат номеру телефона відправника");

            if (generalNewPackageVM.SenderDepartmentId < 1)
                ModelState.AddModelError(nameof(generalNewPackageVM.SenderDepartmentId), "Номер відділення відправника не може бути меншим за 1");

            if (!string.IsNullOrEmpty(generalNewPackageVM.ReciverPhone) && !Regex.IsMatch(generalNewPackageVM.ReciverPhone, @"^\+380\d{9}$"))
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverPhone), "Невірний формат номеру телефона відправника");

            if (generalNewPackageVM.ReciverDepartmentId < 1)
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverDepartmentId), "Номер відділення отримувача не може бути меншим за 1");

            if (!string.IsNullOrEmpty(generalNewPackageVM.PackageTypeName) && generalNewPackageVM.PackageTypeName.Length < 3)
                ModelState.AddModelError(nameof(generalNewPackageVM.PackageTypeName), "Назва типу не можк бути коротшою за 3 символи");

            if (!string.IsNullOrEmpty(generalNewPackageVM.PackageTypeName) && generalNewPackageVM.PackageTypeName.Length > 15)
                ModelState.AddModelError(nameof(generalNewPackageVM.PackageTypeName), "Назва типу не може бути довшою за 15 символів");

            if (generalNewPackageVM.SenderPhone == generalNewPackageVM.ReciverPhone)
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverPhone), "Відправник і отримувач не можуть співпадати");

            if (generalNewPackageVM.SenderDepartmentId == generalNewPackageVM.ReciverDepartmentId)
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverDepartmentId), "Відділення відправника і відділення отримувача не можуть співпадати");

            if (customers.Count(c => c.Phone == generalNewPackageVM.SenderPhone) == 0)
                ModelState.AddModelError(nameof(generalNewPackageVM.SenderPhone), "Такого користувача не існує");

            if (departments.Count(c => c.NumberOfDepartment == generalNewPackageVM.SenderDepartmentId) == 0)
                ModelState.AddModelError(nameof(generalNewPackageVM.SenderDepartmentId), "Такого відділення не існує");

            if (customers.Count(c => c.Phone == generalNewPackageVM.ReciverPhone) == 0)
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverPhone), "Такого користувача не існує");

            if (departments.Count(c => c.NumberOfDepartment == generalNewPackageVM.ReciverDepartmentId) == 0)
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverDepartmentId), "Такого відділення не існує");

            if (packageTypes.Count(c => c.NameOfType == generalNewPackageVM.PackageTypeName) == 0)
                ModelState.AddModelError(nameof(generalNewPackageVM.PackageTypeName), "Такого типу посилки не існує");

            Models.Route searchRoute = null;
            foreach (var route in routes)
                if(route.Departments.First().NumberOfDepartment == generalNewPackageVM.SenderDepartmentId)
                    if(route.Departments.Last().NumberOfDepartment == generalNewPackageVM.ReciverDepartmentId)
                    {
                        searchRoute = route;
                        break;
                    }

            if (searchRoute == null)
                ModelState.AddModelError(nameof(generalNewPackageVM.SenderDepartmentId), "Такого маршруту не існує");

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Нову посилку не було створено";
                return View(generalNewPackageVM);
            }

            var newSenderInfo = new SenderInfo()
            {
                Sender = _unitOfWork.Customer.GetFirstOrDefault(c => c.Phone == generalNewPackageVM.SenderPhone),
                DepartmentOfSender = _unitOfWork.Department.GetFirstOrDefault(c => c.NumberOfDepartment == generalNewPackageVM.SenderDepartmentId)
            };

            _unitOfWork.SenderInfo.Add(newSenderInfo);

            var newReciverInfo = new ReciverInfo()
            {
                Reciver = _unitOfWork.Customer.GetFirstOrDefault(c => c.Phone == generalNewPackageVM.ReciverPhone),
                DepartmentOfReciver = _unitOfWork.Department.GetFirstOrDefault(c => c.NumberOfDepartment == generalNewPackageVM.ReciverDepartmentId)
            };

            _unitOfWork.ReciverInfo.Add(newReciverInfo);

            var searchPackageType = _unitOfWork.PackageType.GetFirstOrDefault(c => c.NameOfType == generalNewPackageVM.PackageTypeName);

            var newPackage = new Package()
            {
                SenderInfo = newSenderInfo,
                ReciverInfo = newReciverInfo,
                PackageType = searchPackageType,
                Status = Statuses.Status_Sent,
                Time = DateTime.Now + searchRoute.Time,
                Cost = searchRoute.Cost + (searchRoute.Cost * (searchPackageType.InterestRate / 100))
            };

            _unitOfWork.Package.Add(newPackage);
            _unitOfWork.Save();

            TempData["success"] = "Нову посилку успішно створено";
            return RedirectToAction("Index");
        }
    }
}
