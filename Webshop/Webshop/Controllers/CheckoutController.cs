using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.DBM;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class CheckoutController : Controller
    {
        //
        // GET: /Checkout/

        public ActionResult Index()
        {
            Customer c = ((Customer)Session["Customer"]);

            if (c == null)
            {
                return View();
            }
            else
            {
                return View(DBController.Instance.GetBasketByPersonId(c.PersonId));
            }
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveCustomer(Customer customer)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                DBController.Instance.SaveCustomer(customer);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("CreateCustomer");
            }
        }

        [HttpPost]
        public ActionResult Login(string id)
        {
            Session["Customer"] = DBController.Instance.GetCustomerByPersonId(id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Buy()
        {
            Customer c = (Customer)Session["Customer"];

            if (c != null)
            {
                DBController.Instance.Buy(c);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("error", "Du har inte loggat in!");
                return RedirectToAction("Index", "Checkout");
            }
        }
    }
}
