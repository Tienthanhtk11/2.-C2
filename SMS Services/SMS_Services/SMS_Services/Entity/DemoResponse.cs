using System.Net;

namespace SMS_Services
{
    public class DemoResponse<T>
    {
        public HttpStatusCode Code { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }

        public static DemoResponse<T> GetResult(HttpStatusCode StatusCode, string Description, object? Data = null)
        {

            return new DemoResponse<T>
            {
                Code = StatusCode,
                Msg = Description,
                Data = Data
            };
        }
    }
}
