using ECom.Framework.Validator;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Inventory;
using Point_of_Sale.Model.Request;
using Point_of_Sale.Model.Warehouse;

namespace Point_of_Sale.Controllers
{
    [Route("api/warehouse-request")]
    [ApiController]
    public class WarehouseRequestController : BaseController
    {
        private readonly IRequestRepository _requestRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public WarehouseRequestController(IRequestRepository requestRepository, IHttpContextAccessor httpContextAccessor)
        {
            _requestRepository = requestRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("detail")]
        public async Task<IActionResult> RequestDetail(long id)
        {
            try
            {
                var products = await _requestRepository.Request(id);
                return Ok(new ResponseSingleContentModel<RequestModel>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = products
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpGet("modify-status")]
        public async Task<IActionResult> RequestModifyStatus(long id, byte status)
        {
            try
            {
                string? mess = await _requestRepository.RequestModifyStatus(id, status);
                return mess == "0"
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Cập nhật không thành công " + mess,
                        Data = null
                    });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> RequestCreate([FromBody] RequestModel model)
        {
            try
            {
                var validator = ValitRules<RequestModel>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    string? mess = await _requestRepository.RequestCreate(model);
                    return mess == "0"
                        ? Ok(new ResponseSingleContentModel<string>
                        {
                            StatusCode = 200,
                            Message = "Thêm mới thành công",
                            Data = null
                        })
                        : (IActionResult)Ok(new ResponseSingleContentModel<string>
                        {
                            StatusCode = 500,
                            Message = "Thêm mới không thành công " + mess,
                            Data = null
                        });
                }
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("modify")]
        public async Task<IActionResult> RequestModify([FromBody] RequestModel model)
        {
            try
            {
                var validator = ValitRules<RequestModel>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userUpdated = userid(_httpContextAccessor);
                    string? mess = await _requestRepository.RequestModify(model);
                    return mess == "0"
                        ? Ok(new ResponseSingleContentModel<string>
                        {
                            StatusCode = 200,
                            Message = "Thêm mới thành công",
                            Data = null
                        })
                        : (IActionResult)Ok(new ResponseSingleContentModel<string>
                        {
                            StatusCode = 500,
                            Message = "Thêm mới không thành công " + mess,
                            Data = null
                        });
                }
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }

        [HttpPost("list")]
        public async Task<IActionResult> RequestList([FromBody] Warehouse_Request_Search search)
        {
            try
            {
                var products = await _requestRepository.RequestList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<RequestViewModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = products
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
      
    }
}
