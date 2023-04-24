using diplomOriginal.Login;
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
        public async Task<IActionResult> ChangeName(string newName)
        {
            TempData["ErrorMessage"] = null;

            LoginViewModel vm = new LoginViewModel() { Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? "_" };
            if (Database.ChangeAccountName(vm, newName).Result)
            {
                await LoginManager.Logout(HttpContext);
                return Redirect("/login");
            }

            TempData["ErrorMessage"] = "Ошибка при изменении имени, свяжитесь с администратором";
            return RedirectToAction("Index");
        }
    }
}
