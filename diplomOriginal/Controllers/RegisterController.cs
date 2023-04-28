using diplomOriginal.Models;
using diplomOriginal.Login;
using Microsoft.AspNetCore.Mvc;

namespace diplomOriginal.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            var user = HttpContext.User.Identity?.IsAuthenticated;
            if (user is not null && user.Value)
            {
                return Redirect("/cabinet");
            }

            return View();
        }

        public async Task<IActionResult> RegisterMe(LoginViewModel account)
        {
            ViewData["RegisterError"] = string.Empty;
            
            if (ModelState.IsValid)
            {
				if (Database.DoesAccountExist(account.Email).Result)
				{
					ViewData["RegisterError"] = "Пользователь с таким именем уже существует";
					return View("Index");
				}
				else
				{
					if (Database.AddAccountToDB(account).Result)
					{
						await LoginManager.Login(HttpContext, account);

                        return Redirect("/cabinet");
                    }
				}
			}

            ViewData["RegisterError"] = "Ошибка при регистрации нового пользователя, свяжитесь с администратором.";
			return View("Index");
		}
    }
}
