using ECom.Framework.Validator;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model.Inventory;
using Point_of_Sale.Model;
using Point_of_Sale.Extensions;

namespace Point_of_Sale.Controllers
{
    [Route("api/warehouse-inventory")]
    [ApiController]
    public class WarehouseInventoryController : BaseController
    {
        private readonly IWarehouseInventoryRepository _inventoryRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public WarehouseInventoryController(IWarehouseInventoryRepository inventoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _inventoryRepository = inventoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("approve")]
        public async Task<IActionResult> Warehouse_InventoryApprove(long id)
        {
            try
            {
                var check = await this._inventoryRepository.Warehouse_InventoryConfirm(id, userid(_httpContextAccessor));
                return check
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Duyệt bản ghi thành công",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Không duyệt được phiếu kiểm kho này",
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
        [HttpGet("reject")]
        public async Task<IActionResult> Warehouse_InventoryReject(long id)
        {
            try
            {
                var news = await this._inventoryRepository.Warehouse_InventoryReject(id, userid(_httpContextAccessor));
                return news
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Từ chôi thành công",
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
        public async Task<IActionResult> Warehouse_InventoryDetail(long id)
        {
            try
            {
                var products = await this._inventoryRepository.Warehouse_InventoryDetail(id);
                return Ok(new ResponseSingleContentModel<Warehouse_Inventory_Model>
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
        public async Task<IActionResult> Warehouse_InventoryPrint(long id)
        {
            try
            {
                var products = await this._inventoryRepository.Warehouse_InventoryPrint(id);
                return Ok(new ResponseSingleContentModel<Warehouse_Inventory_Print_Model>
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
        public async Task<IActionResult> Warehouse_InventoryModify([FromBody] Warehouse_Inventory_Model model)
        {
            try
            { 
                model.userUpdated = userid(_httpContextAccessor);
                var response = await this._inventoryRepository.Warehouse_InventoryModify(model);
                if (response != new Warehouse_Inventory_Model())
                    return Ok(new ResponseSingleContentModel<Warehouse_Inventory_Model>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = response
                    });
                else
                    return Ok(new ResponseSingleContentModel<Warehouse_Inventory_Model>
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
        public async Task<IActionResult> Warehouse_InventoryCreate([FromBody] Warehouse_Inventory_Model model)
        {
            try
            {
                var validator = ValitRules<Warehouse_Inventory_Model>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                   
                    model.userAdded = userid(_httpContextAccessor);
                    var response = await this._inventoryRepository.Warehouse_InventoryCreate(model);
                    if (response != new Warehouse_Inventory_Model())
                        return Ok(new ResponseSingleContentModel<Warehouse_Inventory_Model>
                        {
                            StatusCode = 200,
                            Message = "Thêm mới thành công",
                            Data = response
                        });
                    else
                        return Ok(new ResponseSingleContentModel<Warehouse_Inventory_Model>
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

        [HttpPost("list")]
        public async Task<IActionResult> Warehouse_InventoryList([FromBody] Warehouse_Inventory_Search search)
        {
            try
            {
                var response = await this._inventoryRepository.Warehouse_InventoryList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<Warehouse_Inventory_Model>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = response
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
        public async Task<IActionResult> Warehouse_InventoryDelete(long id)
        {
            try
            {
                var news = await this._inventoryRepository.Warehouse_InventoryDelete(id);
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

    }
}
