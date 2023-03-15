using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Product;
using static Humanizer.On;

namespace Point_of_Sale.Controllers
{
    [Route("api/partner")]
    [ApiController]
    public class PartnerController : BaseController
    {
        private readonly IPartnerRepository _PartnerRepository;
        public PartnerController(IPartnerRepository PartnerRepository)
        {
            _PartnerRepository = PartnerRepository;
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Partner(long id)
        {
            try
            {
                var Partner = await this._PartnerRepository.Partner(id);
                return Ok(new ResponseSingleContentModel<PartnerModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Partner
                });

            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> PartnerCreate([FromBody] PartnerModel PartnerModels)
        {
            try
            {
                var Partners = await this._PartnerRepository.PartnerCreate(PartnerModels);
                if (Partners == null)
                {
                    return this.RouteToInternalServerError();
                }
                return Ok(new ResponseSingleContentModel<PartnerModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Partners
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("modify")]
        public async Task<IActionResult> PartnerModify([FromBody] PartnerModel PartnerModel)
        {
            try
            {
                var Partner = await this._PartnerRepository.PartnerModify(PartnerModel);
                if (Partner == null)
                    return Ok(new ResponseSingleContentModel<PartnerModel>
                    {
                        StatusCode = 500,
                        Message = "Không được cập nhật NCC trùng tên, MST",
                        Data = Partner
                    });
                else
                    return Ok(new ResponseSingleContentModel<PartnerModel>
                    {
                        StatusCode = 200,
                        Message = "Success",
                        Data = Partner
                    });

            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("list")]
        public async Task<IActionResult> ShopList(SearchBase search)
        {
            try
            {
                var Partner = await this._PartnerRepository.PartnerList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<PartnerModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Partner
                });
            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpGet("list-for-pms")]
        public async Task<IActionResult> PartnerList()
        {
            try
            {
                var Partner = await this._PartnerRepository.PartnerListAll();
                return Ok(Partner);
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();


            }
        }
    }
}
