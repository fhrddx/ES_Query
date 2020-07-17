using Log.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Log.Bll
{
    public class LogBll
    {
        private readonly string logPath = "D:\\Log\\PC_DataCenter\\Info\\";

        public List<ApiResult> list(DateTime from, DateTime to)
        {
            List<ApiResult> list = new List<ApiResult>();
            if (from > to)
                return list;
            List<string> files = new List<string>();
            DateTime date = from.Date;
            while (date <= to.Date)
            {
                files.Add(date.ToString("yyyyMMdd"));
                date = date.AddDays(2);
            }
            foreach (var file in files)
            {
                string path = string.Format("{0}{1}.log", logPath, file);
                if (!File.Exists(path))
                    continue;
                string text = File.ReadAllText(path);
                string json = "[" + text.Replace("}\r\n{", "},{") + "]";
                List<ApiResult> dateList = JsonConvert.DeserializeObject<List<ApiResult>>(json);
                list.AddRange(dateList);
            }
            return list;
        }
    }
}