using ECom.Framework.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Warehouse;

namespace Point_of_Sale.Controllers
{
    [Route("api/warehouse")]
    [ApiController]
    public class WarehouseController : BaseController
    {
        private readonly IWarehouseRepository _warehouseRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public WarehouseController(IWarehouseRepository warehouseRepostiry, IHttpContextAccessor httpContextAccessor)
        {
            _warehouseRepository = warehouseRepostiry;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("detail")]
        public async Task<IActionResult> Warehouse(long id)
        {
            try
            {
                var warehouse = await this._warehouseRepository.Warehouse(id);
                return Ok(new ResponseSingleContentModel<Warehouse>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = warehouse
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
        public async Task<IActionResult> WarehouseCreate([FromBody] Warehouse_Model model)
        {
            try
            {
                var validator = ValitRules<Warehouse_Model>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    if (await this._warehouseRepository.WarehouseCheckDuplicate(model))
                    {
                        return Ok(new ResponseSingleContentModel<Warehouse>
                        {
                            StatusCode = 400,
                            Message = "Đã tồn tại kho",
                            Data = null
                        });
                    }
                    model.userAdded = userid(_httpContextAccessor);
                    var warehouse = await this._warehouseRepository.WarehouseCreate(model);
                    return Ok(new ResponseSingleContentModel<Warehouse>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = warehouse
                    });
                }
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
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
        [HttpPost("modify")]
        public async Task<IActionResult> WarehouseModify([FromBody] Warehouse_Model model)
        {
            try
            {
                var validator = ValitRules<Warehouse_Model>
                    .Create()
                    //.Ensure(m => m.name, rule => rule.Required())
                    // .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    //.Ensure(m => m.note, rule => rule.Required())
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    if (await this._warehouseRepository.WarehouseCheckDuplicate(model))
                    {
                        return Ok(new ResponseSingleContentModel<Warehouse>
                        {
                            StatusCode = 400,
                            Message = "Đã tồn tại kho",
                            Data = null
                        });
                    }
                    model.userUpdated = userid(_httpContextAccessor);
                    var warehouse = await this._warehouseRepository.WarehouseModify(model);

                    return Ok(new ResponseSingleContentModel<Warehouse>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = warehouse
                    });
                }

                // Return invalidate data
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
        [AllowAnonymous]
        [HttpPost("list")]
        public async Task<IActionResult> WarehouseList(SearchBase searchBase)
        {
            try
            {
                var pagination = await this._warehouseRepository.WarehouseList( searchBase);
                return Ok(new ResponseSingleContentModel<PaginationSet<Warehouse_Model>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = pagination
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
        [HttpGet("user-warehouse-list")]
        public async Task<IActionResult> WarehouseUserList()
        {
            try
            {
                long user_id = userid(_httpContextAccessor);
                var list = await this._warehouseRepository.WarehouseUserList(user_id);
                return Ok(new ResponseSingleContentModel<List<UserWarehouseModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = list
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
