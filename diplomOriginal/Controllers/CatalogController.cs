using diplomOriginal.Models;
using diplomOriginal.Modules;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace diplomOriginal.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index(string? catalog, int? id)
        {
            //if (catalog is not null)
            //{
            //    if (id is not null)
            //    {
            //        var a = new { Catalog = catalog, ID = id };
            //        ViewData["Data"] = a;

            //        // try to find an item with id, if not, then error page that item has not been found
            //        // if item has been found, then find image path, description and price and etc and make a class
            //        // and send to view page via ItemViewModel.cs (to be created) to display

            //        return View("Index");
            //    } else
            //    {
            //        var a = new { Catalog = catalog };
            //        ViewData["Data"] = a;

            //        // try to find a catalogue with catalogue name, if not, redirect to main page

            //        return View("CatalogPage");
            //    }
            //} else
            //{
            //    return View("CatalogueList");
            //}
            return View("CatalogueList");
        }

        [HttpPost]
        public IActionResult AddToCart(string id, string prevIp)
        {
            ShoppingCart.AddItem(id);
            return Redirect(prevIp ?? "/");
        }
    }
}
