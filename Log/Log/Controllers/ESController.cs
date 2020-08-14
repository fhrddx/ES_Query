using Bll;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Controllers
{
    public class ESController : Controller
    {
        //测试单个词的搜索
        public ActionResult Index(string key = "黛玉")
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            var model = ElasticSearchHelper.Intance.Term(key.Trim(), 0, 10);
            sw.Stop();
            ViewBag.Message = $"共耗时{sw.ElapsedMilliseconds}毫秒";
            return View(model);
        }

        public ActionResult Query(string key = "宝玉黛玉螃蟹宴作诗")
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            var model = ElasticSearchHelper.Intance.Query(key.Trim(), 0, 10);
            sw.Stop();
            ViewBag.Message = $"共耗时{sw.ElapsedMilliseconds}毫秒";
            return View("~/Views/ES/Index.cshtml", model);
        }

        public object Search()
        {
            //第一步：建立索引库，设置映射规则
            //ElasticSearchHelper.Intance.BuildStudentMapping();
            //第二步：给索引库一个别名
            //ElasticSearchHelper.Intance.Alias();
            //第三步：随机生成测试数据
            int xing_length = TestData.xing.Length;
            int name_length = TestData.name.Length;
            int school_length = TestData.school.Length;
            int content_length = TestData.content.Length;

            ParallelOptions _po = new ParallelOptions();
            _po.MaxDegreeOfParallelism = 4;
            Parallel.For(0, 100000000, _po, c =>
            {
                Random r = new Random(c);
                Random r2 = new Random();
                try
                {
                    string desc = TestData.content.Substring((r.Next(0, content_length - 700)), 20).Trim().Replace("/r/n", string.Empty);
                    Student model = new Student()
                    {
                        name = TestData.xing[r.Next(0, xing_length)].ToString() + TestData.name.Substring(r.Next(0, name_length / 2) * 2, 2),
                        school = TestData.school[r.Next(0, school_length)],
                        chinese = r.Next(25, 80) + r2.Next(0, 20),
                        math = r.Next(15, 60) + r2.Next(0, 40),
                        english = r.Next(21, 70) + r2.Next(0, 30),
                        @class = c,
                        desc = desc + TestData.school[r2.Next(0, school_length)]
                    };
                    ElasticSearchHelper.Intance.CreateIndex("db_student_test1", Guid.NewGuid().ToString(), model);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            });
            return 1;
        }
    }
}