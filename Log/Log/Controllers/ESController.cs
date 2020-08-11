using Log.Bll;
using Log.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Log.Controllers
{
    public class ESController : Controller
    {

        public ActionResult Search()
        {
            ElasticSearchHelper.Intance.BuildStudentMapping();

            //ElasticSearchHelper.Intance.

            /*
             Student model = new Student()
            {
                name = "范冰冰",
                school = "上海戏剧学院",
                chinese = 90,
                math = 65,
                english = 78,
                @class = 1,
                desc = "来自山东青岛"
            };
            ElasticSearchHelper.Intance.CreateIndex("student", Guid.NewGuid().ToString(), model);
            */
            return View();
        }
    }
}