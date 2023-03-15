using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Promotion;
using Point_of_Sale.Repositories;

namespace Point_of_Sale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : BaseController
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PromotionController(IPromotionRepository PromotionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _promotionRepository = PromotionRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Promotion(long id)
        {
            try
            {
                var Promotion = await this._promotionRepository.Promotion(id);
                return Ok(new ResponseSingleContentModel<PromotionModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Promotion
                });

            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CartCreate([FromBody] PromotionModel model)
        {
            try
            {
                model.userAdded = userid(_httpContextAccessor);
                var Promotions = await this._promotionRepository.PromotionCreate(model);
                return Ok(new ResponseSingleContentModel<PromotionModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Promotions
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("modify")]
        public async Task<IActionResult> PromotionModify([FromBody] PromotionModel model)
        {
            try
            {
                model.userUpdated = userid(_httpContextAccessor);
                var promotion = await this._promotionRepository.PromotionModify(model);
                return Ok(new ResponseSingleContentModel<PromotionModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = promotion
                });

            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("approve")]
        public async Task<IActionResult> PromotionApprove(long id)
        {
            try
            {   
                var user = userid(_httpContextAccessor);
                var response = await this._promotionRepository.PromotionApprove(id, user);
                return Ok(new ResponseSingleContentModel<bool>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });

            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("list")]
        public async Task<IActionResult> PromotionList(PromotionSearch search)
        {
            try
            {
                var Promotion = await this._promotionRepository.PromotionList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<PromotionModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Promotion
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> PromotionDelete(long id)
        {
            try
            {
                var promotion = await this._promotionRepository.PromotionDelete(id);
                return Ok(new ResponseSingleContentModel<bool>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = promotion
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
    }
}
