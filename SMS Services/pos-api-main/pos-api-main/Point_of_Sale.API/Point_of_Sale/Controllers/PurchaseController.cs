using ECom.Framework.Validator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Purchase;

namespace Point_of_Sale.Controllers
{
    [Route("api/purchase")]
    [ApiController]
    public class PurchaseController : BaseController
    {
        private readonly IPurchaseRepository _purchaseRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public PurchaseController(IPurchaseRepository purchaseRepository, IHttpContextAccessor httpContextAccessor)
        {
            _purchaseRepository = purchaseRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("purchase")]
        public async Task<IActionResult> Purchase(long id)
        {
            try
            {
                var products = await this._purchaseRepository.Purchase(id);
                return Ok(new ResponseSingleContentModel<PurchaseModel>
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
        [HttpDelete("purchase-delete")]
        public async Task<IActionResult> PurchaseDelete(long id)
        {
            try
            {
                var mess = await this._purchaseRepository.PurchaseDelete(id);
                if (mess == "0")
                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Xóa bản ghi thành công!",
                        Data = null
                    });
                else
                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Xóa bản ghi không thành công " + mess,
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
        [HttpPost("purchase-create")]
        public async Task<IActionResult> PurchaseCreate([FromBody] PurchaseModel model)
        {
            try
            {
                var validator = ValitRules<PurchaseModel>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    var mess = await this._purchaseRepository.PurchaseCreate(model);
                    if (mess == "0")
                        return Ok(new ResponseSingleContentModel<string>
                        {
                            StatusCode = 200,
                            Message = "Thêm mới thành công",
                            Data = null
                        });
                    else
                        return Ok(new ResponseSingleContentModel<string>
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
        [HttpPost("purchase-modify")]
        public async Task<IActionResult> PurchaseModify([FromBody] PurchaseModel model)
        {
            try
            {
                model.userUpdated = userid(_httpContextAccessor);
                var mess = await this._purchaseRepository.PurchaseModify(model);
                if (mess == "0")
                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = null
                    });
                else
                    return Ok(new ResponseSingleContentModel<string>
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
        [HttpGet("purchase-list")]
        public async Task<IActionResult> PurchaseList(string? keyword, byte? status_id, DateTime? start_date, DateTime? end_date, int page_size, int page_number)
        {
            try
            {
                var products = await this._purchaseRepository.PurchaseList(keyword, status_id, start_date, end_date, page_size, page_number);
                return Ok(new ResponseSingleContentModel<PaginationSet<PurchaseViewModel>>
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

        [HttpGet("quantity-inventory")]
        public async Task<IActionResult> QuantityInventory(long warehouse_id, long product_id)
        {
            try
            {
                var quantity = await this._purchaseRepository.QuantityInventory(warehouse_id, product_id);
                return Ok(new ResponseSingleContentModel<double>
                {
                    StatusCode = 200,
                    Message = "Lấy số lượng thành công.",
                    Data = quantity
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
