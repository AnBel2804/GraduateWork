using Graduate_Work.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduate_Work.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = Roles.Role_Customer)]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
