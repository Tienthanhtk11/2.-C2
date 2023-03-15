using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.SaleSession;

namespace Point_of_Sale.Controllers
{
    [Route("api/sale-session")]
    [ApiController]
    public class SaleSessionController : BaseController
    {
        private readonly ISales_SessionRepository _salesSessionRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public SaleSessionController(ISales_SessionRepository salesSessionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _salesSessionRepository = salesSessionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Sales_SessionCreate([FromBody] Sales_Session model)
        {
            try
            {
                var resonse = await this._salesSessionRepository.Sales_SessionCreate(model);
                return Ok(new ResponseSingleContentModel<long>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = resonse
                });

            }
            catch (Exception )
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("modify")]
        public async Task<IActionResult> Sales_SessionModify([FromBody] Sales_Session model)
        {
            try
            {
                var sale_session = await this._salesSessionRepository.Sales_SessionModify(model);
                return Ok(new ResponseSingleContentModel<Sales_Session>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = sale_session
                });

            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("list")]
        public async Task<IActionResult> Sales_SessionList([FromBody] SaleSessionSearch search)
        {
            try
            {
                var sale_session = await this._salesSessionRepository.Sales_SessionList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<Sales_SessionModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = sale_session
                });
            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();


            }
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Sales_SessionDetail(long id)
        {
            try
            {
                var user_id = userid(_httpContextAccessor);
                var resonse = await this._salesSessionRepository.Sales_SessionDetail(id, user_id);
                return Ok(new ResponseSingleContentModel<Sales_SessionModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = resonse
                });

            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }

        [HttpGet("id-current")]
        public async Task<IActionResult> Sales_SessionCurrentId()
        {
            try
            {
                var user_id = userid(_httpContextAccessor);
                var resonse = await this._salesSessionRepository.Sales_SessionCurrentId(user_id);
                return Ok(new ResponseSingleContentModel<long>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = resonse
                });

            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }


    }
}
