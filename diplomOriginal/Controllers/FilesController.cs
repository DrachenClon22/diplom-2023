using Microsoft.AspNetCore.Mvc;

namespace diplomOriginal.Controllers
{
    public class FilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
