using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;

namespace Point_of_Sale.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherController : BaseController
    {
        private readonly IVoucherRepository _voucherRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public VoucherController(IVoucherRepository voucherRepository, IHttpContextAccessor httpContextAccessor)
        {
            _voucherRepository = voucherRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("detail")]
        public async Task<IActionResult> VoucherGetById(long id)
        {
            try
            {
                var data = await this._voucherRepository.VoucherGetById(id);
                return Ok(new ResponseSingleContentModel<VoucherModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = data
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();

            }
        }
        [HttpPost("list")]
        public async Task<IActionResult> VoucherList([FromBody] VoucherSearch search)
        {
            try
            {
                var data = await this._voucherRepository.VoucherList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<VoucherModel>>
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

        //[Authorize(Roles = "QUANLYVOUCHER")]
        [HttpPost("create")]
        public async Task<IActionResult> VoucherCreate([FromBody] VoucherModel model)
        {
            try
            {
                model.userAdded = userid(_httpContextAccessor);
                var data = await this._voucherRepository.VoucherCreate(model);
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = data
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }

        //[Authorize(Roles = "QUANLYVOUCHER")]
        [HttpPost("modify")]
        public async Task<IActionResult> VoucherModify([FromBody] VoucherModel model)
        {
            try
            {
                model.userUpdated = userid(_httpContextAccessor);
                var data = await this._voucherRepository.VoucherModify(model);
                return Ok(new ResponseSingleContentModel<VoucherModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = data
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }

        //[Authorize(Roles = "QUANLYVOUCHER")]
        [HttpGet("delete")]
        public async Task<IActionResult> VoucherDelete(long id)
        {
            try
            {
                var data = await this._voucherRepository.VoucherDelete(id);
                return data == true
                    ? Ok(new ResponseSingleContentModel<bool>
                    {
                        StatusCode = 200,
                        Message = "Success",
                        Data = true
                    })
                    : this.RouteToInternalServerError();
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();

            }
        }


    }
}
