using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Refund;

namespace Point_of_Sale.Controllers
{
    [Route("api/refund")]
    [ApiController]
    public class RefundController : BaseController
    {
        private readonly IRefundRepository _refundRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly ISales_SessionRepository _salesSessionRepository;
        public RefundController(IRefundRepository refundRepository, ISales_SessionRepository salesSessionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _refundRepository = refundRepository;
            _httpContextAccessor = httpContextAccessor;
            _salesSessionRepository = salesSessionRepository;
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Refund(long id)
        {
            try
            {
                var Refund = await this._refundRepository.Refund(id);
                return Ok(new ResponseSingleContentModel<RefundModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Refund
                });

            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }

        }
        [HttpPost("create")]
        public async Task<IActionResult> RefundCreate([FromBody] RefundModel RefundModels)
        {
            try
            {
                
                RefundModels.userAdded = userid(_httpContextAccessor);
                var sales_session_id = await this._salesSessionRepository.Sales_SessionCurrentId(RefundModels.userAdded);
                if (sales_session_id == 0)
                {
                    RefundModels.id = -1;
                    return Ok(new ResponseSingleContentModel<RefundModel>
                    {
                        StatusCode = 200,
                        Message = "Ca làm đã kết thúc",
                        Data = RefundModels
                    });
                }
                RefundModels.sales_session_id = sales_session_id;
                //var listWarehouse=await _userRepository.GetListWarehouseId(RefundModels.userAdded);
                // RefundModels.warehouse_id = listWarehouse.FirstOrDefault();
                var Refunds = await this._refundRepository.RefundCreate(RefundModels);
                return Ok(new ResponseSingleContentModel<RefundModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Refunds
                });

            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }
        
        [HttpPost("modify")]
        public async Task<IActionResult> RefundModify([FromBody] RefundModel RefundModel)
        {
            try
            {
                RefundModel.userUpdated = userid(_httpContextAccessor);
                var Refund = await this._refundRepository.RefundModify(RefundModel);
                return Ok(new ResponseSingleContentModel<RefundModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Refund
                });
            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("list")]
        public async Task<IActionResult> RefundList([FromBody] RefundSearch search)
        {
            try
            {
                var Refund = await this._refundRepository.RefundList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<RefundModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Refund
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }

        [HttpGet("list-order")]
        public async Task<IActionResult> OrderList(string? keyword, long warehouse_id)
        {
            try
            {
                var order = await this._refundRepository.OrderList(keyword, warehouse_id);
                return Ok(new ResponseSingleContentModel<List<RefundSearchOrderModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = order
                });
            }
       
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }

        [HttpPost("check-order")]
        public async Task<IActionResult> OrderCheck([FromBody] RefundSearchOrderModel model)
        {
            try
            {
                var data = await this._refundRepository.OrderCheck(model);
                return Ok(new ResponseSingleContentModel<OrderCheckData>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }


    }
}
