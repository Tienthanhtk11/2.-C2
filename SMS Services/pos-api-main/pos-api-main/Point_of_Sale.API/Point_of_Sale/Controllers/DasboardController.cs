using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Dasboad;

namespace Point_of_Sale.Controllers
{
    [Route("api/dasboard")]
    [ApiController]
    public class DasboardController : BaseController
    {
        private readonly IDasboardRepository _dasboardRepository; 
        private IHttpContextAccessor _httpContextAccessor;

        public DasboardController(IDasboardRepository dasboardRepository,IHttpContextAccessor httpContextAccessor)
        {
            _dasboardRepository = dasboardRepository;
            _httpContextAccessor = httpContextAccessor; 
        }
         
        [HttpGet("info")]
        public async Task<IActionResult> Info(long id)
        {
            try
            {
                var info = await this._dasboardRepository.DasboardInfo(id);
                return Ok(new ResponseSingleContentModel<PosInfoModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = info
                });

            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<PosInfoModel>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            } 
        }

        [HttpPost("revenue-chart")]
        public async Task<IActionResult> Revenue(ChartSearch search)
        {
            try
            {
                var data = await this._dasboardRepository.DasboardRevenue(search);
                return Ok(new ResponseSingleContentModel<RevenueModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = data
                });

            }
            catch (Exception )
            {
                return Ok(new ResponseSingleContentModel<RevenueModel>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("top-product-chart")]
        public async Task<IActionResult> TopProduct(ChartSearch search)
        {
            try
            {
                var data = await this._dasboardRepository.DasboardTopProduct(search);
                return Ok(new ResponseSingleContentModel<List<TopProductModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = data
                }); 
            }
            catch (Exception )
            {
                return Ok(new ResponseSingleContentModel<List<TopProductModel>>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
    }
}
