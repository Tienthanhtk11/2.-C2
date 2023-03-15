using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;

namespace Point_of_Sale.Controllers
{
    [Route("api/bank-account")]
    [ApiController]
    public class BankAccountController : BaseController
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        public BankAccountController(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }
      
        [HttpGet("list")]
        public async Task<IActionResult> BankAccountList()
        {
            try
            {
                var data = await this._bankAccountRepository.bankAccountList();
                return Ok(new ResponseSingleContentModel<List<BankAccount>>
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

        [HttpPost("create")]
        public async Task<IActionResult> BankAccountCreate([FromBody] BankAccount model)
        {
            try
            {
                var data = await this._bankAccountRepository.bankAccountCreate(model);
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

        [HttpPost("modify")]
        public async Task<IActionResult> BankAccountModify([FromBody] BankAccount model)
        {
            try
            {
                var data = await this._bankAccountRepository.bankAccountModify(model);
                return Ok(new ResponseSingleContentModel<BankAccount>
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

        [HttpGet("delete")]
        public async Task<IActionResult> BankAccountDelete(long id)
        {
            try
            {
                var data = await this._bankAccountRepository.bankAccountDelete(id);
                if (data == true)
                {
                    return Ok(new ResponseSingleContentModel<bool>
                    {
                        StatusCode = 200,
                        Message = "Success",
                        Data = true
                    });
                }
                else
                    return this.RouteToInternalServerError();
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();

            }
        }

    }
}
