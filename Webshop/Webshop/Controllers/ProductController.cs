using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Webshop.DBM;

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
            return View(DBController.Instance.GetProduct(id));
        }

    }
}
