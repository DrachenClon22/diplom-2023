using diplomOriginal.Models;
using Microsoft.AspNetCore.Mvc;

namespace diplomOriginal.Controllers
{
    public class ContactsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View("About");
        }

        [HttpPost]
        public IActionResult Index(ContactsViewModel contact)
        {
            if (ModelState.IsValid)
            {
                ViewData["Message"] = "Сообщение отправлено!";
            } else
            {
                ViewData["Message"] = "Ошибка при отправке сообщения, проверьте правильность заполнения форм";
            }

            return View();
        }
    }
}
