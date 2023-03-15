using ECom.Framework.Validator;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Export;

namespace Point_of_Sale.Controllers
{
    [Route("api/warehouse-export")]
    [ApiController]
    public class WarehouseExportController : BaseController
    {
        private readonly IWarehouseExportRepository _exportRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public WarehouseExportController(IWarehouseExportRepository exportRepository, IHttpContextAccessor httpContextAccessor)
        {
            _exportRepository = exportRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("approve")]
        public async Task<IActionResult> Warehouse_ExportApprove(long id)
        {
            try
            {
                var news = await this._exportRepository.Warehouse_ExportApprove(id, userid(_httpContextAccessor));
                return news
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Xuất kho thành công",
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

        [HttpGet("detail")]
        public async Task<IActionResult> Warehouse_ExportDetail(long id)
        {
            try
            {
                var products = await this._exportRepository.Warehouse_ExportDetail(id);
                return Ok(new ResponseSingleContentModel<Warehouse_Export_Model>
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
        [HttpGet("print")]
        public async Task<IActionResult> Warehouse_ExportPrint(long id)
        {
            try
            {
                var products = await this._exportRepository.Warehouse_ExportPrint(id);
                return Ok(new ResponseSingleContentModel<Warehouse_Export_Print_Model>
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
        [HttpPost("modify")]
        public async Task<IActionResult> Warehouse_ExportModify([FromBody] Warehouse_Export_Model model)
        {
            try
            {
                var check = await this._exportRepository.Warehouse_ExportVerify(model.warehouse_id, model.warehouse_destination_id ?? 0, model.type);
                if (!string.IsNullOrEmpty(check))
                {
                    return Ok(new ResponseSingleContentModel<Warehouse_Export_Model>
                    {
                        StatusCode = 500,
                        Message = check,
                        Data = null
                    });
                }
                model.userUpdated = userid(_httpContextAccessor);
                var response = await this._exportRepository.Warehouse_ExportModify(model);
                if (response != new Warehouse_Export_Model())
                    return Ok(new ResponseSingleContentModel<Warehouse_Export_Model>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = response
                    });
                else
                    return Ok(new ResponseSingleContentModel<Warehouse_Export_Model>
                    {
                        StatusCode = 500,
                        Message = "Cập nhật không thành công ",
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
        public async Task<IActionResult> Warehouse_ExportCreate([FromBody] Warehouse_Export_Model model)
        {
            try
            {
                var validator = ValitRules<Warehouse_Export_Model>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    var check = await this._exportRepository.Warehouse_ExportVerify(model.warehouse_id, model.warehouse_destination_id ?? 0, model.type);
                    if (!string.IsNullOrEmpty(check))
                    {
                        return Ok(new ResponseSingleContentModel<Warehouse_Export_Model>
                        {
                            StatusCode = 500,
                            Message = check,
                            Data = null
                        });
                    }
                    model.userAdded = userid(_httpContextAccessor);
                    var response = await this._exportRepository.Warehouse_ExportCreate(model);
                    if (response != new Warehouse_Export_Model())
                        return Ok(new ResponseSingleContentModel<Warehouse_Export_Model>
                        {
                            StatusCode = 200,
                            Message = "Thêm mới thành công",
                            Data = response
                        });
                    else
                        return Ok(new ResponseSingleContentModel<Warehouse_Export_Model>
                        {
                            StatusCode = 500,
                            Message = "Thêm mới không thành công ",
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

        [HttpGet("list")]
        public async Task<IActionResult> Warehouse_ExportList(long partner_id, long warehouse_id, byte? status_id, string? keyword, int page_number, int page_size, DateTime start_date, DateTime end_date)
        {
            try
            {
                var response = await this._exportRepository.Warehouse_ExportList(partner_id, warehouse_id, status_id, keyword, page_number, page_size, start_date, end_date);
                return Ok(new ResponseSingleContentModel<PaginationSet<Warehouse_ExportViewModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = response
                });
            }
            catch (Exception ex)
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
        public async Task<IActionResult> Warehouse_ExportDelete(long id)
        {
            try
            {
                var news = await this._exportRepository.Warehouse_ExportDelete(id);
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
        [HttpGet("confirm")]
        public async Task<IActionResult> Warehouse_ExportConfirm(long id)
        {
            try
            {
                var news = await this._exportRepository.Warehouse_ExportConfirm(id, userid(_httpContextAccessor));
                return news
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Duyệt bản ghi thành công",
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
    }
}
