//using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NEST.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("db_student");
            //var Client = new ElasticClient(settings);

            // client.CreateMapping();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}