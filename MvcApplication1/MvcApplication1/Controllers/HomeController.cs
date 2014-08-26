using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

            var db = new DBEntities();
            db.Database.CreateIfNotExists();

            System.Diagnostics.Debug.Write("Início");

            return View();
        }

    }
}