﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SMS_Services.Entity;
using SMS_Services.Model;
using SMS_Services.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SMS_Services.Controllers
{
    [Route("api/customer/")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public CustomerController(ICustomerRepository Repository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this._config = configuration;
            this._repository = Repository;
            this._httpContextAccessor = httpContextAccessor;
            this._config = configuration;
        }
        #region Customer
        [HttpPost("create")]
        public async Task<IActionResult> CustomerCreate([FromBody] Customer model)
        {
            try
            {
                var response = await this._repository.CustomerCreate(model);
                return Ok(new ResponseSingleContentModel<Customer>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = response
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CustomerRegister([FromBody] Customer model)
        {
            try
            {
                var response = await this._repository.CustomerCreate(model);
                if (response != null)
                {
                    return Ok(new ResponseSingleContentModel<Customer>
                    {
                        StatusCode = 200,
                        Message = "",
                        Data = response
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
        [HttpPost("modify")]
        public async Task<IActionResult> CustomerModify([FromBody] Customer model)
        {
            try
            {
                var Customer = await this._repository.CustomerModify(model);
                return Ok(new ResponseSingleContentModel<Customer>
                {
                    StatusCode = 200,
                    Message = "Cập nhật thành công",
                    Data = model
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
        public async Task<IActionResult> CustomerList(string? Customer_name)
        {
            try
            {
                var response = await _repository.CustomerList(Customer_name);

                return Ok(new ResponseSingleContentModel<List<Customer>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<List<Customer>>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = new()
                });
            }
        }
        [AllowAnonymous]

        [HttpGet("get-sms-request")]
        public async Task<IActionResult> GetSMSRequest(long customer_id)
        {
            try
            {
                var response = await _repository.GetSMSRequest(customer_id);

                return Ok(new ResponseSingleContentModel<SMS_Request_Customer>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<SMS_Request_Customer>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpGet("list-sms-template")]
        public async Task<IActionResult> ListSMSTemplate(long customer_id)
        {
            try
            {
                var response = await _repository.ListSMSTemplate(customer_id);

                return Ok(new ResponseSingleContentModel<List<SMS_Template>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<List<SMS_Template>>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [AllowAnonymous]
        [HttpGet("list-sms-request")]
        public async Task<IActionResult> ListSMSRequest(long customer_id)
        {
            try
            {
                var response = await _repository.ListSMSRequest(customer_id);
                return Ok(new ResponseSingleContentModel<List<SMS_Request_Customer>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<List<SMS_Template>>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = null
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("ping")]
        public async Task<IActionResult> CustomerPing(long customer_id)
        {
            _repository.CustomerPing(customer_id);
            return Ok();

        }
        [AllowAnonymous]
        [HttpGet("list-config-port")]
        public async Task<IActionResult> Config_Port_List(long customer_id)
        {
            var response = await _repository.Config_Port_List(customer_id);
            return Ok(new ResponseSingleContentModel<List<Config_Port>>
            {
                StatusCode = 200,
                Message = "Success",
                Data = response
            });
        }
        [AllowAnonymous]
        [HttpGet("list-phone-number")]
        public async Task<IActionResult> Get_Customer_Config_Phone_Number(long customer_id)
        {
            try
            {

                var response = await _repository.Get_Customer_Config_Phone_Number(customer_id);
                return Ok(new ResponseSingleContentModel<List<Customer_Config_Phone_Number>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = string.Empty
                });
            }
        }
        [AllowAnonymous]
        [HttpGet("list-all-phone-number")]
        public async Task<IActionResult> Get_List_Phone_Number()
        {
            try
            {

                var response = await _repository.Get_List_Phone_Number();
                return Ok(new ResponseSingleContentModel<List<Phone>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = string.Empty
                });
            }
        }
        [AllowAnonymous]
        [HttpPost("create-phone-number")]
        public async Task<IActionResult> Customer_Config_Phone_Number_Create(List<Customer_Config_Phone_Number> model)
        {
            try
            {
                var response = await _repository.Customer_Config_Phone_Number_Create(model);
                return Ok(new ResponseSingleContentModel<List<Customer_Config_Phone_Number>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = string.Empty
                });
            }
        }


        [AllowAnonymous]
        [HttpPost("config-port-create")]
        public async Task<IActionResult> Config_Port_Create([FromBody] Config_Port model)
        {
            var response = await _repository.Config_Port_Create(model);
            return Ok(new ResponseSingleContentModel<Config_Port>
            {
                StatusCode = 200,
                Message = "Success",
                Data = response
            });
        }
        [AllowAnonymous]
        [HttpPost("config-port-modify")]
        public async Task<IActionResult> Config_Port_Modify([FromBody] Config_Port model)
        {
            var response = await _repository.Config_Port_Modify(model);
            return Ok(new ResponseSingleContentModel<Config_Port>
            {
                StatusCode = 200,
                Message = "Success",
                Data = response
            });
        }


        [AllowAnonymous]
        [HttpPost("create-sms-request")]
        public async Task<IActionResult> CreateSMSRequest([FromBody] List<SMS_Request_Customer> model)
        {
            try
            {

                var response = await _repository.CreateSMSRequest(model);
                return Ok(new ResponseSingleContentModel<bool>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = string.Empty
                });
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> CustomerLogin(LoginModel login)
        {
            try
            {

                Customer user = await _repository.Customer_Check(login.user_name);
                if (user != null)
                {
                    int checkAccount = _repository.Customer_Authenticate(login);
                    CustomerTokenModel customerAuthen = new();
                    if (checkAccount == 1)
                    {
                        ClaimModel claim = new ClaimModel
                        {
                            email = user.email,
                            name = user.name,
                            id = user.id,
                            user_name = user.user_name,

                        };
                        string tokenString = GenerateToken(claim);
                        customerAuthen.token = tokenString;
                        customerAuthen.id = user.id;
                        customerAuthen.license_exp = user.license_exp;
                        customerAuthen.license_key = user.license_key;
                        customerAuthen.user_name = user.user_name;
                        customerAuthen.name = user.name;
                        customerAuthen.token = tokenString;
                        return Ok(new ResponseSingleContentModel<CustomerTokenModel>
                        {
                            StatusCode = 200,
                            Message = "Đăng nhập thành công",
                            Data = customerAuthen
                        });
                    }
                    else
                    {
                        return Ok(new ResponseSingleContentModel<string>
                        {
                            StatusCode = 500,
                            Message = "Sai tài khoản hoặc mật khẩu",
                            Data = null
                        });
                    }
                }
                else
                {
                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Tài khoản không tồn tại trong hệ thống",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = string.Empty
                });
            }
        }
        private string GenerateToken(ClaimModel user)
        {
            var identity = GetClaims(user);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["TokenSettings:Key"]));
            var token = new JwtSecurityToken(
              _config["TokenSettings:Issuer"],
              _config["TokenSettings:Audience"],
              expires: DateTime.Now.AddHours(9),
              claims: identity,
              signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
              );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private IEnumerable<Claim> GetClaims(ClaimModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.user_name.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.name),
                new Claim(JwtRegisteredClaimNames.Email, user.email),
                new Claim(JwtRegisteredClaimNames.Sid, user.id.ToString())
            };
            return claims;
        }
        #endregion
    }
}
