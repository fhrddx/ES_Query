using Log.Bll;
using Log.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Log.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Test()
        {
            return View();
        }

        //这个大概好了，明天再继续
        public ActionResult Index()
        {
            return View();
        }

        //按照 ApiName 调用次数排序
        public JsonResult ApiNameRanking(DateTime from, DateTime to, int take = 10)
        {
            List<ApiItem> result = new List<ApiItem>();
            List<ApiResult> list = new LogBll().list(from, to);
            if (list != null && list.Any())
            {
                result = list.GroupBy(i => new { i.apiName }).Select(i => new ApiItem
                {
                    ApiName = i.Key.apiName,
                    Total = i.Count()
                }).ToList();
                result = result.OrderByDescending(i => i.Total).Take(take).ToList();
            }
            return Json(new { list = result }, JsonRequestBehavior.AllowGet);
        }

        //按照 ApiKey 调用次数排序
        public JsonResult ApiKeyRanking(DateTime from, DateTime to, int take = 10)
        {
            List<ApiItem> result = new List<ApiItem>();
            List<ApiResult> list = new LogBll().list(from, to);
            if (list != null && list.Any())
            {
                result = list.GroupBy(i => new { i.key }).Select(i => new ApiItem
                {
                    Key = i.Key.key,
                    Total = i.Count()
                }).ToList();
                result = result.OrderByDescending(i => i.Total).Take(take).ToList();
            }
            return Json(new { list = result }, JsonRequestBehavior.AllowGet);
        }

        //按照压力指数排名
        public JsonResult ApiPressureRanking(DateTime from, DateTime to, int take = 500)
        {
            List<ApiResult> list = new LogBll().list(from, to);

            var result = list.GroupBy(i => new { i.key, i.apiName }).Select(i =>
            {
                int total = i.Count();
                int cache = i.Count(x => x.cache);
                double pressure = (double)(total - cache) / total * 100;
                return new ApiItem
                {
                    Key = i.Key.key,
                    ApiName = i.Key.apiName,
                    Total = i.Count(),
                    Cache = i.Count(x => x.cache),
                    Pressure = pressure
                };
            }).OrderByDescending(i => i.Total).ThenByDescending(i => i.Pressure).Take(take).ToList();

            return Json(new { list = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TimeList(DateTime from, DateTime to)
        {
            List<ApiResult> list = new LogBll().list(from, to);
            var result = list.GroupBy(i => new { Hour = DateTime.Parse(i.time).ToString("HH") }).Select(i => new
            {
                Time = i.Key.Hour,
                Count = i.Count()
            }).OrderBy(i => i.Time).ToList();

            List<string> hours = new List<string>();
            for (int i = 0; i < 24; i++)
            {
                hours.Add(i < 10 ? "0" + i : i.ToString());
            }

            var query = from h in hours
                        join d in result on h equals d.Time into temp
                        from t in temp.DefaultIfEmpty()
                        select new TimeItem
                        {
                            Hour = h,
                            Count = t == null ? 0 : t.Count
                        };
            var timeList = query.OrderBy(i => i.Hour).ToList();
            return Json(new
            {
                category = timeList.Select(i => i.Hour).ToList(),
                data = timeList.Select(i => i.Count).ToList()
            }, JsonRequestBehavior.AllowGet);
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

        public class ApiItem
        {
            public string Key { get; set; }

            public string ApiName { get; set; }

            public int Total { get; set; }

            public int Cache { get; set; }

            public double Pressure { get; set; }
        }

        public class TimeItem
        {
            public string Hour { get; set; }

            public int Count { get; set; }
        }
    }
}