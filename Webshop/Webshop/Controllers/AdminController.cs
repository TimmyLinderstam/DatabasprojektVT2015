using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.DBM;
using Webshop.Models;

namespace Webshop.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult EditProduct(int id = -1)
        {
            ViewBag.categories = DBController.Instance.GetCategories();
            return View(DBController.Instance.GetProduct(id));
        }

        [HttpPost]
        public ActionResult SaveProduct(Product p)
        {
            DBController.Instance.SaveProduct(p);

            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public ActionResult RemoveProduct(int id)
        {
            DBController.Instance.RemoveProduct(id);

            return RedirectToAction("Index", "Product");
        }

        public ActionResult Category()
        {
            return View(DBController.Instance.GetCategories());
        }

        // Category
        public ActionResult RemoveCategory(int id)
        {
            DBController.Instance.RemoveCategory(id);

            return RedirectToAction("Category", "Admin");
        }

        [HttpPost]
        public ActionResult SaveCategory(Category category)
        {
            DBController.Instance.SaveCategory(category);

            return RedirectToAction("Category", "Admin");
        }

    }
}
