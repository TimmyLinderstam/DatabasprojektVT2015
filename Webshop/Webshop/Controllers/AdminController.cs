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

        public ActionResult EditProduct()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SaveProduct(Product p)
        {
            DBController.Instance.AddProduct(p);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveProduct(int id)
        {
            DBController.Instance.RemoveProduct(id);

            return RedirectToAction("Index");
        }

    }
}
