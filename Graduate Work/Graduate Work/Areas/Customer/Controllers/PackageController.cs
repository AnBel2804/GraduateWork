﻿using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Graduate_Work.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Graduate_Work.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = Roles.Role_Customer)]
    public class PackageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var packages = _unitOfWork.Package.GetAll(null, "PackageType", "SenderInfo", "ReciverInfo");
            var customer = _unitOfWork.Customer.GetFirstOrDefault(c =>
                    c.User.Id == claim.Value, "User");

            var packagesOfUser = new List<Package>();
            foreach (var package in packages)
            {
                if (package.SenderInfo.Sender == customer)
                    packagesOfUser.Add(package);
                if (package.ReciverInfo.Reciver == customer)
                    packagesOfUser.Add(package);
            }
            return View(packagesOfUser);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchString)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var packages = _unitOfWork.Package.GetAll(null, "PackageType", "SenderInfo", "ReciverInfo");
            var customer = _unitOfWork.Customer.GetFirstOrDefault(c =>
                    c.User.Id == claim.Value, "User");

            var packagesOfUser = new List<Package>();
            foreach (var package in packages)
            {
                if (package.SenderInfo.Sender == customer)
                    packagesOfUser.Add(package);
                if (package.ReciverInfo.Reciver == customer)
                    packagesOfUser.Add(package);
            }

            var searchPackages = new List<Package>();
            if (!String.IsNullOrEmpty(SearchString))
            {
                foreach (var package in packagesOfUser)
                {
                    if (package.TTN == SearchString)
                        searchPackages.Add(package);
                }
                return View(searchPackages);
            }

            return View(packagesOfUser);
        }

        public IActionResult Add()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var thisCustomer = _unitOfWork.Customer.GetFirstOrDefault(c =>
                    c.User.Id == claim.Value, "User");

            GeneralNewPackageVM generalNewPackageVM = new GeneralNewPackageVM();
            generalNewPackageVM.SenderPhone = thisCustomer.Phone;

            return View(generalNewPackageVM);
        }

        [HttpPost]
        public IActionResult Add(GeneralNewPackageVM generalNewPackageVM)
        {
            var customers = _unitOfWork.Customer.GetAll(null, "User");
            var departments = _unitOfWork.Department.GetAll(null, "Routes");
            var packageTypes = _unitOfWork.PackageType.GetAll();
            var routes = _unitOfWork.Route.GetAll(null, "Departments");

            if (string.IsNullOrEmpty(Convert.ToString(generalNewPackageVM.SenderDepartmentId)))
                ModelState.AddModelError(nameof(generalNewPackageVM.SenderDepartmentId), "Відділення відправника є пустим");

            if (string.IsNullOrEmpty(generalNewPackageVM.ReciverPhone))
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverPhone), "Номер телефону отримувача є пустим");

            if (string.IsNullOrEmpty(Convert.ToString(generalNewPackageVM.ReciverDepartmentId)))
                ModelState.AddModelError(nameof(generalNewPackageVM.ReciverDepartmentId), "Відділення отримувача є пустим");

            if (string.IsNullOrEmpty(generalNewPackageVM.PackageTypeName))
                ModelState.AddModelError(nameof(generalNewPackageVM.PackageTypeName), "Назва типу є пустою");

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
                if (route.Departments.First().NumberOfDepartment == generalNewPackageVM.SenderDepartmentId)
                    if (route.Departments.Last().NumberOfDepartment == generalNewPackageVM.ReciverDepartmentId)
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

            Random rnd = new Random();

            var newPackage = new Package()
            {
                TTN = Convert.ToString(rnd.Next(400000000, 499999999)),
                SenderInfo = newSenderInfo,
                ReciverInfo = newReciverInfo,
                PackageType = searchPackageType,
                Status = Statuses.Status_Await,
                Time = DateTime.Now + searchRoute.Time,
                Cost = searchRoute.Cost + (searchRoute.Cost * (searchPackageType.InterestRate / 100))
            };

            _unitOfWork.Package.Add(newPackage);
            _unitOfWork.Save();

            TempData["success"] = "Нову посилку успішно створено";
            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            var searchPackage = _unitOfWork.Package.GetFirstOrDefault(c =>
                    c.PackageId == id, "PackageType", "SenderInfo", "ReciverInfo");

            var searchPackageType = _unitOfWork.PackageType.GetFirstOrDefault(c =>
                    c.PackageTypeId == searchPackage.PackageType.PackageTypeId);

            var searchSenderInfo = _unitOfWork.SenderInfo.GetFirstOrDefault(c =>
                    c.SenderInfoId == searchPackage.SenderInfo.SenderInfoId, "Sender", "DepartmentOfSender");

            var searchReciverInfo = _unitOfWork.ReciverInfo.GetFirstOrDefault(c =>
                    c.ReciverInfoId == searchPackage.ReciverInfo.ReciverInfoId, "Reciver", "DepartmentOfReciver");

            var searchSender = _unitOfWork.Customer.GetFirstOrDefault(c =>
                    c.CustomerId == searchSenderInfo.Sender.CustomerId);

            var searchReciver = _unitOfWork.Customer.GetFirstOrDefault(c =>
                    c.CustomerId == searchReciverInfo.Reciver.CustomerId);

            var searchDepartmentOfSender = _unitOfWork.Department.GetFirstOrDefault(c =>
                    c.DepartmentId == searchSenderInfo.DepartmentOfSender.DepartmentId);

            var searchDepartmentOfReciver = _unitOfWork.Department.GetFirstOrDefault(c =>
                    c.DepartmentId == searchReciverInfo.DepartmentOfReciver.DepartmentId);

            var ganeralInfo = new GaneralInfo()
            {
                Package = searchPackage,
                PackageType = searchPackageType,
                Sender = searchSender,
                Reciver = searchReciver,
                SenderDepartment = searchDepartmentOfSender,
                ReciverDepartment = searchDepartmentOfReciver
            };

            return View(ganeralInfo);
        }
    }
}
