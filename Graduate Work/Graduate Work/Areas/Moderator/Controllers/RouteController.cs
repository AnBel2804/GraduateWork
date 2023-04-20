using Graduate_Work.Repository.IRepository;
using Graduate_Work.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduate_Work.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    [Authorize(Roles = Roles.Role_Moderator)]
    public class RouteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RouteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var routes = _unitOfWork.Route.GetAll();
            return View(routes);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchString)
        {
            var departments = _unitOfWork.Route.GetAll();
            ViewData["SearchString"] = SearchString;
            if (!String.IsNullOrEmpty(SearchString))
                departments = departments.Where(c => c.Departments.First().NumberOfDepartment == int.Parse(SearchString)
                                                && c.Departments.Last().NumberOfDepartment == int.Parse(SearchString));
            return View(departments);
        }
    }
}
