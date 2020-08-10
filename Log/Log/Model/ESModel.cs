using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Log.Model
{
    /// <summary>
    /// ik分词结果对象
    /// </summary>
    public class ik
    {
        public List<tokens> tokens { get; set; }
    }

    public class tokens
    {
        public string token { get; set; }
        public int start_offset { get; set; }
        public int end_offset { get; set; }
        public string type { get; set; }
        public int position { get; set; }
    }

    /// <summary>
    /// 测试数据对象
    /// </summary>
    public class personList
    {
        public personList()
        {
            this.list = new List<person>();
        }
        public int hits { get; set; }
        public int took { get; set; }
        public List<person> list { get; set; }
    }

    public class person
    {
        public string id { get; set; }

        public string name { get; set; }

        public int age { get; set; }

        public bool sex { get; set; }

        public DateTime birthday { get; set; }

        public string intro { get; set; }
    }
}