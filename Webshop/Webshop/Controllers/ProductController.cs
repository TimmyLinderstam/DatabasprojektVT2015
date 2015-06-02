using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Webshop.DBM;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            ViewBag.isAdmin = WebSecurity.IsAuthenticated;
            return View(DBController.Instance.GetProducts());
        }

        public ActionResult Item(int id)
        {
            ViewBag.isAdmin = WebSecurity.IsAuthenticated;

            var product = DBController.Instance.GetProduct(id);
            ViewBag.promotion = DBController.Instance.GetPromotionByCategory(product.Category);
            ViewBag.related = DBController.Instance.GetProductSuggestions(product);
            return View("Item", product);
        }

        [HttpPost]
        public ActionResult AddToBasket(int id, int quantity)
        {
            Customer c = (Customer)Session["Customer"];

            if (c != null)
            {
                if (DBController.Instance.AddToBasket(id, c.Id, quantity))
                {
                    ModelState.AddModelError("error", (quantity + " st laddes till i din kundvagn!"));
                    return Item(id);
                }
                else
                {
                    ModelState.AddModelError("error", "There isnt enough units!");
                    return Item(id);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
