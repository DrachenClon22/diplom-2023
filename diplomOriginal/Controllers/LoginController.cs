using diplomOriginal.Models;
using Microsoft.AspNetCore.Mvc;
using diplomOriginal.Login;

namespace diplomOriginal.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            var user = HttpContext.User.Identity?.IsAuthenticated;
            if (user is not null && user.Value)
            {
                return RedirectToAction("Index", "UserCabinet");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            var user = HttpContext.User.Identity?.IsAuthenticated;
            if (user is not null && user.Value)
            {
                await LoginManager.Logout(HttpContext);
            }

            return Redirect("/");
        }

        // postIp is for global Login form, not for Login Index
        [HttpPost]
        public IActionResult Identify(LoginViewModel account, string postIp)
        {
            TempData["ErrorMessage"] = null;

            if (LoginManager.Login(HttpContext, account).Result)
            {
                return Redirect(postIp ?? "/");
            } else
            {
                TempData["ErrorMessage"] = $"Проверьте правильность логина и пароля";
                return RedirectToAction("Index");
            }
        }
    }
}
