using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Graduate_Work.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchPackage(string? SearchString)
        {
            var packages = _unitOfWork.Package.GetAll(null, "PackageType", "SenderInfo", "ReciverInfo");
            ViewData["SearchString"] = SearchString;
            var searchPackage = new Package();
            if (!String.IsNullOrEmpty(SearchString))
            {
                searchPackage = packages.FirstOrDefault(c => c.TTN == SearchString);
                if (searchPackage != null)
                    return View(searchPackage);
                else
                {
                    TempData["error"] = "Посилку не було знайдено";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
