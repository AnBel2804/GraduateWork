using Graduate_Work.Models;
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
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var customer = _unitOfWork.Customer.GetFirstOrDefault(c =>
                    c.User.Id == claim.Value, "User");

            return View(customer);
        }

        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var customerFromDbFirst = _unitOfWork.Customer.GetFirstOrDefault(c => c.CustomerId == id);
            if (customerFromDbFirst == null)
            {
                return NotFound();
            }

            return View(customerFromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Models.Customer customer)
        {
            if (string.IsNullOrEmpty(customer.FirstName))
                ModelState.AddModelError(nameof(customer.FirstName), "Ім`я є пустим");

            if (string.IsNullOrEmpty(customer.LastName))
                ModelState.AddModelError(nameof(customer.LastName), "Прізвище є пустим");

            if (!string.IsNullOrEmpty(customer.FirstName) && customer.FirstName.Length < 4)
                ModelState.AddModelError(nameof(customer.FirstName), "Ім`я не може бути коротшим за 4 символи");

            if (!string.IsNullOrEmpty(customer.FirstName) && customer.FirstName.Length > 25)
                ModelState.AddModelError(nameof(customer.FirstName), "Ім`я не може бути довшим за 25 символів");

            if (!string.IsNullOrEmpty(customer.LastName) && customer.LastName.Length < 4)
                ModelState.AddModelError(nameof(customer.LastName), "Прізвище не може бути коротшим за 4 символи");

            if (!string.IsNullOrEmpty(customer.LastName) && customer.LastName.Length > 25)
                ModelState.AddModelError(nameof(customer.LastName), "Прізвище не може бути довшим за 25 символів");

            if (ModelState.IsValid)
            {
                _unitOfWork.Customer.Update(customer);
                _unitOfWork.Save();
                TempData["success"] = "Інформацію успішно відредаговано";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Інформацію не було відредаговано";
            return View(customer);
        }
    }
}
