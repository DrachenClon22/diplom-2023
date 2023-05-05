using Microsoft.AspNetCore.Mvc;

namespace diplomOriginal.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            var user = HttpContext.User.Identity?.IsAuthenticated;
            if (user is not null && user.Value)
            {
                return View();
            }
            else
            {
                return Redirect("/");
            }
        }
    }
}
