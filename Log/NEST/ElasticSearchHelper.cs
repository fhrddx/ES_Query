//using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace NEST
{
    //public class ElasticSearchHelper
    //{
    //    public static readonly ElasticSearchHelper Intance = new ElasticSearchHelper();

    //    private ElasticClient Client;

    //    private ElasticSearchHelper()
    //    {
    //        var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("db_student");
    //        Client = new ElasticClient(settings);
    //    }

    //    public void CreateMapping()
    //    {
    //        try
    //        {
    //            var createIndexResponse = Client.CreateIndex("db_student", c => c
    //                .Mappings(ms => ms
    //                   .Map<Student>(m => m.AutoMap())
    //                )
    //            );
    //            string a = "";
    //        }
    //        catch (Exception ex)
    //        {
    //            string b = "";
    //        }
    //    }
    //}

    //public class Student
    //{
    //    [Number(Name = "id", DocValues = false, IgnoreMalformed = true, Coerce = true)]
    //    public int id { get; set; }

    //    [Text(Name = "name")]
    //    public string name { get; set; }

    //    [Text(Name = "name", Analyzer = "ik_max_word")]
    //    public string school { get; set; }

    //    public int chinese { get; set; }

    //    public int math { get; set; }

    //    public int english { get; set; }

    //    [Text(Name = "name", Analyzer = "ik_max_word")]
    //    public string desc { get; set; }
    //}
}