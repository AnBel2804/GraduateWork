using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Graduate_Work.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
