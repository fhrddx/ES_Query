using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;

namespace Bll
{
    //PlainElastic.Net 说明文档 https://github.com/Yegoroff/PlainElastic.Net

    public class ElasticSearchHelper
    {
        public static readonly ElasticSearchHelper Intance = new ElasticSearchHelper();

        private ElasticConnection Client;

        private ElasticSearchHelper()
        {
            Client = new ElasticConnection("localhost", 9200);
        }

        #region 创建索引库和映射规则
        public bool BuildStudentMapping()
        {
            #region 第一个坑，也是版本问题：不能用 PlainElastic.Net 这种方式建立 mapping ，原因是ES版本是7.8.0 ，这个版本不再支持 string , 取而代之的是 text 
            /*string mapping = new MapBuilder<Student>()
                                 .RootObject(typeName: "mappings",
                                    map: r => r.All(a => a.Enabled(false))
                                       .Dynamic(false)
                                       .Properties(pr => pr.String(person => person.name, f => f.Analyzer(DefaultAnalyzers.standard).Boost(2))
                                                           .String(person => person.school, f => f.Analyzer("ik_max_word"))
                                                           .String(person => person.desc, f => f.Analyzer("ik_max_word"))
                                       .Number(person => person.@class)
                                       .Number(person => person.chinese)
                                       .Number(person => person.english)
                                       .Number(person => person.math)
                                       )
                                  ).BuildBeautified();*/
            #endregion
            var mapping = new
            {
                mappings = new
                {
                    properties = new
                    {
                        name = new
                        {
                            type = "text",
                            analyzer = "standard"
                        },
                        school = new
                        {
                            type = "text",
                            analyzer = "ik_max_word"
                        },
                        desc = new
                        {
                            type = "text",
                            analyzer = "ik_max_word"
                        },
                        @class = new
                        {
                            type = "integer"
                        },
                        chinese = new
                        {
                            type = "integer"
                        },
                        english = new
                        {
                            type = "integer"
                        },
                        math = new
                        {
                            type = "integer"
                        }
                    }
                }
            };
            string jsonDocument = new JsonNetSerializer().Serialize(mapping);
            OperationResult operationResult = Client.Put("db_student_test1", jsonDocument);
            CommandResult result = new JsonNetSerializer().ToCommandResult(operationResult.Result);
            if (result?.acknowledged != null)
                return result.acknowledged;
            return false;
        }
        #endregion

        #region 给索引库 db_student_test 起一个别名 student_test
        public bool Alias()
        {
            OperationResult operationResult = Client.Put("db_student_test1/_alias/student_test1");
            CommandResult result = new JsonNetSerializer().ToCommandResult(operationResult.Result);
            if (result?.acknowledged != null)
                return result.acknowledged;
            return false;
        }
        #endregion

        #region 插入索引文档数据
        public IndexResult CreateIndex(string indexName, string id, string jsonDocument)
        {
            var serializer = new JsonNetSerializer();
            //注意ES版本是8.7.0，type只能是默认的、唯一的 _doc
            string cmd = new IndexCommand(indexName, "_doc", id);
            Client.Timeout = 30000;
            OperationResult result = Client.Put(cmd, jsonDocument);
            var indexResult = serializer.ToIndexResult(result.Result);
            GC.Collect();
            return indexResult;
        }

        public IndexResult CreateIndex(string indexName, string id, object document)
        {
            var serializer = new JsonNetSerializer();
            var jsonDocument = serializer.Serialize(document);
            return CreateIndex(indexName, id, jsonDocument);
        }
        #endregion

        #region  单个关键词查询（分页、高亮、And逻辑、范围查询、排序）
        public ElasticsearchResult<Student> Term(string key, int from = 0, int size = 10)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            key = key.Trim();
            string cmd = new SearchCommand("student_test1", "_doc");
            var query = new QueryBuilder<Student>().Query(
                              b => b.Bool(m => m.Must(t =>
                                   t.Term(d => d.Field("desc").Value(key))
                                    .Range(d => d.Field("chinese").From("90").To("100"))
                                    .Range(d => d.Field("math").Gt("90"))
                                    .Range(d => d.Field("english").Gt("90"))
                                 )
                               )
                             )
                             .From(from)
                             .Size(size)
                             .Sort(s => s.Field("chinese", SortDirection.desc).Field("math", SortDirection.desc).Field("english", SortDirection.desc))
                             .Highlight(h => h
                                  .PreTags("<span class=\"label label-sm label-danger\">")
                                  .PostTags("</span>")
                                  .Fields(
                                      f => f.FieldName("desc").Order(HighlightOrder.score)
                                   )
                              )
                             .Build();

            string result = Client.Post(cmd, query);
            var list = new JsonNetSerializer().Deserialize<ElasticsearchResult<Student>>(result);
            return list;
        }
        #endregion

        #region  多个关键词查询（分页、高亮、And逻辑、范围查询、排序）
        public ElasticsearchResult<Student> Query(string key, int from = 0, int size = 10)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            key = key.Trim();
            string cmd = new SearchCommand("student_test1", "_doc");
            var query = new QueryBuilder<Student>().Query(
                              b => b.Bool(m => m.Must(t =>
                                   //其实也是可以用 t.match() 的，可以试一下
                                   t.QueryString(d => d.DefaultField("desc").Query(key))
                                    .Range(d => d.Field("chinese").From("90").To("100"))
                                    .Range(d => d.Field("math").Gt("90"))
                                    .Range(d => d.Field("english").Gt("90"))
                                 )
                               )
                             )
                             .From(from)
                             .Size(size)
                             //这里不再按照分数来排序，这时ES会根据关键词匹配度来排序，出现在最前的，应该是最匹配的
                             //.Sort(s => s.Field("chinese", SortDirection.desc).Field("math", SortDirection.desc).Field("english", SortDirection.desc))
                             .Highlight(h => h
                                  .PreTags("<span class=\"label label-sm label-danger\">")
                                  .PostTags("</span>")
                                  .Fields(
                                      f => f.FieldName("desc").Order(HighlightOrder.score)
                                   )
                              )
                             .Build();

            string result = Client.Post(cmd, query);
            var list = new JsonNetSerializer().Deserialize<ElasticsearchResult<Student>>(result);
            return list;
        }
        #endregion
    }

    #region 辅助类
    public class Student
    {
        public string name { get; set; }

        public string school { get; set; }

        public int chinese { get; set; }

        public int math { get; set; }

        public int english { get; set; }

        public int @class { get; set; }

        public string desc { get; set; }
    }

    public class ElasticsearchShards
    {
        public int total { get; set; }

        public int successful { get; set; }

        public int skipped { get; set; }

        public int failed { get; set; }
    }

    public class ElasticsearchHitTotal
    {
        public int value { get; set; }

        public string relation { get; set; }
    }

    public class HisItem<T>
    {
        public string _index { get; set; }

        public string _type { get; set; }

        public string _id { get; set; }

        public double _score { get; set; }

        public T _source { get; set; }

        public Dictionary<string, string[]> highlight { get; set; }
    }

    public class ElasticsearchHit<T>
    {
        public ElasticsearchHitTotal total { get; set; }

        public double max_score { get; set; }

        public List<HisItem<T>> hits { get; set; }
    }

    public class ElasticsearchResult<T>
    {
        public int took { get; set; }

        public bool timed_out { get; set; }

        public ElasticsearchShards _shards { get; set; }

        public ElasticsearchHit<T> hits { get; set; }
    }
    #endregion
}