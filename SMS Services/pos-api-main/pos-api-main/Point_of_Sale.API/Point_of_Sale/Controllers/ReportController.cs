using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Report;

namespace Point_of_Sale.Controllers
{
    [Route("api/report")]
    [ApiController]
    //[Authorize(Roles = "QUANLYBAOCAO")]
    public class ReportController : BaseController
    {
        private readonly IReportRepository _reportRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportController(IReportRepository reportRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reportRepository = reportRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        [HttpPost("revenue-products")]
        public async Task<IActionResult> RevenueProduct([FromBody] RevenueProductSearchModel search)
        {
            try
            {
                var report = await this._reportRepository.RevenueProducts(search);
                return Ok(new ResponseSingleContentModel<RevenueProductModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });

            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<RevenueProductModel>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }

        }
        [HttpPost("in-out-products")]
        public async Task<IActionResult> InOutInventoryProducts([FromBody] InOutInventorySearchModel search)
        {
            try
            {
                var report = await this._reportRepository.InOutInventoryProducts(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<InOutInventoryProductModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<PaginationSet<InOutInventoryProductModel>>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }

        }
        [HttpPost("history-inventory-product")]
        public async Task<IActionResult> HistoryInventoryProducts([FromBody] HistoryInventorySearchModel search)
        {
            try
            {
                var report = await this._reportRepository.HistoryInventoryProducts(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<HistoryInventoryProductModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }

        }
        [HttpGet("daily-import-product")]
        public async Task<IActionResult> Daily_Import_Product_Report(long warehouse_id, int type, DateTime start_date, DateTime end_date)
        {
            try
            {
                var report = await this._reportRepository.Daily_Import_Product_Report(warehouse_id, type, start_date, end_date);
                return Ok(new ResponseSingleContentModel<List<Import_Export_Product_Model>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }

        }
        [HttpGet("daily-export-product")]
        public async Task<IActionResult> Daily_Export_Product_Report(long warehouse_id, int type, DateTime start_date, DateTime end_date)
        {
            try
            {
                var report = await this._reportRepository.Daily_Export_Product_Report(warehouse_id, type, start_date, end_date);
                return Ok(new ResponseSingleContentModel<List<Import_Export_Product_Model>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }

        }
        [HttpGet("order-export")]
        public async Task<IActionResult> Daily_Order_List(long warehouse_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                var report = await this._reportRepository.Daily_Order_List(warehouse_id, start_date, end_date);
                return Ok(new ResponseSingleContentModel<Revenue_Book_Report>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }

        }
        [HttpGet("session-export")]
        public async Task<IActionResult> SalesSessionList(long warehouse_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                var report = await this._reportRepository.Sales_SessionList(warehouse_id, start_date, end_date);
                return Ok(new ResponseSingleContentModel<List<Sale_Session_Report>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpGet("customer-revenues")]
        public async Task<IActionResult> Customer_Revenues(DateTime start_date, DateTime end_date)
        {
            try
            {
                var report = await this._reportRepository.Customer_Revenues(start_date, end_date);
                return Ok(new ResponseSingleContentModel<List<Customer_Revenue>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [AllowAnonymous]
        [HttpGet("category-revenues")]
        public async Task<IActionResult> Category_Revenues(DateTime start_date, DateTime end_date)
        {
            try
            {
                var report = await this._reportRepository.Category_Revenues(start_date, end_date);
                return Ok(new ResponseSingleContentModel<List<Category_Revenue>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpGet("product-import-fast")]
        public async Task<IActionResult> Product_Import_Fast(long warehouse_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                var report = await this._reportRepository.Product_Import_Fast(warehouse_id, start_date, end_date);
                return Ok(new ResponseSingleContentModel<List<Export_Product_Fast_Model>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpGet("receipt-cash-form-list")]
        public async Task<IActionResult> Receipt_Cash_Form_List(long warehouse_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                var report = await this._reportRepository.Receipt_Cash_Form_List(warehouse_id, start_date, end_date);
                return Ok(new ResponseSingleContentModel<List<Receipt_Cash_Form>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpGet("product-export-fast")]
        public async Task<IActionResult> Product_Export_Fast(long warehouse_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                var report = await this._reportRepository.Product_Export_Fast(warehouse_id, start_date, end_date);
                return Ok(new ResponseSingleContentModel<List<Export_Product_Fast_Model>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpGet("product-warehouse-fast")]
        public async Task<IActionResult> Warehouse_Product_Fast(DateTime dateTime)
        {
            try
            {
                var report = await this._reportRepository.Warehouse_Product_Fast(dateTime);
                return Ok(new ResponseSingleContentModel<List<Stock_Product>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpGet("product-warehouse-inventory-fast")]
        public async Task<IActionResult> Warehouse_Product_Inventory_Fast(DateTime dateTime)
        {
            try
            {
                var report = await this._reportRepository.Warehouse_Product_Inventory_Fast(dateTime);
                return Ok(new ResponseSingleContentModel<List<Stock_Product>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
    }
}