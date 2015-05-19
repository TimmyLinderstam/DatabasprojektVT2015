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
            var product = DBController.Instance.GetProduct(id);

            ViewBag.categories = DBController.Instance.GetCategories();

            // Series
            var ls = new List<Serie>();
            ls.Add(new Serie() { Id = -1, SerieName = "(Ingen)" });
            ls.AddRange(DBController.Instance.GetSeries());

          //  var series = new SelectList(ls, "Id", "SerieName", product.Series);
            //series.Where(x => x.Value == product.Series.ToString()).All(x => x.Selected = true);


            var serieList = new List<SelectListItem>();

            foreach (Serie p in ls)
            {
                serieList.Add(new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.SerieName.ToString(),
                    // To set the selected item use the following code 
                    // Note: you should not set every item to selected
                    Selected = product == null ? false : product.Series == p.Id
                });
            }

            ViewBag.series = serieList;
           
            return View(product);
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


        // Serie
        public ActionResult Serie()
        {
            return View(DBController.Instance.GetSeries());
        }

        // Category
        public ActionResult RemoveSerie(int id)
        {
            DBController.Instance.RemoveSerie(id);

            return RedirectToAction("Serie", "Admin");
        }

        [HttpPost]
        public ActionResult SaveSerie(Serie serie)
        {
            DBController.Instance.SaveSerie(serie);

            return RedirectToAction("Serie", "Admin");
        }

        // purchases
        public ActionResult Purchase()
        {
            return View(DBController.Instance.GetPurchases());
        }


    }
}
