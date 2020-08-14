using Log.Bll;
using Log.Model;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Log.Controllers
{
    public class ESController : Controller
    {
        private readonly string xing = @"王李张刘陈杨赵黄周吴徐孙胡朱高林何郭马罗梁宋郑谢韩唐冯于董萧程曹袁邓许傅沈曾彭吕苏卢蒋蔡贾丁魏薛叶阎余潘杜戴夏钟汪田任姜范方石姚谭廖邹熊金陆郝孔白崔康毛邱秦江史顾侯邵孟龙万段雷钱汤尹黎易常武乔贺赖龚文庞樊兰殷施陶洪翟安颜倪严牛温芦季俞章鲁葛伍韦申尤毕聂丛焦向柳邢路岳齐沿梅莫庄辛管祝左涂谷祁时舒耿牟卜肖詹关苗凌费纪靳盛童欧甄项曲成游阳裴席卫查屈鲍位覃霍翁隋植甘景薄单包司柏宁柯阮桂闵欧阳解强柴华车冉房边净阴闫佘练骆付代麦容悲初瞿褚班全名井米谈宫虞奚佟符蒲穆漆卞东储党从艾苻厉岑燕吉冷仇伊首郁娄楚邝历狄简胥连帅封危支原滕苑信索栗官沙池藏师国巩刁茅杭巫居窦皮戈麻饶习巴旷宗荆荣孝蔺廉员西寇刃见底区郦卓琚续朴蒙敖花应喻冀尚顿菅嵇雒弓忻权谌卿扈海冼伦鹿宿山桑裘达么智宣尉迟东方幺郎农戚屠楼步鞠仲尉蓝招攀栾籍寿邬荚税逄加勾由福缑钦鲜于但邸逢况鄢古乐斯钮盖旦毅邰哈鄂商英迟仝亓玄黑腾晏禹诸苟湛殳亢奉占闻粟种匡宾劳申屠伏过水真宇巢计羌相辜展丑银丰矫上昝绳臧舍郅布糜乌衣来恒那满门司徒皋旺公言藤释尧缪干阚靖渠契晋六束良鹗贝邴沃竺扬励归上官荃焉多都果郜隆诸葛令狐慕礼祖翦力朗撖修呼富明站虢冶茹禚笪云肇平弋候尔姬宝畅冒邾延禅浦敬颉南巍补";

        private readonly string name = @"帆栋祜权锟坤允骞谛初盛炳初泽荣喆恒鹤礼华帝宇中鑫彬槐禧允翱鹏皓中伟炳皓槐帆芃欣鑫振杰诚锟潍吉轩福宇初柏芃翰浩峰延帆欣帆奇郁烁卓仕吉帝潍钊杰鑫星谛鑫铭锋沛芃泽禄勇峰欣延鹤郁信侠翰邦寅轩泽哲佑福翱恒文枫澄栋翰中震杞斌凯锦升逸延腾谛权盛弘烁俊强博禄中欣权浩阳裕延盛平畅沛吉强骏起华炳腾柏佑畅杰凯鸿斌加振晨沛祥祜盛濡彬成弘天福锦颖嘉茜芸格美漫慧漫妍钰琪玥沛玥鑫洁岚采曼珍雪昕婷碧弦雪洁馨昕香弦帆芳菲楠俊月珊函蔚帆灵灵莲优蔚碧文蕾娅林婧妮婷薇馨淑惠杉美栀怡薇琪曦云漫瑶韵楠妮颖妮杉媛诗芳菲锦锦蕾芸欢珍岚鹤莉优云舒舒璇慧依菡雅妍楠雅慧灵阳漫珠帆媛可雅欣鑫妮雯霞柔芳芝琳彩冰林媛柔初倩玉冰薇洁妍洁璐采彩颖呈雪云欢琪璟紫静蓓薇欢薇柔晨萱云歆鑫月阳娅媛露露琳";

        private readonly string[] school = new string[] { "中山大学", "暨南大学", "汕头大学", "华南理工大学", "华南农业大学", "广东海洋大学", "广州医科大学", "广州中医药大学", "华南师范大学", "韶关学院", "深圳大学", "广东财经大学", "广东工业大学", "东莞理工学院", "南方科技大学", "香港中文大学", "广州商学院", "上海交通大学", "同济大学", "复旦大学", "上海大学", "上海财经大学", "北京大学", "清华大学", "北京邮电大学", "中国人民大学", "北京理工大学" };

        public object Search()
        {
            //第一步：建立索引库，设置映射规则
            //ElasticSearchHelper.Intance.BuildStudentMapping();
            //第二步：给索引库一个别名
            //ElasticSearchHelper.Intance.Alias();
            //第三步：随机生成测试数据
            int xing_length = xing.Length;
            int name_length = name.Length;
            int school_length = school.Length;
            int content_length = TestData.content.Length;

            ParallelOptions _po = new ParallelOptions();
            _po.MaxDegreeOfParallelism = 4;
            Parallel.For(0, 100000000, _po, c =>
            {
                Random r = new Random(c);
                int xingIndex = r.Next(0, xing_length);
                string stu_xing = xing[xingIndex].ToString();
                int nameStartIndex = r.Next(0, name_length / 2);
                string stu_name = name.Substring(nameStartIndex * 2, 2);
                try
                {
                    string desc = TestData.content.Substring((r.Next(0, content_length - 700)), 20);
                    desc = desc.Trim();
                    desc.Replace("\r\n", "");
                    Student model = new Student()
                    {
                        name = stu_xing + stu_name,
                        school = school[r.Next(0, school_length)],
                        chinese = r.Next(25, 94),
                        math = r.Next(15, 100),
                        english = r.Next(21, 95),
                        @class = c,
                        desc = desc
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