using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;
using Graduate_Work.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Graduate_Work.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    [Authorize(Roles = Roles.Role_Moderator)]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var departments = _unitOfWork.Department.GetAll();
            return View(departments);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchString)
        {
            var departments = _unitOfWork.Department.GetAll();
            ViewData["SearchString"] = SearchString;
            if (!String.IsNullOrEmpty(SearchString))
                departments = departments.Where(c => c.City == SearchString);
            return View(departments);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Department department)
        {
            if (string.IsNullOrEmpty(department.City))
                ModelState.AddModelError(nameof(department.City), "Назва міста не може бути пустою");

            if (string.IsNullOrEmpty(department.NumberOfDepartment.ToString()))
                ModelState.AddModelError(nameof(department.NumberOfDepartment), "Номер відділення не може бути пустим");

            if (!string.IsNullOrEmpty(department.NumberOfDepartment.ToString()))
                if (department.NumberOfDepartment < 1)
                    ModelState.AddModelError(nameof(department.NumberOfDepartment), "Номер відділення не може бути меншим за 1");

            if (!string.IsNullOrEmpty(department.NumberOfDepartment.ToString()))
                if (_unitOfWork.Department.GetAll().Count(c => c.NumberOfDepartment == department.NumberOfDepartment) > 0)
                    ModelState.AddModelError(nameof(department.NumberOfDepartment), "Вже існує таке відділення");

            //if (!string.IsNullOrEmpty(department.City))
            //{
            //    IEnumerable<Department> departments_in_city = _unitOfWork.Department.GetAll(c => c.City == department.City);
            //    if (departments_in_city.Count(c => c.NumberOfDepartment == department.NumberOfDepartment) > 0)
            //        ModelState.AddModelError(nameof(department.NumberOfDepartment), "В цьому місті вже існує таке відділення");
            //}

            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Add(department);
                _unitOfWork.Save();
                TempData["success"] = "Нове відділення успішно додано";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Нове відділення не було додано";
            return View(department);
        }

        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var departmentFromDbFirst = _unitOfWork.Department.GetFirstOrDefault(u => u.DepartmentId == id);
            if (departmentFromDbFirst == null)
            {
                return NotFound();
            }

            return View(departmentFromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Department department)
        {
            if (string.IsNullOrEmpty(department.City))
                ModelState.AddModelError(nameof(department.City), "Назва міста не може бути пустою");

            //if (string.IsNullOrEmpty(department.NumberOfDepartment.ToString()))
            //    ModelState.AddModelError(nameof(department.NumberOfDepartment), "Номер відділення не може бути пустим");

            //if (!string.IsNullOrEmpty(department.NumberOfDepartment.ToString()))
            //    if (department.NumberOfDepartment < 1)
            //        ModelState.AddModelError(nameof(department.NumberOfDepartment), "Номер відділення не може бути меншим за 1");

            //if (!string.IsNullOrEmpty(department.City))
            //{
            //    if (_unitOfWork.Department.GetAll(c => c.City == department.City).Where(c => c.NumberOfDepartment == department.NumberOfDepartment 
            //                                    && c.DepartmentId != department.DepartmentId).Count() > 0)
            //        ModelState.AddModelError(nameof(department.NumberOfDepartment), "В цьому місті вже існує таке відділення");
            //}

            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Update(department);
                _unitOfWork.Save();
                TempData["success"] = "Інформація про відділення успішно відредагована";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Інформація про відділення не була відредагована";
            return View(department);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var departmentFromDbFirst = _unitOfWork.Department.GetFirstOrDefault(u => u.DepartmentId == id);
            if (departmentFromDbFirst == null)
            {
                return NotFound();
            }
            _unitOfWork.Department.Remove(departmentFromDbFirst);
            _unitOfWork.Save();
            TempData["success"] = "Відділення успішно видалено";
            return RedirectToAction("Index");
        }
    }
}
