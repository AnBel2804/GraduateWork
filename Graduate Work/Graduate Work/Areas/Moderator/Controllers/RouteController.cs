using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Graduate_Work.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

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
            var routes = _unitOfWork.Route.GetAll(null, "Departments");
            return View(routes);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchString)
        {
            var departments = _unitOfWork.Route.GetAll(null, "Departments");
            ViewData["SearchString"] = SearchString;
            if (!String.IsNullOrEmpty(SearchString))
                departments = departments.Where(c => c.Departments.First().NumberOfDepartment == int.Parse(SearchString)
                                                && c.Departments.Last().NumberOfDepartment == int.Parse(SearchString));
            return View(departments);
        }

        public IActionResult Add()
        {
            return View(new RouteVM());
        }

        [HttpPost]
        public IActionResult Add(RouteVM routeVM)
        {
            TimeSpan maxTime = new TimeSpan(120, 0, 0);
            TimeSpan minTime = new TimeSpan(0, 30, 0);
            var departments = _unitOfWork.Department.GetAll(null);

            if (string.IsNullOrEmpty(Convert.ToString(routeVM.NumberOfSendingDepartment)))
                ModelState.AddModelError(nameof(routeVM.NumberOfSendingDepartment), "Відділення відправлення є пустим");

            if (string.IsNullOrEmpty(Convert.ToString(routeVM.NumberOfReceivingDepartment)))
                ModelState.AddModelError(nameof(routeVM.NumberOfReceivingDepartment), "Відділення прибуття є пустим");

            if (string.IsNullOrEmpty(Convert.ToString(routeVM.Cost)))
                ModelState.AddModelError(nameof(routeVM.Cost), "Вартість є пустою");

            if (string.IsNullOrEmpty(Convert.ToString(routeVM.Time)))
                ModelState.AddModelError(nameof(routeVM.Time), "Час проходження є пустим");

            if (routeVM.NumberOfSendingDepartment < 1)
                ModelState.AddModelError(nameof(routeVM.NumberOfSendingDepartment), "Номер відділення не може бути меншим за 1");

            if (routeVM.NumberOfReceivingDepartment < 1)
                ModelState.AddModelError(nameof(routeVM.NumberOfReceivingDepartment), "Номер відділення не може бути меншим за 1");

            if (routeVM.NumberOfSendingDepartment == routeVM.NumberOfReceivingDepartment)
                ModelState.AddModelError(nameof(routeVM.NumberOfReceivingDepartment), "Відділення відправлення і відділення прибуття не можуть співпадати");

            if (departments.Count(c => c.NumberOfDepartment == routeVM.NumberOfSendingDepartment) == 0)
                ModelState.AddModelError(nameof(routeVM.NumberOfSendingDepartment), "Такого відділення не існує");

            if (departments.Count(c => c.NumberOfDepartment == routeVM.NumberOfReceivingDepartment) == 0)
                ModelState.AddModelError(nameof(routeVM.NumberOfReceivingDepartment), "Такого відділення не існує");

            if (routeVM.Cost < 10)
                ModelState.AddModelError(nameof(routeVM.Cost), "Вартість не може бути меншою за 10грн");

            if (routeVM.Cost > 1000)
                ModelState.AddModelError(nameof(routeVM.Cost), "Вартість не може бути більшою за 1000грн");

            if (routeVM.Time < minTime)
                ModelState.AddModelError(nameof(routeVM.Time), "Час проходження не може бути меншим за 30 хвилин");

            if (routeVM.Time > maxTime)
                ModelState.AddModelError(nameof(routeVM.Time), "Час проходження не може бути більшим за 5 днів");

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Новий маршрут не було додано";
                return View(routeVM);
            }

            var department1 = _unitOfWork.Department.GetFirstOrDefault(c => c.NumberOfDepartment == routeVM.NumberOfSendingDepartment);
            var department2 = _unitOfWork.Department.GetFirstOrDefault(c => c.NumberOfDepartment == routeVM.NumberOfReceivingDepartment);

            var listOfDepartments = new List<Department>();
            listOfDepartments.Add(department1); listOfDepartments.Add(department2);

            var newRoute = new Models.Route(){Departments = listOfDepartments,Cost = routeVM.Cost,Time = routeVM.Time};

            _unitOfWork.Route.Add(newRoute);
            _unitOfWork.Save();

            TempData["success"] = "Новий маршрут успішно додано";
            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var routeFromDbFirst = _unitOfWork.Route.GetFirstOrDefault(u => u.RouteId == id);
            if (routeFromDbFirst == null)
            {
                return NotFound();
            }

            return View(routeFromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Models.Route route)
        {
            TimeSpan maxTime = new TimeSpan(120, 0, 0);
            TimeSpan minTime = new TimeSpan(0, 30, 0);

            if (string.IsNullOrEmpty(Convert.ToString(route.Cost)))
                ModelState.AddModelError(nameof(route.Cost), "Вартість є пустою");

            if (string.IsNullOrEmpty(Convert.ToString(route.Time)))
                ModelState.AddModelError(nameof(route.Time), "Час проходження є пустим");

            if (route.Cost < 10)
                ModelState.AddModelError(nameof(route.Cost), "Вартість не може бути меншою за 10грн");

            if (route.Cost > 1000)
                ModelState.AddModelError(nameof(route.Cost), "Вартість не може бути більшою за 1000грн");

            if (route.Time < minTime)
                ModelState.AddModelError(nameof(route.Time), "Час проходження не може бути меншим за 30 хвилин");

            if (route.Time > maxTime)
                ModelState.AddModelError(nameof(route.Time), "Час проходження не може бути більшим за 5 днів");

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Інформація про маршрут не була відредагована";
                return View(route);
            }

            _unitOfWork.Route.Update(route);
            _unitOfWork.Save();

            TempData["success"] = "Інформація про маршрут успішно відредагована";
            return RedirectToAction("Index");
        }
    }
}
