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
    }
}
