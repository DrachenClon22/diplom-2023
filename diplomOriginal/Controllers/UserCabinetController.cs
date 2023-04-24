using diplomOriginal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace diplomOriginal.Controllers
{
    public class UserCabinetController : Controller
    {
        public ActionResult Index()
        {
            var user = HttpContext.User.Identity?.IsAuthenticated;
            if (user is not null && user.Value)
            {
                return View();
            } else
            {
                return Redirect("/login");
            }
        }

        [HttpPost]
        public IActionResult ChangeName(string name)
        {
            LoginViewModel vm = new LoginViewModel() { Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? "_" };
            if (Database.ChangeAccountName(vm, name).Result)
            {
                return RedirectToAction("Index");
            } else
            {
                ViewData["ErrorMessage"] = "Ошибка при изменении имени, свяжитесь с администратором";
                return RedirectToAction("Index");
            }
        }
    }
}
