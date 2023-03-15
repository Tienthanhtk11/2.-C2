using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Customer;

namespace Point_of_Sale.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerRepository _CustomerRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public CustomerController(ICustomerRepository CustomerRepository, IHttpContextAccessor httpContextAccessor)
        {
            _CustomerRepository = CustomerRepository;
            _httpContextAccessor = httpContextAccessor;

        }
        [HttpGet("detail")]
        public async Task<IActionResult> Customer(long id)
        {
            try
            {
                var Customer = await this._CustomerRepository.Customer(id);
                return Ok(new ResponseSingleContentModel<CustomerModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Customer
                });

            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [AllowAnonymous]
        [HttpGet("list-customer-point")]
        public async Task<IActionResult> List_Customer_Point(long customer_id, int page_size, int page_number)
        {
            try
            {
                var response = await this._CustomerRepository.List_Customer_Point(customer_id, page_size, page_number);
                return Ok(new ResponseSingleContentModel<PaginationSet<Customer_Point_History_Model>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });

            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpDelete("config-delete")]
        public async Task<IActionResult> ConfigDelete(long config_id)
        {
            try
            {
                long user_id = userid(_httpContextAccessor);
                var response = await this._CustomerRepository.ConfigDelete(config_id, user_id);
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });

            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }

        [HttpPost("config-create")]
        public async Task<IActionResult> ConfigCreate(Customer_Member_Config config)
        {
            try
            {
                var response = await this._CustomerRepository.ConfigCreate(config);
                return Ok(new ResponseSingleContentModel<Customer_Member_Config>
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
        [HttpPost("config-update")]
        public async Task<IActionResult> ConfigUpdate(Customer_Member_Config config)
        {
            try
            {
                var response = await this._CustomerRepository.ConfigUpdate(config);
                return Ok(new ResponseSingleContentModel<Customer_Member_Config>
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
        [HttpGet("config-detail")]
        public async Task<IActionResult> ConfigDetail(long config_id)
        {
            try
            {
                var response = await this._CustomerRepository.ConfigDetail(config_id);
                return Ok(new ResponseSingleContentModel<Customer_Member_Config>
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
        [HttpGet("config-active")]
        public async Task<IActionResult> ConfigActive()
        {
            try
            {
                var response = await this._CustomerRepository.ConfigActive();
                return Ok(new ResponseSingleContentModel<Customer_Member_Config>
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
        [HttpGet("config-list")]
        public async Task<IActionResult> ConfigList()
        {
            try
            {
                var response = await this._CustomerRepository.ConfigList();
                return Ok(new ResponseSingleContentModel<List<Customer_Member_Config>>
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
        [HttpPost("create")]
        public async Task<IActionResult> CustomerCreate([FromBody] CustomerModel CustomerModels)
        {
            try
            {
                var Customers = await this._CustomerRepository.CustomerCreate(CustomerModels);
                return Ok(new ResponseSingleContentModel<CustomerModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Customers
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("modify")]
        public async Task<IActionResult> CustomerModìy([FromBody] CustomerModel CustomerModel)
        {
            try
            {
                var Customer = await this._CustomerRepository.CustomerModify(CustomerModel);
                return Ok(new ResponseSingleContentModel<CustomerModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Customer
                });

            }
            catch (Exception ex)
            {
                return this.RouteToInternalServerError();
            }
        }
        [AllowAnonymous]
        [HttpPost("list")]
        public async Task<IActionResult> CustomerList(SearchBase searchBase)
        {
            try
            {
                var Customer = await this._CustomerRepository.CustomerList(searchBase);
                return Ok(new ResponseSingleContentModel<PaginationSet<CustomerModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Customer
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();


            }
        }
    }
}
