using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Graduate_Work.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    }
}
