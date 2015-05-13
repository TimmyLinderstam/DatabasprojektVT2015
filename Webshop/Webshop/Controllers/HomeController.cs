using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.DBM;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
  

        public ActionResult Index()
        {
            ViewBag.Message = "Välkommen!";

           // DBManager.Instance.Test();
            var cmd = DBManager.Instance.CreateCmd();
            cmd.CommandText = "select * from Products";
            var f = DBManager.Instance.ReadQuery<Product>(cmd);
            //MySqlCommand 
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
