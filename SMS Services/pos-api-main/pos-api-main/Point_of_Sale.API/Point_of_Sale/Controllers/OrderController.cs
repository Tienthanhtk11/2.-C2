using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Repositories;

namespace Point_of_Sale.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISales_SessionRepository _salesSessionRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public OrderController(IOrderRepository orderRepository, ISales_SessionRepository salesSessionRepository,IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
            _salesSessionRepository = salesSessionRepository;
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Order(long id)
        {
            try
            {
                var order = await this._orderRepository.Order(id);
                return Ok(new ResponseSingleContentModel<OrderModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = order
                });

            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }

        }
        [HttpPost("create")]
        public async Task<IActionResult> OrderCreate([FromBody] OrderModel orderModels)
        {
            try
            { 
                orderModels.userAdded = userid(_httpContextAccessor);
                var saleId = await _salesSessionRepository.Sales_SessionCurrentId(orderModels.userAdded);
                if (saleId == 0)
                {
                    return BadRequest();
                }
                var orders = await this._orderRepository.OrderCreate(orderModels);
                return Ok(new ResponseSingleContentModel<OrderModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = orders
                });

            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("create-multi")]
        public async Task<IActionResult> OrdersCreate([FromBody] List<OrderModel> orderModels)
        {
            try
            {
                foreach (var orderModel in orderModels)
                    orderModel.userAdded = userid(_httpContextAccessor);
                var orders = await this._orderRepository.OrdersCreate(orderModels);
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = orders
                });

            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("modify")]
        public async Task<IActionResult> OrderModify([FromBody] OrderModel orderModel)
        {
            try
            {
                orderModel.userUpdated = userid(_httpContextAccessor);
                var order = await this._orderRepository.OrderModify(orderModel);
                return Ok(new ResponseSingleContentModel<OrderModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = order
                });
            }
            catch (Exception )
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("list")]
        public async Task<IActionResult> OrderList([FromBody] OrderSearch search)
        {
            try
            {
                var order = await this._orderRepository.OrderList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<OrderModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpGet("list-test")]
        public async Task<IActionResult> OrderList2()
        {
            try
            {
                var order = await this._orderRepository.OrderList2();
                return Ok(new ResponseSingleContentModel<List<OrderModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = order
                });
            }
            catch (Exception )
            {
                return this.RouteToInternalServerError();
            }
        }

    }
}
