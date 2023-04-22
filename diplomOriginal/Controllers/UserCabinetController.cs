using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
