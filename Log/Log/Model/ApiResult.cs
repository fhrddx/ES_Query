
namespace Log.Model
{
    public class ApiResult
    {
        public string apiName { get; set; }

        public string time { get; set; }

        public string key { get; set; }

        public bool cache { get; set; }

        public bool isRefresh { get; set; }

        public string clientIp { get; set; }
    }
}