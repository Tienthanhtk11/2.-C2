using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SMS_Services.Controllers
{
    [ApiController]
    //[Authorize]
    public class BaseController : ControllerBase
    {
        protected long userid(IHttpContextAccessor _httpContextAccessor)
        {
            long id = long.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("sid", StringComparison.OrdinalIgnoreCase))?.Value ?? "0");
            return id;
        }
        protected string username(IHttpContextAccessor _httpContextAccessor)
        {
            string user_name = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("user_name", StringComparison.OrdinalIgnoreCase))?.Value ?? "";
            return user_name;
        }
        protected IActionResult RouteToInternalServerError()
        {
            // Return server error
            return StatusCode(500, new ResponseSingleContentModel<IResponseData>
            {
                StatusCode = 500,
                Message = "Internal Server Error!",
            });
        }

        protected IActionResult RouteToFordbiddenServerError()
        {
            return StatusCode(403, new ResponseSingleContentModel<IResponseData>
            {
                StatusCode = 403,
                Message = "Fordbidden!",

            });
        }
        public class ResponseSingleContentModel<T>
        {
            public string Message { get; set; } = string.Empty;
            public int StatusCode { set; get; } = 200;
            public T? Data { set; get; }
        }
        public class ResponseMultiContentModels<T>
        {
            public string Message { get; set; } = string.Empty;
            public int StatusCode { set; get; } = 200;
            public List<T> Data { set; get; }
        }
        public interface IResponseData
        {

        }
    }
}
