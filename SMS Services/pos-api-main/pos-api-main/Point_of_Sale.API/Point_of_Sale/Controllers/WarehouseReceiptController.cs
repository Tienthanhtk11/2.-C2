using ECom.Framework.Validator;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Receipt;

namespace Point_of_Sale.Controllers
{
    [Route("api/warehouse-receipt")]
    [ApiController]
    public class WarehouseReceiptController : BaseController
    {
        private readonly IReceiptRepository _receiptRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public WarehouseReceiptController(IReceiptRepository receiptRepository, IHttpContextAccessor httpContextAccessor)
        {
            _receiptRepository = receiptRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Receipt(long id)
        {
            try
            {
                var products = await this._receiptRepository.Receipt(id);
                return Ok(new ResponseSingleContentModel<ReceiptModel>
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

       [HttpGet("get-barcode")]
        public async Task<IActionResult> GetBarcode(long id)
        {
            try
            {
                var barcode = await this._receiptRepository.GetBarcode(id);
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = barcode
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
        public async Task<IActionResult> ReceiptModify([FromBody] ReceiptModel model)
        {
            try
            {
                model.userUpdated = userid(_httpContextAccessor);
                var mess = await this._receiptRepository.ReceiptModify(model);
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
        public async Task<IActionResult> RequestCreate([FromBody] ReceiptModel model)
        {
            try
            {
                var validator = ValitRules<ReceiptModel>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    var mess = await this._receiptRepository.ReceiptCreate(model);
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
        public async Task<IActionResult> ReceiptList(SearchReceipt search)
        {
            try
            {
                var products = await this._receiptRepository.ReceiptList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<ReceiptViewModel>>
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
        [HttpDelete("delete")]
        public async Task<IActionResult> ReceiptDelete(long id)
        {
            try
            {
                var news = await this._receiptRepository.ReceiptDelete(id);
                return news
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Xóa bản ghi thành công",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Bản ghi không tồn tại hoặc bị xóa trước đó",
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

        [HttpGet("print")]
        public async Task<IActionResult> ReceiptExportPrint(long id)
        {
            try
            {
                var products = await this._receiptRepository.ReceiptExportPrint(id);
                return Ok(new ResponseSingleContentModel<Receipt_Print_Model>
                {
                    StatusCode = 200,
                    Message = "Lấy thông tin thành công.",
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
